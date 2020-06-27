using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FunctionLineController : MonoBehaviour
{
    public RobotUprising robotUprisingController;
    public bool isBad;
    public Color goodColor;
    public Color badColor;
    public TMP_InputField inputField;
    public TextMeshProUGUI originalFunctionTMP;
    public TextMeshProUGUI playerFunctionTMP;
    public Image textBackground;
    public string playerFunctionString;

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
        originalFunctionTMP.SetText(text);
        this.isBad = isBad;
    }
    public void UpdatePlayerFunctionString(string newPlayerFunctionString)
    {
        playerFunctionString = newPlayerFunctionString;
    }

    public void ShowCorrect()
    {
        if(string.IsNullOrEmpty(playerFunctionString) && isBad)
        {
            inputField.image.color = badColor;
        }
        else if(robotUprisingController.goodCodeLines.Contains(playerFunctionString) && isBad)
        {
            inputField.image.color = goodColor;
        }
        else
        {
            inputField.image.color = goodColor;
        }
    }
    public void Tab(int tabs)
    {
        Vector4 newMargin = playerFunctionTMP.margin;
        newMargin.x = 100 * tabs;
        playerFunctionTMP.margin = newMargin;

        newMargin = originalFunctionTMP.margin;
        newMargin.x = 100 * tabs;
        originalFunctionTMP.margin = newMargin;
    }

    void OnDestroy()
    {
        inputField.onDeselect.RemoveListener(UpdatePlayerFunctionString);
    }
}
