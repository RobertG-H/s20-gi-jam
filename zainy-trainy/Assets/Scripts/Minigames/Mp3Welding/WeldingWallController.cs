using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeldingWallController : MonoBehaviour
{
    [SerializeField]
    private MP3ModuleManager moduleManager;

    public GameObject linePrefab;
    private GameObject currentLine;

    private LineRenderer lineRenderer;
    private List<Vector2> mousePositions = new List<Vector2>();


    enum states { 
        PLAYING,
        FAILED,
        WON
    }


    private Collider2D[] collisions;

    private Vector2 mousePosition;
    private int wallLayerMask = 0;
    private int startLayerMask = 0;
    private int endLayerMask = 0;

    bool isPressingDown = false;

    bool doesLineExist = false;

    states gameState = states.FAILED;


    // Start is called before the first frame update
    void Start()
    {
        mousePosition = Vector2.zero;
        wallLayerMask = 1 << LayerMask.NameToLayer("MP3Wall");
        startLayerMask = 1 << LayerMask.NameToLayer("MP3StartPoint");
        endLayerMask = 1 << LayerMask.NameToLayer("MP3EndPoint");
    }

    private void OnEnable()
    {
        mousePosition = Vector2.zero;
        mousePositions = new List<Vector2>();
        gameState = states.FAILED;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == states.WON) return;
        if (CheckMouseCollide(wallLayerMask))
        {
            Debug.Log("Hit Wall... Reseting");
            if (gameState != states.FAILED)
            {
                gameState = states.FAILED;
                RemoveLine();
            }
        }
        if (CheckMouseCollide(startLayerMask))
        {
            Debug.Log("At start pos!");
            if(gameState != states.PLAYING) gameState = states.PLAYING;
        }
        if (CheckMouseCollide(endLayerMask) && gameState == states.PLAYING)
        {
            Debug.Log("COMPLETE MP3 WELDING");
            gameState = states.WON;
            RemoveLine();
            moduleManager.MinigameCompleted(1f);
        }
    }

    private void DrawLine()
    {
        Vector2 globalPos = Camera.main.ScreenToWorldPoint(mousePosition);

        mousePositions.Add(globalPos);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, globalPos);

    }

    private void CreateLine(Vector2 globalPos)
    {
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        mousePositions.Clear();

        mousePositions.Add(globalPos);
        mousePositions.Add(globalPos);

        lineRenderer.SetPosition(0, mousePositions[0]);
        lineRenderer.SetPosition(1, mousePositions[1]);
    }

    private void RemoveLine()
    {
        if (doesLineExist)
        {
            doesLineExist = false;
            Destroy(currentLine);
        }
    }

    private bool CheckMouseCollide(int layerMask)
    {
        Vector3 mousePosV3 = new Vector3(mousePosition.x, mousePosition.y,10f);
        Vector3 globalPos = Camera.main.ScreenToWorldPoint(mousePosV3);
        collisions = Physics2D.OverlapPointAll(globalPos, layerMask);
        return collisions.Length > 0;
    }


    public void OnMousePosition(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
        if (isPressingDown && gameState == states.PLAYING && doesLineExist)
        {
            DrawLine();
        }
        
    }

    public void OnMouse1(InputAction.CallbackContext context)
    {
        isPressingDown = context.ReadValueAsButton();
        if (isPressingDown && gameState == states.PLAYING)
        {
            if (!doesLineExist)
            {
                doesLineExist = true;
                CreateLine(Camera.main.ScreenToWorldPoint(mousePosition));
            }
        }
        else
        {
            RemoveLine();
        }
    }

}
