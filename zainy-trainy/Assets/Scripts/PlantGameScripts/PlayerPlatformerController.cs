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

    public static PlayerPlatformerController instance;

    public float horSpeed;
    public float jumpTakeOffSpeed;
    Vector2 velocity;
    bool grounded;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public PlantPlayerInputs currentInputs;
    private bool isJumping = false;
    public float jumptime;
    private float jumptimer;
    bool facingRight = false;

    public bool isDead = false;

    int size=5;

    Rigidbody2D rb2d;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerPlatformerController.instance = this;
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
        if (col.tag == "Bug")
        {
            if (col.GetComponent<BugScript>().size<=size)
            {
                Destroy(col.gameObject);

                if (size <= 9)
                {
                    print("size");
                    print(size <= 9);
                    size += 1;
                    Vector3 scaler = transform.localScale;
                    scaler = (scaler / ((size - 1) / 5f)) * (size / 5f);
                    transform.localScale = scaler;
                }
                
            }
            else
            {
                //print("you cannot eat this");
            }
            
        }

        if (col.name == "DeadBox")
        {
            Debug.Log("you are dead");
            isDead = true;

        }

    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "LeafCollider")
        {
            if (currentInputs.down)
            {
                col.gameObject.GetComponent<LeafColliderScript>().playerPassThrough();
                print(col.gameObject.name);
            }
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