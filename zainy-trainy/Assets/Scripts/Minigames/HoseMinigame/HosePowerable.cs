using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoseNode : MonoBehaviour
{
	public void Depower()
	{
		this.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
	}

	public void Power()
	{
		this.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
	}
}
