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
	public Camera gameCamera;
	public GameObject functionLinePrefab;
	public GameObject codeLinePrefab;
	public GameObject goodFunctionPrefab;
	public RectTransform codeContainer;
	public RectTransform goodFunctionsContainer;
	public int maxLines = 12;
	public RobotUprisingAudioController audioController;

	[SerializeField]
	private DemoModuleManager moduleManager;
	private List<CodeBlock> codeBlocks;
	private List<TextMeshProUGUI> goodFunctionUIElements;
	private List<TextMeshProUGUI> codeLineUIElements;
	[HideInInspector]
	public List<string> goodFunctions;
	[HideInInspector]
	public List<string> usedGoodFunctions;
	private List<FunctionLineController> lineFunctions;
	
	#region Formatting
	private int currentLine = 0;
	public float initialCodeYPos;
	public float codeYPosIncrease;
	private float codeYPos;

	public float initialGoodFunctionYPos;
	public float goodFunctionYPosIncrease;
	private float goodFunctionYPos;
	#endregion


	private bool ctrlPressed, sPressed;
	private float score;
	private bool submitted;
	
	
    // Start is called before the first frame update
    void Start()
    {
		lineFunctions = new List<FunctionLineController>();
		goodFunctionUIElements = new List<TextMeshProUGUI>();
		codeLineUIElements = new List<TextMeshProUGUI>();
		usedGoodFunctions = new List<string>();
		codeYPos = initialCodeYPos;
		submitted = false;

		int numFunctions = GenerateCodeLines();
		GenerateGoodFunctions(numFunctions);
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

					codeLineUIElements.Add(codeLine);
				}
				codeYPos -= codeYPosIncrease;
			}
		}

		goodFunctionYPos = initialGoodFunctionYPos;
		foreach(string goodFunctionString in goodFunctions)
		{

			TextMeshProUGUI goodFunctionInstance = Instantiate(goodFunctionPrefab, goodFunctionsContainer.transform).GetComponent<TextMeshProUGUI>();
			//Set position
			Vector2 newPos = goodFunctionInstance.gameObject.transform.localPosition;
			newPos.y = goodFunctionYPos;
			goodFunctionInstance.gameObject.transform.localPosition = newPos;

			//Update text
			goodFunctionInstance.text = goodFunctionString;

			goodFunctionUIElements.Add(goodFunctionInstance);
			goodFunctionYPos -= goodFunctionYPosIncrease;
		}
		StartTheGame();
    }

	public void StartTheGame()
	{
		lineFunctions[0].inputField.ActivateInputField();
	}

	public void GenerateGoodFunctions(int numToGenerate)
	{
		goodFunctions = new List<string>();
		List<int> usedIndices = new List<int>();
		object[] allGoodFunctions = Resources.LoadAll("ScriptableObjects/RobotUprising/GoodFunctions");
		System.Random random = new System.Random();
		for(int i = 0; i < numToGenerate; i++)
		{
			int randomIndex;
			do
			{
				randomIndex = random.Next(allGoodFunctions.Length);
			}
			while(usedIndices.Contains(randomIndex) && usedIndices.Count != allGoodFunctions.Length);
			// int randomIndex = random.Next(allGoodFunctions.Length);
			usedIndices.Add(randomIndex);
			goodFunctions.Add(((GoodFunction)allGoodFunctions[randomIndex]).function);
		}
	}

	public int GenerateCodeLines()
	{
		codeBlocks = new List<CodeBlock>();
		List<int> usedIndices = new List<int>();
		int numFunctions = 0;
		object[] allCodeBlocks = Resources.LoadAll("ScriptableObjects/RobotUprising/CodeBlocks");
		int currentNumberOfLines = 0;
		System.Random random = new System.Random();
		while(currentNumberOfLines < maxLines && usedIndices.Count != allCodeBlocks.Length)
		{
			int randomIndex;
			do
			{
				randomIndex = random.Next(allCodeBlocks.Length);
			}
			while(usedIndices.Contains(randomIndex) && usedIndices.Count != allCodeBlocks.Length);
			// int randomIndex = random.Next(allCodeBlocks.Length);
			usedIndices.Add(randomIndex);
			CodeBlock newCodeBlock = (CodeBlock)allCodeBlocks[randomIndex];
			if(currentNumberOfLines + newCodeBlock.lines.Count <= maxLines)
			{
				codeBlocks.Add(newCodeBlock);
				currentNumberOfLines += newCodeBlock.lines.Count;
				foreach(CodeBlock.line line in newCodeBlock.lines)
				{
					if(line.isFunction)
					{
						numFunctions++;
					}
				}
			}
		}
		return numFunctions;
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
		if(context.ReadValue<float>() == 1)
		{
			ctrlPressed = true;
		}
		else
		{
			ctrlPressed = false;
		}
		if(ctrlPressed && sPressed)
		{
			Submit();
		}
	}
	
	public void OnS(InputAction.CallbackContext context)
	{
		if(context.ReadValue<float>() == 1)
		{
			sPressed = true;
		}
		else
		{
			sPressed = false;
		}
		if(ctrlPressed && sPressed)
		{
			Submit();	
		}
	}

	public void DeselectCurrent()
	{
		lineFunctions[currentLine].inputField.OnDeselect (new BaseEventData(EventSystem.current));
	}

	public void Submit()
	{
		if(!submitted)
		{
			submitted = true;
			float numberBad = 0;
			float numberGood = 0;
			foreach(FunctionLineController lineFunc in lineFunctions)
			{
				lineFunc.ShowResult();
				if(lineFunc.isBad)
				{
					numberBad++;
				}
				else
				{
					numberGood++;
				}
			}
			score = numberGood/(numberGood + numberBad);
			Debug.Log(score);
			StartCoroutine("EndGame");
		}
	}

	private IEnumerator EndGame()
	{
		yield return new WaitForSeconds(1.0f);
		Cleanup();
		moduleManager.MinigameCompleted(score);
	}

	public void UseGoodCodeLine(string line)
	{
		usedGoodFunctions.Add(line);
		int goodFunctionIndex = goodFunctions.IndexOf(line);
		goodFunctionUIElements[goodFunctionIndex].fontStyle = FontStyles.Strikethrough;
	}

	public void ReleaseGoodCodeLIne(string line)
	{
		usedGoodFunctions.Remove(line);
		int goodFunctionIndex = goodFunctions.IndexOf(line);
		goodFunctionUIElements[goodFunctionIndex].fontStyle = FontStyles.Normal;
	}

	public void Cleanup()
	{
		foreach(TextMeshProUGUI codeLine in codeLineUIElements)
		{
			Destroy(codeLine.gameObject);
		}
		foreach(TextMeshProUGUI goodFunction in goodFunctionUIElements)
		{
			Destroy(goodFunction.gameObject);
		}
		foreach(FunctionLineController lineFunc in lineFunctions)
		{
			Destroy(lineFunc.gameObject);
		}
		Start();
	}
}
