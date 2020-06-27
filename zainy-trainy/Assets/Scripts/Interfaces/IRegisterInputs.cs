using UnityEngine;
using System.Collections;

public interface IRegisterInputs
{
    void RegisterMouseInputObject(IRecieveMouseInputs inputObject);
    void RegisterWASDInputObject(IRecieveWASDInputs inputObject);
}
