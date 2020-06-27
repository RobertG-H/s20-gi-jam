using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManger : MonoBehaviour, IRecieveCarBreakAlert, IGetScoresOnRepairComplete, IServiceProvider
{

	float currentscore = 0f;

	[SerializeField]
	GameObject playerToDisable;

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
		playerToDisable.SetActive(true);
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
	}


	public void EnterTheOneMinigame()
	{
		moduleObject.GetComponent<IAmAMinigame>().OpenMinigame(1);
		playerToDisable.SetActive(false);
	}
}
