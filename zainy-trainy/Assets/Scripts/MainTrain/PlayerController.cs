using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = new Vector2(1,0);
    }

    public void OnHorizontal(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        rigidbody.velocity = new Vector2(value,0);
    }
}
