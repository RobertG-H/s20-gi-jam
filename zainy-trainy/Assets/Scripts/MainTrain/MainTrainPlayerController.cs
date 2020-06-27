using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainTrainPlayerController : MonoBehaviour, IAmAMainTrainPlayer
{

    [Dependency]
    IRegisterMainTrainInputs inputManager;

    private Rigidbody2D rigidbody;
    private PhotonView photonView;
    public float speed;

    void Awake()
    {
        this.ResolveDependencies();
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            inputManager.RegisterPlayer(this);
        }
    }

    void Update()
    {

    }

    void IAmAMainTrainPlayer.HandleInput(Inputs currentInputs)
    {
        if (!photonView.IsMine) return;
        if(currentInputs.left)
        {
            rigidbody.velocity = new Vector2(speed*-1, rigidbody.velocity.y);
        }
        else if (currentInputs.right)
        {
            rigidbody.velocity = new Vector2(speed * 1, rigidbody.velocity.y);
        }
        else
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        }
    }



}
