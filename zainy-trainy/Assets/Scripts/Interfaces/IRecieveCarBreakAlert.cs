using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRecieveCarBreakAlert
{
	void TraincarIsBroken(GameObject Traincar, ICanBreakdown traincar, IAmAMinigame minigame);
	void TraincarIsDamaged(GameObject Traincar, ICanBreakdown traincar, IAmAMinigame minigame);
}
