using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FunctionLineController : MonoBehaviour
{
    [HideInInspector]
    public RobotUprising robotUprisingController;
    public Animator anim;
    public bool isBad;
    public bool isSyntaxError = false;
    public bool isDuplicate = false;
    public Color goodColor;
    public Color badColor;
    public TMP_InputField inputField;
    public string playerFunctionString;

    
    private string defaultString;
    public bool defaultBad;

    void Awake()
    {
        inputField.onDeselect.AddListener(UpdatePlayerFunctionString);
    }
    // Start is called before the first frame update
    void Start()
    {
        inputField.lineLimit = 1;
    }
    public void SetFunction(string text, bool isBad)
    {
        defaultBad = isBad;
        defaultString = text;
        SetToDefaultString();
    }
    public void UpdatePlayerFunctionString(string newPlayerFunctionString)
    {
        if(robotUprisingController.usedGoodFunctions.Contains(playerFunctionString) && !isDuplicate)
        {
            robotUprisingController.ReleaseGoodCodeLIne(playerFunctionString);
        }
        if(newPlayerFunctionString == string.Empty)//Player deleted and then entered nothing, re-enter the default string
        {
            SetToDefaultString();
        }
        else if(newPlayerFunctionString != defaultString)//Player enterd a non-default, non-empty string
        {
            playerFunctionString = newPlayerFunctionString;
            CheckGoodAndUnique();
        }
    }

    public void CheckGoodAndUnique()
    {
        if(robotUprisingController.goodFunctions.Contains(playerFunctionString))
        {
            if(!robotUprisingController.usedGoodFunctions.Contains(playerFunctionString))
            {
                robotUprisingController.UseGoodCodeLine(playerFunctionString);
                isBad = false;
                isDuplicate = false;
                isSyntaxError = false;
            }
            else //Player already entered that string
            {
                anim.Play("Shake");
                isDuplicate = true;
                robotUprisingController.GoToFunction(this);
            }
        }
        else
        {
            anim.Play("Shake");
            isSyntaxError = true;
            robotUprisingController.GoToFunction(this);
        }
    }

    public void ShowResult()
    {
        if(isBad)
        {
            inputField.image.color = badColor;
        }
        else
        {
            inputField.image.color = goodColor;
        }
    }
    public void Tab(int tabs)
    {
        Vector4 newMargin = inputField.textComponent.margin;
        newMargin.x = 100 * tabs;
        inputField.textComponent.margin = newMargin;
    }

    public void SetToDefaultString()
    {
        inputField.text = defaultString;
        playerFunctionString = defaultString;
        isBad = defaultBad;
        isDuplicate = false;
        isSyntaxError = false;
    }

    public void PlayTypingSound()
    {
        if(robotUprisingController != null)
            robotUprisingController.audioController.PlayType();
    }
    void OnDestroy()
    {
        inputField.onDeselect.RemoveListener(UpdatePlayerFunctionString);
    }
}
