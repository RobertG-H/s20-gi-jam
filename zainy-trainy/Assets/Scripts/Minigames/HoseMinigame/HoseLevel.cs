using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoseLevel : MonoBehaviour
{
	public GameObject anchorObject;
	List<CircleCollider2D> nodesGot;
	public List<CircleCollider2D> nodes
	{
		get
		{
			if(nodesGot == null)
				nodesGot = new List<CircleCollider2D>(this.gameObject.GetComponentsInChildren<CircleCollider2D>());
			return nodesGot;
		}
	}
}
