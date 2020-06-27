using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HoseLookAt : MonoBehaviour
{
	[SerializeField]
	private Transform lookAtTarget;
	private BoxCollider2D boxcol;
	
    // Start is called before the first frame update
    void Start()
    {
		boxcol = this.gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(lookAtTarget)
		{
			Vector3 diff = (lookAtTarget.position - this.transform.position);
			float dist = diff.magnitude;
			Vector3 dir = diff.normalized;
			this.transform.up = dir;
			boxcol.size = new Vector2(.5f, dist);
			boxcol.offset = new Vector2(0f, dist * 0.5f);
		}
	}
}
