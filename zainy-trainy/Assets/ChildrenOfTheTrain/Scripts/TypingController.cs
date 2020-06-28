using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingController : MonoBehaviour
{
    private InputField input;
    public CircleController[] circles;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<InputField>();
        input.ActivateInputField();
        input.onValueChanged.AddListener(SubmitAttack);
    }

    public void KeepFocus()
    {
        if (!input.isFocused)
        {
            input.ActivateInputField();
        }
    }

    // Update is called once per frame
    void Update()
    {
        KeepFocus();
    }

    void SubmitAttack(string text)
    {
        if (text.Length > 0)
        {
            foreach (CircleController circle in circles)
                circle.CheckInput(text[0]);
            input.text = "";
        }
    }
}
