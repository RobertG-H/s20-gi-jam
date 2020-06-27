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

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    Vector2 velocity;
    bool grounded;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public PlantPlayerInputs currentInputs;

    Rigidbody2D rb2d;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        currentInputs.horizontal = 0;
        currentInputs.jumping = false;
        currentInputs.down = false;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector2 targetVelocity = ComputeVelocity();
        velocity.x = targetVelocity.x;
        //velocity += Physics2D.gravity * Time.deltaTime;
        Vector2 deltaPosition = velocity * Time.deltaTime;
        rb2d.position = rb2d.position + deltaPosition;
    }

    Vector2 ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = currentInputs.horizontal;
        print(currentInputs.horizontal);

        if (currentInputs.jumping)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        //else if (currentInputs.jumping)
        //{
        //    if (velocity.y > 0)
        //    {
        //        velocity.y = velocity.y * 0.5f;
        //    }
        //}

        //bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
        //if (flipSprite)
        //{
        //    spriteRenderer.flipX = !spriteRenderer.flipX;
        //}

        return move * maxSpeed;
    }
}