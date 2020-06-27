using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class RobotUprising : MonoBehaviour
{
	//No scrolling
	//Once a page is complete it goes to the next one
	//Same # of good functions as total functions
	public GameObject functionLinePrefab;
	public GameObject codeLinePrefab;
	public RectTransform codeContainer;
	[SerializeField]
	private DemoModuleManager moduleManager;

	[SerializeField]
	private List<CodeBlock> codeBlocks;
	public List<string> goodCodeLines;
	public List<string> usedGoodCodeLines;
	private List<FunctionLineController> lineFunctions;
	private int currentLine = 0;
	public float initialCodeYPos;
	public float codeYPosIncrease;
	private float codeYPos;
	private bool ctrlPressed, sPressed;
	
	 
    // Start is called before the first frame update
    void Start()
    {
		lineFunctions = new List<FunctionLineController>();
		usedGoodCodeLines = new List<string>();
		codeYPos = initialCodeYPos;
        foreach(CodeBlock cb in codeBlocks)
		{
			foreach(CodeBlock.line line in cb.lines)
			{
				if(line.isFunction)
				{
					FunctionLineController funcLineCon = Instantiate(functionLinePrefab, codeContainer.transform).GetComponent<FunctionLineController>();
					
					//Set position
					Vector2 newPos = funcLineCon.gameObject.transform.localPosition;
					newPos.y = codeYPos;
					funcLineCon.gameObject.transform.localPosition = newPos;

					//Update text
					funcLineCon.SetFunction(line.text, line.isBad);
					funcLineCon.robotUprisingController = this;
					//Tab function line
					funcLineCon.Tab(line.tabs);

					lineFunctions.Add(funcLineCon);
				}
				else
				{
					TextMeshProUGUI codeLine = Instantiate(codeLinePrefab, codeContainer.transform).GetComponent<TextMeshProUGUI>();

					//Set position
					Vector2 newPos = codeLine.gameObject.transform.localPosition;
					newPos.y = codeYPos;
					codeLine.gameObject.transform.localPosition = newPos;

					//Update text
					codeLine.text = line.text;
					
					//Tab code line
					Vector4 newMargin = codeLine.margin;
					newMargin.x = 100 * line.tabs;
					codeLine.margin = newMargin;
				}
				codeYPos -= codeYPosIncrease;
			}
		}
		lineFunctions[currentLine].inputField.ActivateInputField();
    }

	public void GoToFunction(FunctionLineController line)
	{
		int index = lineFunctions.IndexOf(line);
		currentLine = index;
		lineFunctions[currentLine].inputField.ActivateInputField();
	}
	public void GoToNextFunction()
	{
		if(currentLine < lineFunctions.Count-1)
		{
			currentLine++;
			lineFunctions[currentLine].inputField.ActivateInputField();
		}		
		else
		{
			DeselectCurrent();
		}
	}
	public void OnUp(InputAction.CallbackContext context)
	{
		if(context.started)
		{
			if(currentLine > 0)
			{
				currentLine--;
				lineFunctions[currentLine].inputField.ActivateInputField();
			}
		}
	}
	public void OnDown(InputAction.CallbackContext context)
	{
		if(context.started)
		{
			if(currentLine < lineFunctions.Count-1)
			{
				currentLine++;
				lineFunctions[currentLine].inputField.ActivateInputField();
			}
		}
	}

	public void OnEnter(InputAction.CallbackContext context)
	{
		if(context.started)
		{
			GoToNextFunction();
		}
	}

	public void OnCtrl(InputAction.CallbackContext context)
	{
		if(context.started)
		{
			ctrlPressed = true;
			if(ctrlPressed && sPressed)
			{
				Submit();
			}
		}
		else
		{
			sPressed = false;
		}
	}
	
	public void OnS(InputAction.CallbackContext context)
	{
		if(context.started)
		{
			sPressed = true;
			if(ctrlPressed && sPressed)
			{
				Submit();
			}
		}
		else
		{
			sPressed = false;
		}
	}

	public void DeselectCurrent()
	{
		lineFunctions[currentLine].inputField.OnDeselect (new BaseEventData(EventSystem.current));
	}

	public void Submit()
	{
		foreach(FunctionLineController lineFunc in lineFunctions)
		{
			lineFunc.ShowResult();

			//End or go to next page
		}
	}

	public void UseGoodCodeLine(string line)
	{
		usedGoodCodeLines.Add(line);
	}

	public void ReleaseGoodCodeLIne(string line)
	{
		usedGoodCodeLines.Remove(line);
	}


	// public void OnFastAndBadRepair()
	// {
	// 	moduleManager.MinigameCompleted(0.4f);
	// }

	// public void OnGoodRepair()
	// {
	// 	moduleManager.MinigameCompleted(1f);
	// }
}
