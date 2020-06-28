using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoseNode : MonoBehaviour, IHosePowerable
{
	float powerlevel = 0f;
	void IHosePowerable.Depower()
	{
		this.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
	}

	bool IHosePowerable.NeedsPowerForComplete()
	{
		return true;
	}

	void IHosePowerable.Power()
	{
		powerlevel = 1.1f;
		this.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
	}

	bool IHosePowerable.IsPowered()
	{
		return powerlevel > 1f;
	}



	void Update()
	{
		this.gameObject.GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.black, Color.blue, powerlevel);
		powerlevel -= Time.deltaTime;
	}
}
