using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafColliderScript : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    bool playerPassing=false;
    void Start()
    {
        player = GameObject.Find("PlantPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPlatformerController.instance.gameObject.transform.position.y-0.5 <= gameObject.transform.position.y)
        {
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            playerPassing = false;
        }

        else if(!playerPassing)
        {
            gameObject.GetComponent<PolygonCollider2D>().enabled = true;
        }
    }

    public void playerPassThrough()
    {
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        playerPassing = true;
    }
}
