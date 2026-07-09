using com.VisionXR.HelperClasses;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "BoardDataSO", menuName = "ScriptableObjects/BoardDataSO", order = 1)]
    public class BoardDataSO : ScriptableObject
    {
        [Header("Board Properties")]
        public List<GameObject> Holes;
        public List<GameObject> HolesTriggers;
        public List<GameObject> StrikerRotations;
        public List<SplineContainer> strikerStrips;
        public List<Transform> FinePositions;
        public List<Transform> PlayerPositionsFrontView;
        public List<Transform> PlayerPositionsTopView;
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
            FinePositions.Clear();
            PlayerPositionsFrontView.Clear();
            PlayerPositionsTopView.Clear();
            AvatarPositions.Clear();
            strikerStrips.Clear();
            StrikerRotations.Clear();
        }



        // Getters for the properties
        public List<GameObject> GetHoles() => Holes;
        public Transform GetAllCoinsTransform() => AllCoins;
    
        public List<Transform> GetFinePositions() => FinePositions;
        public Transform GetAvatarPositions(int playerId) => AvatarPositions[playerId - 1].transform;
        public GameObject GetStrikerRotations(int id)
        {
        
           return StrikerRotations[id - 1];
        }

        public SplineContainer GetStrikerStrip(int id)
        {
            if (id >= 1 && id <= strikerStrips.Count)
            {
                return strikerStrips[id - 1];
            }
            else
            {
                Debug.LogError($"Invalid player ID: {id}. Must be between 1 and {strikerStrips.Count}.");
                return null;
            }
        }
        public Transform GetPlayerPositionFrontView(int playerId) => PlayerPositionsFrontView[playerId - 1].transform;
        public Transform GetPlayerPositionTopView(int playerId) => PlayerPositionsTopView[playerId - 1].transform;

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

        public void SetStrikerStrips(List<SplineContainer> strikerStrips)
        {
            this.strikerStrips = strikerStrips;
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

        public void SetStrikerRotations(List<GameObject> strikerRotations)
        {
            StrikerRotations = strikerRotations;
        }

        public void SetPlayerPositionsFrontView(List<Transform> playerPositions)
        {
            PlayerPositionsFrontView = playerPositions;
        }

        public void SetPlayerPositionsTopView(List<Transform> playerPositions)
        {
            PlayerPositionsTopView = playerPositions;
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
        