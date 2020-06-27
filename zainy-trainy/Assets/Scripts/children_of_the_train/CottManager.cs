using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CottManager : MonoBehaviour
{
    public int spawns_remaining = 10;
    public int remaining_enemies;

    // Start is called before the first frame update
    void Start()
    {
        remaining_enemies = spawns_remaining;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DecreaseSpawns()
    {
        spawns_remaining = spawns_remaining - 1;
    }

    public void DecreaseRemainingEnemies()
    {
        remaining_enemies = remaining_enemies - 1;
        if (remaining_enemies < 1)
            WinGame();
    }
    
    public bool SpawnsRemaining()
    {
        return spawns_remaining > 0;
    }

    public void WinGame()
    {
        Debug.Log("Win Game");
    }

    public void LoseGame()
    {
        spawns_remaining = 0;
        Debug.Log("Lose Game");
    }
}
