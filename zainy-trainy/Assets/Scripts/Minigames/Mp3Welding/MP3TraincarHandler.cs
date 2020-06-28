using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP3TraincarHandler : MonoBehaviour, ICanBreakdown
{
	[SerializeField]
	private float currentHealth = 1f;

	[SerializeField]
	private float damagePerSecond;

	TrainCarHealthController healthController;



	void ICanBreakdown.AddDamage(float amountOfDamage)
	{
		Debug.Log(string.Format("Adding damage to mp3 traincar {0}", amountOfDamage));

		currentHealth -= amountOfDamage;
	}

	float ICanBreakdown.GetCurrentRepairStatus()
	{
		return currentHealth;
	}

	void Awake()
    {
		healthController = GetComponentInChildren<TrainCarHealthController>();

	}

	// Update is called once per frame
	void Update()
    {
		UpdateHealth();

	}

	public void UpdateHealth()
	{
		currentHealth = Mathf.Clamp01(currentHealth - Time.deltaTime * damagePerSecond);
		if (healthController != null)
		{
			if (currentHealth <= 0) healthController.ShowBroken();
			else healthController.ShowFunctional();
			Color darkRed = new Color(0.4f, 0.05f, 0.05f);
			Color newColor = Color.Lerp(Color.green, darkRed, Mathf.Pow(1 - currentHealth, 4f));
			healthController.UpdateStatusColor(newColor);
		}
		else
		{
			Debug.Log("No health controller on train car...");
		}
	}
}
