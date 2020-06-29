using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using UnityEngine.UI;

public enum MiniGames { WELDING, ROBOT, TOASTER, HOSE, CHILDREN, PLANT};

public class MiniGameManger : MonoBehaviour, IRecieveCarBreakAlert, IGetScoresOnRepairComplete, IAmAMinigameManager, IServiceProvider
{
	public float goalDistance;

	public int amtBrokenCarsToLose;

	public Text distanceText;

	public GameObject winText;

	public GameObject loseText;

	public AudioClip win;

	public AudioClip lose;

	public AudioSource aSource;

	private IEnumerator coroutine;

	[HideInInspector]
	public float currentDistance;

	PhotonView photonView;

	[SerializeField]
	GameObject controlsToDisable;

	IAmAMainTrainPlayer player;

	Dictionary<MiniGames, IAmAMinigame> allMiniGames = new Dictionary<MiniGames, IAmAMinigame>();

	Dictionary<MiniGames, bool> isTrainCarBroken = new Dictionary<MiniGames, bool>();

	bool gameIsOver = false;

	void IServiceProvider.RegisterServices()
	{
		photonView = GetComponent<PhotonView>();
		// This is now handled in awake because it is going to be instaniated by the PhotonGameManager
		this.RegisterService<IRecieveCarBreakAlert>();
		this.RegisterService<IGetScoresOnRepairComplete>();
		this.RegisterService<IAmAMinigameManager>();
	}
	void OnDestroy()
	{
		this.UnRegister<IRecieveCarBreakAlert>();
		this.UnRegister<IGetScoresOnRepairComplete>();
		this.UnRegister<IAmAMinigameManager>();
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
		winText.SetActive(false);

		loseText.SetActive(false);
		aSource = GetComponent<AudioSource>();

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
			if (gameIsOver) return;
			winText.SetActive(true);
			aSource.clip = win;
			aSource.Play();
			gameIsOver = true;

			coroutine = EndGame(5.0f, true);
			StartCoroutine(coroutine);

		}
		else
		{
			if (gameIsOver) return;

			currentDistance -=  8f * Time.deltaTime;
			distanceText.text = string.Format("Distance: {0}km", ((int)currentDistance).ToString());
		}
		if (GetTotalBrokenCars() >= amtBrokenCarsToLose)
		{
			if (gameIsOver) return;
			loseText.SetActive(true);
			aSource.clip = lose;
			aSource.Play();
			gameIsOver = true;

			coroutine = EndGame(5.0f, false);
			StartCoroutine(coroutine);

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

	private IEnumerator EndGame(float waitTime, bool didWin)
	{
		while (true)
		{
			yield return new WaitForSeconds(waitTime);
			GameManager.Instance.EndGame(didWin, currentDistance);
		}
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
