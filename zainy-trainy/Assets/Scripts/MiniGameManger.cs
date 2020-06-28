using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public enum MiniGames { WELDING, ROBOT, TOASTER, HOSE};

public class MiniGameManger : MonoBehaviour, IRecieveCarBreakAlert, IGetScoresOnRepairComplete, IAmAMinigameManager, IServiceProvider
{
	public float goalDistance;

	public int amtBrokenCarsToLose;

	[HideInInspector]
	public float currentDistance;

	PhotonView photonView;

	[SerializeField]
	GameObject controlsToDisable;

	IAmAMainTrainPlayer player;

	Dictionary<MiniGames, IAmAMinigame> allMiniGames = new Dictionary<MiniGames, IAmAMinigame>();

	Dictionary<MiniGames, bool> isTrainCarBroken = new Dictionary<MiniGames, bool>();


	void IServiceProvider.RegisterServices()
	{
		photonView = GetComponent<PhotonView>();
		// This is now handled in awake because it is going to be instaniated by the PhotonGameManager
		this.RegisterService<IRecieveCarBreakAlert>();
		this.RegisterService<IGetScoresOnRepairComplete>();
		this.RegisterService<IAmAMinigameManager>();
	}

	void IGetScoresOnRepairComplete.RepaireCompleted(ICanBreakdown traincar, IAmAMinigame completedGame, float score, int playerid)
	{

		print("Repairs Complete!");
		controlsToDisable.SetActive(true);
		player.EnableCamera();
		// Send request to GameManager to fix all clients traincar
		FixTrainCar((int)completedGame.GetGameEnum(), score);
	}


	void IRecieveCarBreakAlert.TraincarIsBroken(GameObject Traincar, ICanBreakdown traincar, IAmAMinigame minigame)
	{
		isTrainCarBroken[minigame.GetGameEnum()] = true;
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
		currentDistance = goalDistance;

	}

	void Start()
	{
		// Find all minigames
		var search = FindObjectsOfType<MonoBehaviour>().OfType<IAmAMinigame>();
		foreach (IAmAMinigame game in search)
		{
			allMiniGames.Add(game.GetGameEnum(), game);
			isTrainCarBroken.Add(game.GetGameEnum(), false);
			Debug.Log(string.Format("Adding ref to minigame: {0} ", game.GetGameEnum()));
		}
	}

	void Update()
	{
		if (currentDistance <= 0)
		{
			//EndGame(true);
		}
		if (GetTotalBrokenCars() >= amtBrokenCarsToLose)
		{
			GameManager.Instance.EndGame(false, currentDistance);
		}
	}

	int GetTotalBrokenCars()
	{
		int count = 0;
		foreach (KeyValuePair<MiniGames, bool> entry in isTrainCarBroken)
		{
			if (entry.Value) count++;
		}
		Debug.Log(string.Format("Total broken cars: {0}", count));
		return count;
	}

	#region PUNRPC

	// Sync fixing of traincars

	public void FixTrainCar(int miniGameNum, float amountToFix)
	{
		photonView.RPC("RPC_FixTrainCar", RpcTarget.AllViaServer, miniGameNum, amountToFix);
	}

	[PunRPC]
	void RPC_FixTrainCar(int miniGameNum, float amountToFix)
	{
		if (amountToFix > 0)
		{
			isTrainCarBroken[(MiniGames)miniGameNum] = false;
		}
		IAmAMinigame gameTofix = allMiniGames[(MiniGames)miniGameNum];
		gameTofix.fixTrainCar(amountToFix);
		Debug.Log(string.Format("Is local: {0}, and fixing mini game: {1}", photonView.IsMine, miniGameNum));
	}


	#endregion

}
