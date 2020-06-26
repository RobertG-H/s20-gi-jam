using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTraincarHandler : MonoBehaviour, ICanBreakdown
{
	[SerializeField]
	private float currentHealth = 1f;


	void ICanBreakdown.AddDamage(float amountOfDamage)
	{
		currentHealth -= amountOfDamage;
	}

	float ICanBreakdown.GetCurrentRepairStatus()
	{
		return currentHealth;
	}

	void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
		currentHealth = Mathf.Clamp01(currentHealth-Time.deltaTime * 0.1f);

		this.GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.green, Color.red, Mathf.Pow(1 - currentHealth, 4f));
    }
}
