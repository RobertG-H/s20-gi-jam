using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGetScoresOnRepairComplete
{
	void RepaireCompleted(ICanBreakdown traincar, IAmAMinigame completedGame, float score, int playerid);//no idea what score should mean
}
