using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHosePowerable
{
	void Depower();
	void Power();
	bool NeedsPowerForComplete();
	bool IsPowered();
}
