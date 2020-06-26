using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanBreakdown
{
	void AddDamage(float amountOfDamage);//from 0-1??
	float GetCurrentRepairStatus();//from 0-1 0=broken, 1=fine
}
