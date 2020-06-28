using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleController : MonoBehaviour
{

    public float activation_duration = 0.2f;
    public Color active_color = new Color(200, 0, 0);
    public Color inactive_color = new Color(255, 255, 255);
    public char current_key;
    public GameObject collider;
    public TextMesh display_text;

    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        ChangeKey();
        collider.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeKey()
    {
        current_key = (char) Random.Range((int) 'a', (int) 'z');
        display_text.text = current_key.ToString();

    }

    public void CheckInput(char input_key)
    {
        if (input_key == current_key)
            StartCoroutine(ActivationProcedure());
    }

    private IEnumerator ActivationProcedure()
    {
        sprite.color = active_color;
        collider.active = true;
        ChangeKey();

        yield return new WaitForSeconds(activation_duration);

        sprite.color = inactive_color;
        collider.active = false;
    }

}
