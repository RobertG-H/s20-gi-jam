using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CotTCreator : MonoBehaviour
{
    public GameObject cott_prefab;
    public DemoModuleManager mod_manager;

    private GameObject game;

    public Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        game = Instantiate(cott_prefab, spawnPoint).gameObject;
        game.GetComponent<CottManager>().mod_manager = mod_manager;
    }

    void OnDisable()
    {
        Destroy(game);
    }
}
