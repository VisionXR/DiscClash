using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "StrikerDataSO", menuName = "ScriptableObjects/StrikerDataSO", order = 1)]
    public class StrikerDataSO : ScriptableObject
    {
        // variables
        public float ForceLowerLimit = 0.2f;
        public float ForceUpperLimit = 5;
        public float LeftLimit = -0.3f;
        public float RightLimit = 0.3f;
        public bool isFoul = false;
        public float baseLerpDuration = 0.0375f; // (3 / 80), adjust this as per your send interval
        public List<GameObject> AvailableStrikersinGame = new List<GameObject>();

        // Events
        public Action<int, int,Action<GameObject>> CreateStrikerEvent;
        public Action<int> DestroyStrikerEvent;
        public Action<int, int> ChangeStrikerEvent;
        public Action<int, int> NetworkStrikerEvent;
       
        public Action<StrikerData> SetStrikerDataEvent;
        public Action<StrikerData> SetInitialStrikerDataEvent;


        public Action TurnOnRigidbodiesEvent;
        public Action TurnOffRigidbodiesEvent;

        public Action TurnOnStrikerArrowEvent;
        public Action TurnOffStrikerArrowEvent;

        public Action<GameObject> StrikerFellInHoleEvent;
        public Action<GameObject> StrikerpocketedUntoHoleEvent;

        public Action<string> audioPlayedEvent;

        public Action ResetStrikerDataEvent;

        // Methods
        private void OnEnable()
        {
            AvailableStrikersinGame.Clear();
        }

        public void ChangeStriker(int playerId,int strikerId)
        {
            ChangeStrikerEvent?.Invoke(playerId,strikerId);
        }

        public void StrikerFellInHole(GameObject striker)
        {
            StrikerFellInHoleEvent?.Invoke(striker);

        }

        public void StrikerPocketedUntoHole(GameObject hole)
        {
            StrikerpocketedUntoHoleEvent?.Invoke(hole);
        }

        public void CreateStriker(int id, int strikerID,Action<GameObject> strikerCreatedEvent)
        {
            CreateStrikerEvent?.Invoke(id, strikerID, strikerCreatedEvent);
        }
        public void DestroyStriker(int id)
        {
            DestroyStrikerEvent?.Invoke(id);
        }

        public void SetFoul()
        {
            isFoul = true;
        }
        public void ResetFoul()
        {
            isFoul = false;
        }

        public void TurnOnStrikerArrow()
        {
            TurnOnStrikerArrowEvent?.Invoke();
        }
        public void TurnOffStrikerArrow()
        {
            TurnOffStrikerArrowEvent?.Invoke();
        }


        public GameObject GetStriker(int id)
        {
            foreach (GameObject s in AvailableStrikersinGame)
            {
                if (s.GetComponent<StrikerMovement>().strikerId == id)
                {
                    return s;
                }

            }
            return null;
        }

        public void ResetStrikerData()
        {
            ResetStrikerDataEvent?.Invoke();
        }
    }
}
