using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoseInputManager : MonoBehaviour
{
	public Vector2 mousePos = Vector2.zero;
	public bool leftClick = false;




	public bool rightClick = false;


	bool rightdown;
	bool lastrightdown;
	public bool leftdown;
	bool lastleftdown;


	private void Update()
	{
		leftClick = leftdown && !lastleftdown;
		rightClick = rightdown && !lastrightdown;


		lastrightdown = rightdown;
		lastleftdown = leftdown;

	}

	public void MousePosition(InputAction.CallbackContext context)
	{
		// currentInputs.up = context.ReadValue<float>() == 1;
		mousePos = context.ReadValue<Vector2>();
	}

	public void MouseClick1(InputAction.CallbackContext context)
	{
		leftdown = context.ReadValueAsButton();

	}

	public void MouseClick2(InputAction.CallbackContext context)
	{
		rightdown = context.ReadValueAsButton();

	}

}
