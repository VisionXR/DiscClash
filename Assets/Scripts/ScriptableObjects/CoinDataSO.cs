using com.VisionXR.HelperClasses;
using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "CoinDataSO", menuName = "ScriptableObjects/CoinDataSO", order = 1)]
    public class CoinDataSO : ScriptableObject
    {
        // variables
        
        public int WhiteCount = 10;
        public int BlackCount = 10;
        public int RedCount = 2;
        public int Whites, Blacks, Red;
        public List<string> CoinFellInThisTurn = new List<string>();
        public List<Rigidbody> AvailableCoinsInGame = new List<Rigidbody>();
        public List<GameObject> BlackCoins = new List<GameObject>();
        public List<GameObject> WhiteCoins = new List<GameObject>();
        public GameObject RedCoin;
        public GameObject AllCoinsReference;
        public float AllCoinsYRotationValue;
        public float baseLerpDuration = 0.0375f;



        // Events
        public Action CreateAllCoinsEvent;
        public Action DestroyAllCoinsEvent;

        public Action ShowRotationCanvasEvent;
        public Action<float> RotateCoinsEvent;
        public Action<float> SetAllCoinsRotationEvent;


        public Action<PlayerCoin> CreateCoinEvent;
        public Action<string> DestroyCoinEvent;


        public Action<Rigidbody> RegisterCoinEvent;
        public Action<Rigidbody> DeRegisterCoinEvent;
        public Action<List<string>> DestroyCoinsFellInThisTurnEvent;


        public Action<GameObject> CoinFellInHoleEvent;
        public Action<GameObject> CoinFellOnGroundEvent;
        public Action<GameObject> CoinpocketedUntoHoleEvent;


        public Action ShowFoulEvent;
        // Methods
        private void OnEnable()
        {
            AvailableCoinsInGame.Clear();
            BlackCoins.Clear();
            WhiteCoins.Clear();

            ResetData();
            ResetCount();
        }

        public void ResetCount()
        {
            WhiteCount = 10;
            BlackCount = 10;
            RedCount = 2;
        }

        public void ResetData()
        {
          
            Whites = 0;
            Blacks = 0;
            Red = 0;
            CoinFellInThisTurn.Clear();
        }


        public void DestroyCoinsFellInthisTurn(List<string> coins)
        {
            DestroyCoinsFellInThisTurnEvent?.Invoke(coins);
        }

        public void CoinFellInHole(GameObject striker)
        {
            CoinFellInHoleEvent?.Invoke(striker);         
        }
        public void CoinFellOnGround(GameObject striker)
        {
            CoinFellOnGroundEvent?.Invoke(striker);
        }
        public void CoinPocketedUntoHole(GameObject hole)
        {
           
            CoinpocketedUntoHoleEvent?.Invoke(hole);
        }
        public void CreateAllCoins()
        {
            CreateAllCoinsEvent?.Invoke();
        }
        public void DestroyAllCoins()
        {
            DestroyAllCoinsEvent?.Invoke();
        }
        public void CreateCoin(PlayerCoin coin)
        {
            CreateCoinEvent?.Invoke(coin);
        }
    
        public void RegisterCoin(Rigidbody coin)
        {
            RegisterCoinEvent?.Invoke(coin);
            SortCoinsByName();
        }
        public void DeRegisterCoin(Rigidbody coin)
        {
            DeRegisterCoinEvent?.Invoke(coin);
            SortCoinsByName();
        }

        public void SortCoinsByName()
        {
            AvailableCoinsInGame.Sort((a, b) => string.Compare(a.gameObject.name, b.gameObject.name, StringComparison.Ordinal));
        }

        public void CoinPocketed(string name)
        {
            CoinFellInThisTurn.Add(name);
        }
        public List<string> GetCoinsFellInThisTurn()
        {           
            return CoinFellInThisTurn;
        }
        public float GetAllCoinsRotationValue()
        {
            return AllCoinsYRotationValue;
        }



        public void RotateCoins(float value)
        {
            RotateCoinsEvent?.Invoke(value);
        }

        public void SetAllCoinsRotation(float value)
        {
            SetAllCoinsRotationEvent?.Invoke(value);
        }

  

    }
}
