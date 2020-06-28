using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAmAMainTrainPlayer
{
    void EnableCamera();
    void HandleInput(MainTrain.Inputs currentInputs);
}
