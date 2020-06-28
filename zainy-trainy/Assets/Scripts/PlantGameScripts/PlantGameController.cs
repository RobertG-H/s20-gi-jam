using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGameController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject playerPrefab;
    public int maxBugs;
    GameObject plantPlayer;
    public GameObject bug;
    public List<Vector3> spawPositions;
    public List<Vector3 []> walkPositions;
    public float bugSpawnTimer;
    private float bugSpawnCounter;
    public List<GameObject> bugs;
    private int bugSpawns;
    public int bugsEaten=0;
    

    Canvas canvas;


    void Start()
    {
        bugSpawns = 0;
        plantPlayer = Instantiate(playerPrefab, new Vector3(0.32f, -2.0f, 0), Quaternion.identity);
        //Instantiate(bug, new Vector3(2, 0, 0), Quaternion.identity);
        walkPositions = new List<Vector3[]>();
        walkPositions.Add(new[] { new Vector3(0f, 0f, 0f), new Vector3(1f, -1f, 0f) });
        walkPositions.Add(new[] { new Vector3(0f, 0f, 0f), new Vector3(0.5f, -0f, 0f) });
        walkPositions.Add(new[] { new Vector3(0f, 0f, 0f), new Vector3(-0.5f, -0.1f, 0f) });
        walkPositions.Add(new[] { new Vector3(0f, 0f, 0f), new Vector3(0.2f, 0f, 0f) });
        walkPositions.Add(new[] { new Vector3(0f, 0f, 0f), new Vector3(0.2f, -0.2f, 0f) });
        walkPositions.Add(new[] { new Vector3(0f, 0f, 0f), new Vector3(1f, -1f, 0f) });
        walkPositions.Add(new[] { new Vector3(0f, 0f, 0f), new Vector3(-1f, -1f, 0f) });
        walkPositions.Add(new[] { new Vector3(0f, 0f, 0f), new Vector3(-1f, -1f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(0.7f, -0.5f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(-0.7f, -0.5f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(-0.7f, -0.5f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(-0.5f, -0.1f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(-0.5f, -0.1f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(-0.8f, -0.2f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(-1f, -0.4f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(1f, 0.3f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(1f, 0.2f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(1f, 0.2f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(1f, -0.3f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(1f, 0.5f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(0.4f, -0.7f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(0.9f, -0.2f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(0.5f, -0.6f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(0.5f, 0.1f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(0.7f, 0.1f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(0.7f, 0.2f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(0.3f, -0.8f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(0.7f, 0.3f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(0.7f, 0.4f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(0.4f, 0.1f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(0.6f, -0.2f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(0.4f, 0.1f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(0.4f, -0.3f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(0.3f, 0.1f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(0.7f, 0.5f, 0f) });
        walkPositions.Add(new[] { new Vector3(-0f, -0f, 0f), new Vector3(1.1f, 1f, 0f) });







        bugSpawnCounter = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if ((bugSpawnCounter > bugSpawnTimer )&& bugSpawns<=maxBugs)
        {
            int randomChoice = Random.Range(0, spawPositions.Count);
            GameObject currentBug = Instantiate(bug, spawPositions[randomChoice], Quaternion.identity);
            bugs.Add(currentBug);
            currentBug.GetComponent<BugScript>().maxLeft = walkPositions[randomChoice][0];
            currentBug.GetComponent<BugScript>().maxRight = walkPositions[randomChoice][1];

            if (bugSpawns < 10)
            {
                
                currentBug.GetComponent<BugScript>().changeSize(Random.Range(1, (int)(bugSpawns)));
                bugSpawns++;
            }
            else
            {
                currentBug.GetComponent<BugScript>().changeSize(Random.Range(1, 10));
                bugSpawns++;
            }


            bugSpawnCounter = 0;
        }
        else
        {
            bugSpawnCounter += Time.deltaTime;
        }

        if (bugs.Count==0 && bugSpawns>=maxBugs)
        {
            Debug.Log("you have eaten all the bugs");
            resetPlantGame();
        }

        if (plantPlayer.GetComponent<PlayerPlatformerController>().isDead)
        {
            resetPlantGame();
        }

    }

    private void resetPlantGame()
    {
        Destroy(plantPlayer.gameObject);
        foreach (GameObject incBug in bugs)
        {
            Destroy(incBug);
        }
        bugs.Clear();
        

        bugSpawns = 0;
        plantPlayer = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        //Instantiate(bug, new Vector3(2, 0, 0), Quaternion.identity);
        bugSpawnCounter = 0;

        
        bugsEaten = 0;

    }
}
