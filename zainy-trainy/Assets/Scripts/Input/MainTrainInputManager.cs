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

    }

    public class MainTrainInputManager : MonoBehaviour, IRegisterMainTrainInputs, IServiceProvider, IRecieveCarBreakAlert, IGetScoresOnRepairComplete
    {
        private IAmAMainTrainPlayer player;
        private GameObject playerToDisable;

        [SerializeField]
        GameObject moduleObject;

        public Inputs currentInputs;

        void Start()
        {
        }

        void Update()
        {
            player.HandleInput(currentInputs);
        }

        void IServiceProvider.RegisterServices()
        {
            this.RegisterService<IRegisterMainTrainInputs>();
            this.RegisterService<IRecieveCarBreakAlert>();
            this.RegisterService<IGetScoresOnRepairComplete>();
        }

        void IRegisterMainTrainInputs.RegisterPlayer(MainTrainPlayerController p)
        {
            this.player = p;
            playerToDisable = p.gameObject;
        }

        void IGetScoresOnRepairComplete.RepaireCompleted(ICanBreakdown traincar, IAmAMinigame completedGame, float score, int playerid)
        {
            print("FINISHED A GAME");
            playerToDisable.SetActive(true);
        }

        void IRecieveCarBreakAlert.TraincarIsBroken(GameObject Traincar, ICanBreakdown traincar, IAmAMinigame minigame)
        {
            Debug.Log("i dont care");
        }

        void IRecieveCarBreakAlert.TraincarIsDamaged(GameObject Traincar, ICanBreakdown traincar, IAmAMinigame minigame)
        {
            Debug.Log("i dont care");
        }

        public void OnWKey(InputAction.CallbackContext context)
        {
            currentInputs.up = context.ReadValue<float>() == 1;
            if(currentInputs.up)
            {
                moduleObject.GetComponent<IAmAMinigame>().OpenMinigame(1);
                playerToDisable.SetActive(false);
            }
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

    }
}
