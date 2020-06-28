using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MiniGameManger : MonoBehaviour, IRecieveCarBreakAlert, IGetScoresOnRepairComplete, IAmAMinigameManager, IServiceProvider
{

	float currentscore = 0f;

	PhotonView photonView;

	[SerializeField]
	GameObject controlsToDisable;

	IAmAMainTrainPlayer player;

	void IServiceProvider.RegisterServices()
	{
		this.RegisterService<IRecieveCarBreakAlert>();
		this.RegisterService<IGetScoresOnRepairComplete>();
		this.RegisterService<IAmAMinigameManager>();

	}

	void IGetScoresOnRepairComplete.RepaireCompleted(ICanBreakdown traincar, IAmAMinigame completedGame, float score, int playerid)
	{
		print("Repairs Complete!");
		currentscore += score * 100f;
		//photonView.RPC("RPC_FixTrainCar", RpcTarget.AllViaServer, completedGame, -score);
		// TODO UPDATE TO RPC
		completedGame.fixTrainCar(-score);

		controlsToDisable.SetActive(true);
		player.EnableCamera();
	}


	void IRecieveCarBreakAlert.TraincarIsBroken(GameObject Traincar, ICanBreakdown traincar, IAmAMinigame minigame)
	{
		print("TraincarIsBroken");

	}

	void IRecieveCarBreakAlert.TraincarIsDamaged(GameObject Traincar, ICanBreakdown traincar, IAmAMinigame minigame)
	{
		print("TraincarIsDamaged");
	}

	void IAmAMinigameManager.DisableControls()
	{
		controlsToDisable.SetActive(false);

	}

	void IAmAMinigameManager.EnableControls()
	{
		controlsToDisable.SetActive(true);
		player.EnableCamera();

	}

	void IAmAMinigameManager.RegisterPlayer(IAmAMainTrainPlayer p)
	{
		player = p;
	}

	void Awake()
	{
		photonView = GetComponent<PhotonView>();
	}

	[PunRPC]
	void RPC_FixTrainCar(IAmAMinigame game, float amountToFix)
	{
		game.fixTrainCar(amountToFix);
	}
}
