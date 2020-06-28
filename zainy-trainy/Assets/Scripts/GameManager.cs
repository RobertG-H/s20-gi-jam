using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public string PhotonPlayerPrefabName;

    public static GameManager Instance {get; private set;} 



    bool runOnce = true;

    #region UNITY
    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Debug.Log("Warning: multiple " + this + " in scene!"); }
    }
    void Start()
    {
        Hashtable props = new Hashtable
        {
            {"PlayerLoadedLevel", true}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    void Update()
    {
        if (runOnce)
        {
            runOnce = false;
            StartGame();
        }

    }

    public override void OnEnable()
    {
        base.OnEnable();

        CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
    }
    public override void OnDisable()
    {
        base.OnDisable();

        CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
    }

    #endregion

    #region PUN CALLBACKS
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        // if there was no countdown yet, the master client (this one) waits until everyone loaded the level and sets a timer start
        int startTimestamp;
        bool startTimeIsSet = CountdownTimer.TryGetStartTime(out startTimestamp);


        if (changedProps.ContainsKey("PlayerLoadedLevel"))
        {
            if (CheckAllPlayerLoadedLevel())
            {
                if (!startTimeIsSet)
                {
                    Debug.Log("Starting timer");
                    CountdownTimer.SetStartTime();
                }
            }
            else
            {
                // not all players loaded yet. wait:
                Debug.Log("setting text waiting for players! ");
            }
        }
    
    }

    public override void OnDisconnected(DisconnectCause cause)
    {

        SceneManager.LoadScene("Lobby");
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    #endregion




    void StartGame()
    {
        if (PhotonNetwork.IsMasterClient) // Setup props to track if a minigame is being played
        {
            Hashtable roomProps = new Hashtable();
            foreach (int i in (int[])Enum.GetValues(typeof(MiniGames)))
            {
                roomProps.Add(i.ToString(), false);
                Debug.Log(i);
            }
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
            Debug.Log("Set room props");

        }

        Debug.Log("Starting Game");
        Vector3 spawnPos = new Vector3(PhotonNetwork.LocalPlayer.GetPlayerNumber(), 0.5f, 0);
        PhotonNetwork.Instantiate(PhotonPlayerPrefabName, spawnPos, Quaternion.identity);

    }

    public void EndGame(bool won, float dist)
    {
        if (won)
        {
            Debug.Log("You won!");
        }
        else
        {
            Debug.Log("You lost...");
        }
        PhotonNetwork.DestroyAll();
        PhotonNetwork.LeaveRoom();
    }

    private bool CheckAllPlayerLoadedLevel()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object playerLoadedLevel;

            if (p.CustomProperties.TryGetValue("PlayerLoadedLevel", out playerLoadedLevel))
            {
                if ((bool) playerLoadedLevel)
                {
                    continue;
                }
            }

            return false;
        }

        return true;
    }

    private void OnCountdownTimerIsExpired()
    {
        //StartGame();
    }


}
