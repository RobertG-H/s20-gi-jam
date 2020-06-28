using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainTrainPlayerController : MonoBehaviour, IAmAMainTrainPlayer
{

    [Dependency]
    IRegisterMainTrainInputs inputManager;

    [Dependency]
    IAmAMinigameManager miniGameManager;

    private Rigidbody2D rigidbody;
    private PhotonView photonView;

    public Sprite walkingSprite;
    public Sprite fixingSprite;
    private SpriteRenderer spriteRenderer;

    public float speed;
    public float jumpForce;
    public GameObject cam;
    public float distToGround;

    [HideInInspector]
    public ModuleTrigger currentModuleTrigger;

    bool isFacingRight = true;

    void Awake()
    {
    }

    void Start()
    {
        this.ResolveDependencies();

        rigidbody = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (photonView.IsMine)
        {
            inputManager.RegisterPlayer(this);
            miniGameManager.RegisterPlayer(this);
        }
        else
        {
            cam.SetActive(false);
        }
        spriteRenderer.sprite = walkingSprite;

    }

    void Update()
    {
        isGrounded();

    }

    void IAmAMainTrainPlayer.EnableCamera()
    {
        cam.SetActive(true);
        photonView.RPC("RPCLeavingMiniGame", RpcTarget.AllViaServer);
    }

    void IAmAMainTrainPlayer.HandleInput(MainTrain.Inputs currentInputs)
    {
        if (!photonView.IsMine) return;
        if(currentInputs.left)
        {
            rigidbody.velocity = new Vector2(speed*-1, rigidbody.velocity.y);
            isFacingRight = false;
        }
        else if (currentInputs.right)
        {
            rigidbody.velocity = new Vector2(speed * 1, rigidbody.velocity.y);
            isFacingRight = true;

        }
        else
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        }

        if(currentInputs.space && isGrounded())
        {
            rigidbody.AddForce(new Vector2(rigidbody.velocity.x, jumpForce));
        }

        if(currentInputs.up)
        {
            if (currentModuleTrigger != null)
            {
                miniGameManager.DisableControls();
                cam.SetActive(false);
                currentModuleTrigger.EnterTheOneMinigame();
                photonView.RPC("RPCStartingMiniGame", RpcTarget.AllViaServer);
            }
        }

        if (isFacingRight) transform.localScale = new Vector3(1, 1, 1);
        else transform.localScale = new Vector3(-1, 1, 1);

    }

    bool isGrounded()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Ground");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distToGround + 0.1f, layerMask);

        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, -Vector3.up * (distToGround + 0.1f), Color.yellow, 100f);
            return true;
        }
        Debug.DrawRay(transform.position, -Vector3.up * (distToGround + 0.1f), Color.red, 100f);

        return false;
    }

    [PunRPC]
    public void RPCStartingMiniGame()
    {
        spriteRenderer.sprite = fixingSprite;
        rigidbody.velocity = new Vector2(0, 0);
        rigidbody.isKinematic = true;
    }

    [PunRPC]
    public void RPCLeavingMiniGame()
    {
        spriteRenderer.sprite = walkingSprite;
        rigidbody.velocity = new Vector2(0, 0);
        rigidbody.isKinematic = false;
    }



}
