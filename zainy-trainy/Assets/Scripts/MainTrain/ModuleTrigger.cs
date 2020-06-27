using UnityEngine;
using System.Collections;

public class ModuleTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject moduleObject;

    public void EnterTheOneMinigame()
    {
        moduleObject.GetComponent<IAmAMinigame>().OpenMinigame(1);
    }
}
