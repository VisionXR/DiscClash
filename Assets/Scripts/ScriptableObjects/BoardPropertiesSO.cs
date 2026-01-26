using com.VisionXR.HelperClasses;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "BoardPropertiesSO", menuName = "ScriptableObjects/BoardPropertiesSO", order = 1)]
    public class BoardPropertiesSO : ScriptableObject
    {
        [Header("Board Properties")]
        [SerializeField] private List<GameObject> Holes = new List<GameObject>(); 
        [SerializeField] private List<GameObject> HolesTriggers = new List<GameObject>();
        [SerializeField] private List<Transform> Striker1Positions = new List<Transform>();
        [SerializeField] private List<Transform> Striker2Positions = new List<Transform>();
        [SerializeField] private List<Transform> Striker3Positions = new List<Transform>();
        [SerializeField] private List<Transform> Striker4Positions = new List<Transform>();
        [SerializeField] private List<Transform> FinePositions = new List<Transform>();
        [SerializeField] private List<Transform> PlayerPositions = new List<Transform>();
        [SerializeField] private List<Transform> AvatarPositions = new List<Transform>();

        [SerializeField] private GameObject Ground;
        [SerializeField] private Transform AllCoins;
        [SerializeField] private float StrikerRadius;
        [SerializeField] private float CoinRadius;



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
        