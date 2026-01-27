using com.VisionXR.HelperClasses;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "BoardDataSO", menuName = "ScriptableObjects/BoardDataSO", order = 1)]
    public class BoardDataSO : ScriptableObject
    {
        [Header("Board Properties")]
        public List<GameObject> Holes;
        public List<GameObject> HolesTriggers;
        public List<Transform> Striker1Positions;
        public List<Transform> Striker2Positions;
        public List<Transform> Striker3Positions;
        public List<Transform> Striker4Positions;
        public List<Transform> FinePositions;
        public List<Transform> PlayerPositions;
        public List<Transform> AvatarPositions;

        public GameObject Ground;
        public Transform AllCoins;
        public float StrikerRadius;
        public float CoinRadius;


        private void Awake()
        {
            ClearData();
        }

        private void ClearData()
        {
            Holes.Clear();
            HolesTriggers.Clear();
            Striker1Positions.Clear();
            Striker2Positions.Clear();
            Striker3Positions.Clear();
            Striker4Positions.Clear();
            FinePositions.Clear();
            PlayerPositions.Clear();
            AvatarPositions.Clear();
        }



        // Getters for the properties
        public List<GameObject> GetHoles() => Holes;
        public Transform GetAllCoinsTransform() => AllCoins;
    
        public List<Transform> GetFinePositions() => FinePositions;
        public Transform GetAvatarPositions(int playerId) => AvatarPositions[playerId - 1].transform;
        public List<Transform> GetStrikerPosition(int id)
        {
        
            if(id == 1)
            {
                Debug.Log("Getting Striker Positions for ID: " + Striker1Positions.Count);
                return Striker1Positions;
            }
            else if(id == 2)
            {
                return Striker2Positions;
            }
            else if(id == 3)
            {
                return Striker3Positions;
            }
            else // id == 4
            {
                return Striker4Positions;
            }
        }
        public Transform GetPlayerPosition(int playerId) => PlayerPositions[playerId - 1].transform;
    
        public float GetStrikerRadius() => StrikerRadius;
        public float GetCoinRadius() => CoinRadius;

        // Setters for the properties
        public void SetHoles(List<GameObject> holes)
        {
            Holes = holes;
        }

        public void SetGround(GameObject ground)
        {
            Ground = ground;
        }

        public void SetHoleTriggers(List<GameObject> holesTriggers)
        {
            HolesTriggers = holesTriggers;
        }

        public void SetAllCoinsTransform(Transform allCoins)
        {
            AllCoins = allCoins;
        }

        public void SetFinePositions(List<Transform> finePositions)
        {
            FinePositions = finePositions;
        }

        public void SetAvatarPositions(List<Transform> avatarPositions)
        {
            AvatarPositions = avatarPositions;
        }

        public void SetStrikerPositions(int id, List<Transform> strikerPositions)
        {
            switch (id)
            {
                case 1:
                    Striker1Positions = strikerPositions;
                    break;
                case 2:
                    Striker2Positions = strikerPositions;
                    break;
                case 3:
                    Striker3Positions = strikerPositions;
                    break;
                case 4:
                    Striker4Positions = strikerPositions;
                    break;
            }
        }

        public void SetPlayerPositions(List<Transform> playerPositions)
        {
            PlayerPositions = playerPositions;
        }

        public void SetStrikerRadius(float strikerRadius)
        {
            StrikerRadius = strikerRadius;
        }

        public void SetCoinRadius(float coinRadius)
        {
            CoinRadius = coinRadius;
        }
        public void TurnOffHoles()
        { 
        
          foreach(GameObject h in HolesTriggers) 
            {
                h.SetActive(false);
            }

          if(Ground != null)
            {
                Ground.SetActive(false);
            }
        }

        public void TurnOnHoles()   
        {

            foreach (GameObject h in HolesTriggers)
            {
                h.SetActive(true);
            }

            if (Ground != null)
            {
                Ground.SetActive(true);
            }

        }

    }
}
        