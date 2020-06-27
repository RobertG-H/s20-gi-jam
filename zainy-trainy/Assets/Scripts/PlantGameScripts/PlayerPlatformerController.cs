using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public struct PlantPlayerInputs
{
    public int horizontal;
    public bool jumping;
    public bool down;

}

public class PlayerPlatformerController : MonoBehaviour
{

    public float horSpeed = 7;
    public float jumpTakeOffSpeed = 10;
    Vector2 velocity;
    bool grounded;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public PlantPlayerInputs currentInputs;
    private bool isJumping = false;
    public float jumptime;
    private float jumptimer;
    bool facingRight = false;

    Rigidbody2D rb2d;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        currentInputs.horizontal = 0;
        currentInputs.jumping = false;
        currentInputs.down = false;
        jumptimer = 0;
    }

    void Update()
    {
        
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        print(col.name);
        if (col.name == "bug")
        {
            Destroy(col.gameObject);
        }
    }

    void FixedUpdate()
    {

        velocity = rb2d.velocity;

        velocity.x = currentInputs.horizontal*horSpeed;

        if(facingRight&& currentInputs.horizontal > 0)
        {
            Flip();
        }
        if (!facingRight && currentInputs.horizontal < 0)
        {
            Flip();
        }

        if (currentInputs.jumping && velocity.y == 0f)
        {
            velocity.y = jumpTakeOffSpeed;
            isJumping = true;
            jumptimer = jumptime;
            
        }
        else if (currentInputs.jumping && isJumping)
        {
            if (jumptimer > 0){
                velocity.y = jumpTakeOffSpeed;
                jumptimer -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }

        }
        else if (!currentInputs.jumping)
        {
            isJumping = false;
        }





        //bool flipSprite = (spriteRenderer.flipX ? (currentInputs.horizontal > 0.01f) : (currentInputs.horizontal < 0.01f));
        //if (flipSprite)
        //{
        //    spriteRenderer.flipX = !spriteRenderer.flipX;
        //}


        rb2d.velocity = velocity;


        //Vector2 targetVelocity = ComputeVelocity();
        //velocity.x = targetVelocity.x;
        ////velocity += 5*Physics2D.gravity * Time.deltaTime;
        //Vector2 deltaPosition = velocity * Time.deltaTime;
        //rb2d.position = rb2d.position + deltaPosition;
    }

    Vector2 ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = currentInputs.horizontal;

        print(currentInputs.jumping);
        if (currentInputs.jumping)
        {
            print("running");
            velocity.y = jumpTakeOffSpeed;
        }
        else
        {
            velocity.y=0;
        }

        //else if (currentInputs.jumping)
        //{
        //    if (velocity.y > 0)
        //    {
        //        velocity.y = velocity.y * 0.5f;
        //    }
        //}

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }


        return move * horSpeed;
    }
}