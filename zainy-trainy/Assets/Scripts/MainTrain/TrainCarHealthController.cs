using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCarHealthController : MonoBehaviour
{

    public SpriteRenderer statusSprite;
    public GameObject functionalIcon;
    public GameObject brokenIcon;

    void Start()
    {
        functionalIcon.SetActive(true);
        brokenIcon.SetActive(false);
    }

    public void UpdateStatusColor(Color color)
    {
        statusSprite.color = color;
    }

    public void ShowBroken()
    {
        functionalIcon.SetActive(false);
        brokenIcon.SetActive(true);
    }

    public void ShowFunctional()
    {
        functionalIcon.SetActive(true);
        brokenIcon.SetActive(false);
    }


}
