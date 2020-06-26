using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAmAMinigame
{
	bool GetIsMinigameCurrentlyRunning();
	void OpenMinigame(int playerid);//DO WE NEED TO PASS A PLAYER ID??!?!?!
	int GetLastPlayerWhoPlayed();
}
