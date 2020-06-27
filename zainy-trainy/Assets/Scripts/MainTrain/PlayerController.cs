using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IRecieveWASDInputs, IRecieveMouseInputs
{
    [Dependency]
    IRegisterInputs inputManager;

    private Rigidbody2D rigidbody;
    private PhotonView photonView;
    // Start is called before the first frame update

    void Awake()
    {
        this.ResolveDependencies();
    }
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
        inputManager.RegisterWASDInputObject(this);
        inputManager.RegisterMouseInputObject(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void IRecieveWASDInputs.OnWKeyPressed()
    {
        if (!photonView.IsMine)
        {
            return;
        }
    }

    void IRecieveWASDInputs.OnAKeyPressed()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        rigidbody.velocity = new Vector2(-1,0);
    }

    void IRecieveWASDInputs.OnSKeyPressed()
    {
        if (!photonView.IsMine)
        {
            return;
        }
    }

    void IRecieveWASDInputs.OnDKeyPressed()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        rigidbody.velocity = new Vector2(1, 0);
    }

    void IRecieveWASDInputs.OnSpaceKeyPressed()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        rigidbody.velocity = new Vector2(0, 0);
    }

    void IRecieveMouseInputs.OnMousePosition(Vector2 position)
    {
        Debug.Log(string.Format("Mouse Position: {0}", position));
    }

    void IRecieveMouseInputs.OnMouse1Pressed()
    {
    }

    void IRecieveMouseInputs.OnMouse2Pressed()
    {
    }
}
