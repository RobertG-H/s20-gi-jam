﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FunctionLineController : MonoBehaviour
{
    [HideInInspector]
    public RobotUprising robotUprisingController;
    public bool isBad;
    public bool isSyntaxError;
    public Color goodColor;
    public Color badColor;
    public TMP_InputField inputField;
    public string playerFunctionString;
    private string defaultString;
    private bool defaultBad;

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
        if(robotUprisingController.usedGoodCodeLines.Contains(playerFunctionString))
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
        if(robotUprisingController.goodCodeLines.Contains(playerFunctionString))
        {
            if(!robotUprisingController.usedGoodCodeLines.Contains(playerFunctionString))
            {
                robotUprisingController.UseGoodCodeLine(playerFunctionString);
                isBad = false;
            }
            else //Player already entered that string
            {
                //Play some kind of animation
                SetToDefaultString();
            }
        }
        else
        {
            //Play syntax error animation
            robotUprisingController.GoToFunction(this);
        }
    }

    public void ShowResult()
    {
        if(isBad)
        {
            inputField.image.color = badColor;
        }
        else if(isSyntaxError)
        {
            inputField.image.color = Color.cyan;
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
        isSyntaxError = false;
    }

    void OnDestroy()
    {
        inputField.onDeselect.RemoveListener(UpdatePlayerFunctionString);
    }
}
