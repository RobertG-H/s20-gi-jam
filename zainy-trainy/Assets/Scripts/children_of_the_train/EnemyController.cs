using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;

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
        speed = speed * -1;
    }
}
