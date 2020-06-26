using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoGameManager : MonoBehaviour, IRecieveCarBreakAlert, IGetScoresOnRepairComplete, IServiceProvider
{
	[SerializeField]
	private Text scoredisplay;
	float currentscore = 0f;

	[SerializeField]
	GameObject canvastodisable;

	[SerializeField]
	GameObject moduleObject;


	void IServiceProvider.RegisterServices()
	{
		this.RegisterService<IRecieveCarBreakAlert>();
		this.RegisterService<IGetScoresOnRepairComplete>();
	}

	void IGetScoresOnRepairComplete.RepaireCompleted(ICanBreakdown traincar, IAmAMinigame completedGame, float score, int playerid)
	{
		currentscore += score * 100f;
		print("Got here");
		canvastodisable.SetActive(true);
	}


	void IRecieveCarBreakAlert.TraincarIsBroken(GameObject Traincar, ICanBreakdown traincar, IAmAMinigame minigame)
	{
		Debug.Log("i dont care");
	}

	void IRecieveCarBreakAlert.TraincarIsDamaged(GameObject Traincar, ICanBreakdown traincar, IAmAMinigame minigame)
	{
		Debug.Log("i dont care");
	}


	void Update()
	{
		scoredisplay.text = "SCORE: " + currentscore;
	}
	

	public void EnterTheOneMinigame()
	{
		moduleObject.GetComponent<IAmAMinigame>().OpenMinigame(1);
		canvastodisable.SetActive(false);
	}
}
