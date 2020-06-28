using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugScript : MonoBehaviour
{
    // Start is called before the first frame update

    PlantGameController controller;
    Vector3 startPos;
    public Vector3 maxRight;
    public Vector3 maxLeft;
    public int horSpeed;
    bool movingRight = true;

    public int size;

    Rigidbody2D rb2d;


    void Start()
    {
        controller = GameObject.Find("PlantGameController").GetComponent("PlantGameController") as PlantGameController;
        startPos = gameObject.transform.position;
        rb2d = GetComponent<Rigidbody2D>();
        
    }

    public void changeSize(int size)
    {
        this.size = size;
        Vector3 scaler = transform.localScale;
        scaler *= size / 5f;
        transform.localScale = scaler;
        Debug.Log(this.size);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void changeDirection()
    {
        movingRight = !movingRight;
        rb2d.velocity = new Vector2(0f, 0f);
        Flip();
    }

    void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }


    private void FixedUpdate()
    {
        Vector2 velocity = rb2d.velocity;

        if (movingRight)
        {
            Vector2 directionOfMovement = maxRight + startPos - gameObject.transform.position;


            if (directionOfMovement.magnitude <= 0.1)
            {
                changeDirection();
            }
            else
            {
                velocity = horSpeed * directionOfMovement.normalized;
                rb2d.velocity = velocity;
            }
            
        }
        if (!movingRight)
        {
            Vector2 directionOfMovement = (startPos- maxLeft) - gameObject.transform.position;

            if (directionOfMovement.magnitude <= 0.1)
            {
                changeDirection();
            }
            else
            {
                velocity = horSpeed * directionOfMovement.normalized;
                rb2d.velocity = velocity;
            }

        }


        
    }

    private void OnDestroy()
    {
        controller.bugs.Remove(gameObject);
    }
}
