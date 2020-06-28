using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace MainTrain
{
    public struct Inputs
    {
        public bool left;
        public bool right;
        public bool up;
        public bool down;
        public bool space;

    }

    public class MainTrainInputManager : MonoBehaviour, IRegisterMainTrainInputs, IServiceProvider
    {
        private IAmAMainTrainPlayer player;

        public Inputs currentInputs;

        void Update()
        {
            if(player != null) player.HandleInput(currentInputs);
        }


        void IServiceProvider.RegisterServices()
        {
            try
            {
                this.RegisterService<IRegisterMainTrainInputs>();

            }catch(ArgumentNullException e)
            {
                Debug.Log("Error in IOC REGISTER FOR maintrianinput");
                this.RegisterService<IRegisterMainTrainInputs>();

            }

        }

        void IRegisterMainTrainInputs.RegisterPlayer(MainTrainPlayerController p)
        {
            this.player = p;
            Debug.Log("Registered Player");
        }

        public void OnWKey(InputAction.CallbackContext context)
        {
            currentInputs.up = context.ReadValue<float>() == 1;
        }

        public void OnAKey(InputAction.CallbackContext context)
        {
            currentInputs.left = context.ReadValue<float>() == 1;
            // override opposite direction
            if (currentInputs.left) currentInputs.right = false;

        }

        public void OnSKey(InputAction.CallbackContext context)
        {
            currentInputs.down = context.ReadValue<float>() == 1;

        }

        public void OnDKey(InputAction.CallbackContext context)
        {
            currentInputs.right = context.ReadValue<float>() == 1;
            // override opposite direction
            if (currentInputs.right) currentInputs.left = false;

        }

        public void OnSpaceKey(InputAction.CallbackContext context)
        {
            currentInputs.space = context.ReadValueAsButton();
            player.HandleInput(currentInputs);
        }

    }
}
