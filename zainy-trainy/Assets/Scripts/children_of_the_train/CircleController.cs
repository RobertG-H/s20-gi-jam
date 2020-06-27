using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleController : MonoBehaviour
{

    public float activation_duration = 0.2f;
    public Color active_color = new Color(200, 0, 0);
    public Color inactive_color = new Color(255, 255, 255);

    private SpriteRenderer sprite;
    public GameObject collider;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        collider = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        StartCoroutine(ActivationProcedure());
    }

    private IEnumerator ActivationProcedure()
    {
        sprite.color = active_color;
        collider.active = true;

        yield return new WaitForSeconds(activation_duration);

        sprite.color = inactive_color;
        collider.active = false;
    }

}
