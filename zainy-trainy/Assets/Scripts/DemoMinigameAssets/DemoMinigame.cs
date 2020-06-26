using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoMinigame : MonoBehaviour
{
	[SerializeField]
	private DemoModuleManager moduleManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	public void OnFastAndBadRepair()
	{
		moduleManager.MinigameCompleted(0.4f);
	}

	public void OnGoodRepair()
	{
		moduleManager.MinigameCompleted(1f);
	}
}
