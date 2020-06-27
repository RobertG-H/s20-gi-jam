using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, IServiceProvider, IRegisterInputs
{
    List<IRecieveMouseInputs> registeredMouseInputObjects = new List<IRecieveMouseInputs>();
    List<IRecieveWASDInputs> registeredWASDInputObjects = new List<IRecieveWASDInputs>();

    void IServiceProvider.RegisterServices()
    {
        this.RegisterService<IRegisterInputs>();
    }

    void IRegisterInputs.RegisterMouseInputObject(IRecieveMouseInputs inputObject)
    {
        registeredMouseInputObjects.Add(inputObject);
    }

    void IRegisterInputs.RegisterWASDInputObject(IRecieveWASDInputs inputObject)
    {
        registeredWASDInputObjects.Add(inputObject);
    }

    void IRegisterInputs.UnRegisterMouseInputObject(IRecieveMouseInputs inputObject)
    {
        if(registeredMouseInputObjects.Contains(inputObject))
        {
            registeredMouseInputObjects.Remove(inputObject);
        }

    }

    void IRegisterInputs.UnRegisterWASDInputObject(IRecieveWASDInputs inputObject)
    {
        if(registeredWASDInputObjects.Contains(inputObject))
        {
            registeredWASDInputObjects.Remove(inputObject);
        }
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        foreach (IRecieveMouseInputs inputObject in registeredMouseInputObjects)
        {
            inputObject.OnMousePosition(value);
        }
    }

    public void OnMouse1(InputAction.CallbackContext context)
    {
        foreach (IRecieveMouseInputs inputObject in registeredMouseInputObjects)
        {
            inputObject.OnMouse1Pressed();
        }
    }

    public void OnMouse2(InputAction.CallbackContext context)
    {
        foreach (IRecieveMouseInputs inputObject in registeredMouseInputObjects)
        {
            inputObject.OnMouse2Pressed();
        }
    }

    public void OnW(InputAction.CallbackContext context)
    {
        foreach (IRecieveWASDInputs inputObject in registeredWASDInputObjects)
        {
            inputObject.OnWKeyPressed();
        }
    }

    public void OnA(InputAction.CallbackContext context)
    {
        foreach (IRecieveWASDInputs inputObject in registeredWASDInputObjects)
        {
            inputObject.OnAKeyPressed();
        }
    }

    public void OnS(InputAction.CallbackContext context)
    {
        foreach (IRecieveWASDInputs inputObject in registeredWASDInputObjects)
        {
            inputObject.OnSKeyPressed();
        }
    }

    public void OnD(InputAction.CallbackContext context)
    {
        foreach (IRecieveWASDInputs inputObject in registeredWASDInputObjects)
        {
            inputObject.OnDKeyPressed();
        }
    }

    public void OnSpace(InputAction.CallbackContext context)
    {
        foreach (IRecieveWASDInputs inputObject in registeredWASDInputObjects)
        {
            inputObject.OnSpaceKeyPressed();
        }
    }
}
