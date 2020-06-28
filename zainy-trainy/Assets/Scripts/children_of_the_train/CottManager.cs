using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CottManager : MonoBehaviour
{
    public int spawns_remaining = 10;
    public int remaining_enemies;
    public GameObject input;
    public GameObject gloat;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Awake()
    {
        remaining_enemies = spawns_remaining;
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
        if (input.active) // First game over
        {
            spawns_remaining = 0;
            gloat.active = true;
            input.active = false;
            Debug.Log("Lose Game");
        }
    }
}
