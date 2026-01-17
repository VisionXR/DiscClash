using System;
using UnityEngine;
using com.VisionXR.HelperClasses;
using System.Collections.Generic;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "AIDataSO", menuName = "ScriptableObjects/AIDataSO", order = 1)]
    public class AIDataSO : ScriptableObject
    {
        // variables
        public float rotationSpeed = 45f;  // Adjust as needed
        public float positionSpeed = 4;
        public float calculatingShotTime = 2f;
        public float strikeWaitTime = 3;
      //  public float gapBeforeShot

        // Events
        public Action<GameObject,int, AIDifficulty,Action<GameObject>> CreateAIBotEvent;
        public Action<string> SendAIBotAnimationEvent;
        public Action<string> ReceiveAIBotAnimationEvent;
        public Action<int ,List<CoinInfo>> CoinInformationReceivedEvent;


        // Methods
        public void CreateBot(GameObject striker,int id,AIDifficulty difficulty,Action<GameObject> AICreatedEvent)
        {
            CreateAIBotEvent?.Invoke(striker,id, difficulty,AICreatedEvent);
        }
        public void CoinInformationReceived(int id, List<CoinInfo> info)
        {
            CoinInformationReceivedEvent?.Invoke(id, info);
        }

        public void SendAIBotAnimation(string data)
        {
            SendAIBotAnimationEvent?.Invoke(data);
        }

        public void ReceiveAIBotAnimation(string data)
        {
            ReceiveAIBotAnimationEvent?.Invoke(data);
        }
    }
}
