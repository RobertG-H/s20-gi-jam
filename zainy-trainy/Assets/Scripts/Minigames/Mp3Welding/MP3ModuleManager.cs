using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class MP3ModuleManager : MonoBehaviour, IAmAMinigame
{
	[Dependency]//used by the ioc container to know what to populate with references.
	IRecieveCarBreakAlert carBreakdownAlertReciever;
	[Dependency]
	IGetScoresOnRepairComplete scoreReciever;

	[SerializeField]
	private GameObject minigameToEnable;
	[SerializeField]
	private GameObject traincarObject;
	private ICanBreakdown traincarinterface;

	public MiniGames currentMiniGame;

	bool isBeingPlayed = false;
	int playerPlaying = -1;

	void Awake()
	{
		this.ResolveDependencies();//needed for the ioc to work. This populates any [Dependency] tagged variables with proper references. IF there are proper references in the scene.
		traincarinterface = traincarObject.GetComponent<ICanBreakdown>();
	}

	void Update()
	{
		if (traincarinterface.GetCurrentRepairStatus() == 0)
		{
			carBreakdownAlertReciever.TraincarIsBroken(traincarObject, traincarinterface, (IAmAMinigame)this);//notifies the game manager when a car is broken. Can also use Traincar is damaged to notify at a certain threshold.
		}
	}


	public void MinigameCompleted(float score)
	{
		//traincarinterface.AddDamage(-score);
		isBeingPlayed = false;
		minigameToEnable.SetActive(false);
		scoreReciever.RepaireCompleted(traincarinterface, this, score, playerPlaying);
	}

	void IAmAMinigame.fixTrainCar(float amountToFix)
	{
		traincarinterface.AddDamage(amountToFix);
	}

	bool IAmAMinigame.GetIsMinigameCurrentlyRunning()
	{
		return isBeingPlayed;
	}

	void IAmAMinigame.OpenMinigame(int playerid)
	{
		playerPlaying = playerid;
		isBeingPlayed = true;
		minigameToEnable.SetActive(true);
	}

	int IAmAMinigame.GetLastPlayerWhoPlayed()
	{
		return playerPlaying;
	}

	MiniGames IAmAMinigame.GetGameEnum()
	{
		return currentMiniGame;
	}
}
