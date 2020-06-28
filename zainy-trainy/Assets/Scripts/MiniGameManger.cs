using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum MiniGames { WELDING, ROBOT, TOASTER, HOSE};

public class MiniGameManger : MonoBehaviour, IRecieveCarBreakAlert, IGetScoresOnRepairComplete, IAmAMinigameManager, IServiceProvider
{

	float currentscore = 0f;

	PhotonView photonView;

	[SerializeField]
	GameObject controlsToDisable;

	IAmAMainTrainPlayer player;

	void IServiceProvider.RegisterServices()
	{
		// This is now handled in awake because it is going to be instaniated by the PhotonGameManager
		//if (!photonView.IsMine) return;
		//this.RegisterService<IRecieveCarBreakAlert>();
		//this.RegisterService<IGetScoresOnRepairComplete>();
		//this.RegisterService<IAmAMinigameManager>();
	}

	void IGetScoresOnRepairComplete.RepaireCompleted(ICanBreakdown traincar, IAmAMinigame completedGame, float score, int playerid)
	{
		if (!photonView.IsMine) return;

		print("Repairs Complete!");
		currentscore += score * 100f;
		//photonView.RPC("RPC_FixTrainCar", RpcTarget.AllViaServer , completedGame, -score);
		controlsToDisable.SetActive(true);
		player.EnableCamera();
	}


	void IRecieveCarBreakAlert.TraincarIsBroken(GameObject Traincar, ICanBreakdown traincar, IAmAMinigame minigame)
	{
		if (!photonView.IsMine) return;

		print("TraincarIsBroken");

	}

	void IRecieveCarBreakAlert.TraincarIsDamaged(GameObject Traincar, ICanBreakdown traincar, IAmAMinigame minigame)
	{
		if (!photonView.IsMine) return;

		print("TraincarIsDamaged");
	}

	void IAmAMinigameManager.DisableControls()
	{
		if (!photonView.IsMine) return;

		controlsToDisable.SetActive(false);

	}

	void IAmAMinigameManager.EnableControls()
	{
		if (!photonView.IsMine) return;

		controlsToDisable.SetActive(true);
		player.EnableCamera();

	}

	void IAmAMinigameManager.RegisterPlayer(IAmAMainTrainPlayer p)
	{
		if (!photonView.IsMine) return;

		player = p;
	}

	void Awake()
	{
		photonView = GetComponent<PhotonView>();
		if (!photonView.IsMine) return;
		this.RegisterService<IRecieveCarBreakAlert>();
		this.RegisterService<IGetScoresOnRepairComplete>();
		this.RegisterService<IAmAMinigameManager>();
		controlsToDisable = GameObject.FindWithTag("inputmanager");
	}

	[PunRPC]
	void RPC_FixTrainCar(IAmAMinigame game, float amountToFix)
	{
		Debug.Log(photonView.IsMine);
		game.fixTrainCar(amountToFix);
	}
}
