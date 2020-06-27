using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    // Start is called before the first frame update
    private IAmAMainTrainPlayer player;

    public PlantPlayerInputs currentInputs;


    void Awake()
    {
        currentInputs = GetComponent<PlayerPlatformerController>().currentInputs;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnWKey(InputAction.CallbackContext context)
    {
        GetComponent<PlayerPlatformerController>().currentInputs.jumping = context.ReadValue<float>() == 1;

    }

    public void OnAKey(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1)
        {
            GetComponent<PlayerPlatformerController>().currentInputs.horizontal = -1;
        }
        if (context.ReadValue<float>() == 0)
        {
            GetComponent<PlayerPlatformerController>().currentInputs.horizontal = 0;
        }
    }

    public void OnSKey(InputAction.CallbackContext context)
    {
        GetComponent<PlayerPlatformerController>().currentInputs.down = context.ReadValue<float>() == 1;

    }

    public void OnDKey(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1)
        {
            GetComponent<PlayerPlatformerController>().currentInputs.horizontal = 1;
        }
        if (context.ReadValue<float>() == 0)
        {
            GetComponent<PlayerPlatformerController>().currentInputs.horizontal = 0;
        }
    }
}
