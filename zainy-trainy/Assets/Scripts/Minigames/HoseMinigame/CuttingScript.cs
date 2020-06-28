using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingScript : MonoBehaviour, IHosePowerable
{
	float powerlevel = 0f;
	void IHosePowerable.Depower()
	{
		powerlevel = 1.5f;
	}

	bool IHosePowerable.NeedsPowerForComplete()
	{
		return false;
	}


	void IHosePowerable.Power()
	{
		powerlevel = 1.5f;
	}

	bool IHosePowerable.IsPowered()
	{
		return powerlevel > 1f;
	}


	void Update()
    {
		powerlevel -= Time.deltaTime;
		powerlevel = Mathf.Max(powerlevel, 0);

		this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red * (powerlevel * 0.5f + 0.5f);
    }
}
