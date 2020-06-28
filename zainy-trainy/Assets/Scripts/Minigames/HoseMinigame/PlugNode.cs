using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugNode : MonoBehaviour
{
	HoseHandler hosehandler;
    // Start is called before the first frame update
    void Start()
    {
		hosehandler = this.GetComponentInParent<HoseMinigameHandler>().hoseHandler;
    }

    // Update is called once per frame
    void Update()
    {
		if (hosehandler.isPlugged)
			this.GetComponent<Renderer>().material.color = Color.green;
		else
			this.GetComponent<Renderer>().material.color = Color.green*0.5f + Color.red;

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		print("redy? " + hosehandler.ready2Plug);
		if (hosehandler.ready2Plug)
		{
			hosehandler.isPlugged = true;
			hosehandler.handObject.position = this.transform.position;
		}
	}
}
