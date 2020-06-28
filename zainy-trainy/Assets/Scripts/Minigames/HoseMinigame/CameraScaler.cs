using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
		Camera cam = this.gameObject.GetComponent<Camera>();
    }
}
