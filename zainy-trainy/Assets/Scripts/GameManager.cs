using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
    bool runOnce = true;
    #region UNITY
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
        if(runOnce){
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

    #endregion
    private void StartGame()
    {
        Vector3 spawnPos = new Vector3(PhotonNetwork.LocalPlayer.GetPlayerNumber(),0.5f,0);
        PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
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
        StartGame();
    }

}
