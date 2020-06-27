using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTrainPlayerModuleTrigger : MonoBehaviour
{
    public PhotonView view;
    public MainTrainPlayerController parent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!view.IsMine) return;
        if(collision.gameObject.tag == "module")
        {
            parent.currentModuleTrigger = collision.gameObject.GetComponent<ModuleTrigger>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!view.IsMine) return;

        if (collision.gameObject.tag == "module")
        {
            parent.currentModuleTrigger = null;
        }
    }
}
