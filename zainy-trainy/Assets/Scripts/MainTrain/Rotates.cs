﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotates : MonoBehaviour
{
    float rotation = 0;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        rotation += Time.deltaTime * speed;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        if(rotation > 1800)
        {
            rotation = 0;
        }
    }
}
