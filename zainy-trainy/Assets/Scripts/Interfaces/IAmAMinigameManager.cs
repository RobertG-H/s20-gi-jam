using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAmAMinigameManager
{
    void DisableControls();
    void EnableControls();
    void RegisterPlayer(IAmAMainTrainPlayer p);
}
