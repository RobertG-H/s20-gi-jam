using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Hashtable = ExitGames.Client.Photon.Hashtable;


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

    private int currentGame;

    private int lastMiniGamePlayed;

    bool isFacingRight = true;

    public AudioClip enter;

    public AudioClip unable;

    public AudioClip jump;

    private AudioSource aSource;

    void Start()
    {
        this.ResolveDependencies();

        rigidbody = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
        aSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (photonView.IsMine)
        {
            Debug.Log("Attemtping to register Player");

            inputManager.RegisterPlayer(this);
            miniGameManager.RegisterPlayer(this);
        }
        else
        {
            cam.SetActive(false);
        }
        spriteRenderer.sprite = walkingSprite;
        currentGame = -1;
        lastMiniGamePlayed = -1;
    }

    void Update()
    {
        isGrounded();

    }

    void IAmAMainTrainPlayer.EnableCamera()
    {
        cam.SetActive(true);

        // Inform room that player is no longer playing the minigame
        Hashtable currentProps = PhotonNetwork.CurrentRoom.CustomProperties;
        currentProps[currentGame.ToString()] = false;
        PhotonNetwork.CurrentRoom.SetCustomProperties(currentProps);

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
            PlaySound(jump);
        }

        if(currentInputs.up)
        {
            if (currentModuleTrigger != null)
            {
                // Check if another player is playing the mini game
                currentGame = (int)currentModuleTrigger.GetGameEnum();
                if ((bool)PhotonNetwork.CurrentRoom.CustomProperties[currentGame.ToString()] || lastMiniGamePlayed==currentGame)
                {
                    PlaySound(unable);
                    return;
                }

                lastMiniGamePlayed = currentGame;
                // Inform room that player is playing the current Minigame
                Hashtable currentProps = PhotonNetwork.CurrentRoom.CustomProperties;
                currentProps[currentGame.ToString()] = true;
                PhotonNetwork.CurrentRoom.SetCustomProperties(currentProps);

                photonView.RPC("RPCStartingMiniGame", RpcTarget.AllViaServer);
                PlaySound(enter);
                miniGameManager.DisableControls();
                cam.SetActive(false);
                currentModuleTrigger.EnterTheOneMinigame();
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

    private void PlaySound(AudioClip clip)
    {
        aSource.clip = clip;
        aSource.Play();
    }

}
