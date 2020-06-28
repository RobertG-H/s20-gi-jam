using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoseMinigameHandler : MonoBehaviour
{
	[HideInInspector]
	public HoseHandler hoseHandler;

	List<HoseLevel> hoseLevels;

	private void Awake()
	{
		hoseLevels = new List<HoseLevel>(this.gameObject.GetComponentsInChildren<HoseLevel>(true));
		hoseHandler = this.GetComponentInChildren<HoseHandler>();
	}



	private void OnEnable()
	{
		hoseHandler.isPlugged = false;
		int randind = Random.Range(0, hoseLevels.Count);

		for(int i =0; i < hoseLevels.Count; i++)
		{
			hoseLevels[i].gameObject.SetActive(i == randind);
		}

		hoseLevels[randind].gameObject.SetActive(true);
		hoseHandler.activeLevel = hoseLevels[randind];
		hoseHandler.baseTrans = hoseLevels[randind].anchorObject.transform;
		countdowntoquit = 1f;
		hoseHandler.nozzleObject.position = hoseHandler.baseTrans.position;
		hoseHandler.handObject.position = hoseHandler.baseTrans.position;
	}

	float countdowntoquit = 1f;
	private void Update()
	{
		if(hoseHandler.isPlugged)
		{
			countdowntoquit -= Time.deltaTime;
		}

		if(countdowntoquit <0f)
		{
			hoseHandler.Clear();
			this.gameObject.GetComponentInParent<HoseModuleManager>().MinigameCompleted(1f);
		}
	}
}
