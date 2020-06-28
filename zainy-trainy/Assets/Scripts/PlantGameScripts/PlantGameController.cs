using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGameController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;
    public GameObject bug;
    public List<Vector3> spawPositions;
    public List<Vector3 []> walkPositions;
    public float bugSpawnTimer;
    private float bugSpawnCounter;
    public List<GameObject> bugs;

    Canvas canvas;


    void Start()
    {
        Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
        //Instantiate(bug, new Vector3(2, 0, 0), Quaternion.identity);
        walkPositions = new List<Vector3[]>();
        walkPositions.Add(new[] { new Vector3(0f, 0f, 0f), new Vector3(1f, 1f, 0f) });

        int randomChoice = Random.Range(0, spawPositions.Count);
        GameObject currentBug = Instantiate(bug, spawPositions[randomChoice], Quaternion.identity);
        bugs.Add(currentBug);
        currentBug.GetComponent<BugScript>().maxLeft = walkPositions[0][0];
        currentBug.GetComponent<BugScript>().maxRight = walkPositions[0][1];

        bugSpawnCounter = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (bugSpawnCounter > bugSpawnTimer)
        {
            int randomChoice = Random.Range(0, spawPositions.Count);
            GameObject currentBug = Instantiate(bug, spawPositions[randomChoice], Quaternion.identity);
            bugs.Add(currentBug);
            currentBug.GetComponent<BugScript>().maxLeft = walkPositions[0][0];
            currentBug.GetComponent<BugScript>().maxRight = walkPositions[0][1];

            bugSpawnCounter = 0;
        }
        else
        {
            bugSpawnCounter += Time.deltaTime;
        }

        if (spawPositions.Count == 0)
        { 

        }

    }
}
