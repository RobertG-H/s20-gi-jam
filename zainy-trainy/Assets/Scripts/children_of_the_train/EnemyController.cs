using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float defeat_pause_duration = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-1 * speed * Vector3.forward * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(BecomeDefeated());
    }

    private IEnumerator BecomeDefeated()
    {
        speed = 0;
        yield return new WaitForSeconds(defeat_pause_duration);
        Destroy(gameObject);
    }
}
