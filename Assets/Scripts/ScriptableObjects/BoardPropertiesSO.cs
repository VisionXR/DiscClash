using com.VisionXR.HelperClasses;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "BoardPropertiesSO", menuName = "ScriptableObjects/BoardPropertiesSO", order = 1)]
    public class BoardPropertiesSO : ScriptableObject
    {
        [Header("Board Properties")]
        [SerializeField] private List<GameObject> Holes;
        [SerializeField] private GameObject Ground;
        [SerializeField] private List<GameObject> HolesTriggers;
        [SerializeField] private Transform AllCoins;
        [SerializeField] public List<Transform> AllCoinsPositions;
        [SerializeField] private List<GameObject> Striker1Positions;
        [SerializeField] private List<GameObject> Striker2Positions;
        [SerializeField] private List<GameObject> Striker3Positions;
        [SerializeField] private List<GameObject> Striker4Positions;
        [SerializeField] private List<Transform> FinePositions;
        [SerializeField] private List<Transform> PlayerPositions;
        [SerializeField] private List<Transform> AvatarPositions;
        [SerializeField] private List<GameObject> MainCanvasPositions;
        [SerializeField] private float StrikerRadius;
        [SerializeField] private float CoinRadius;
        [SerializeField] private float BoardHeight;

        // Getters for the properties
        public List<GameObject> GetHoles() => Holes;
        public Transform GetAllCoinsTransform() => AllCoins;
        public List<Transform> GetAllCoinsPositions() => AllCoinsPositions;
        public List<Transform> GetFinePositions() => FinePositions;
        public Transform GetAvatarPositions(int playerId) => AvatarPositions[playerId - 1].transform;
        public Transform GetMainCanvasPosition(int playerID) => MainCanvasPositions[playerID - 1].transform;
        public List<GameObject> GetStrikerPosition(StrikerName name)
        {
            return name switch
            {
                StrikerName.Striker1 => Striker1Positions,
                StrikerName.Striker2 => Striker2Positions,
                StrikerName.Striker3 => Striker3Positions,
                _ => Striker4Positions
            };
        }
        public List<GameObject> GetStrikerPosition(int id)
        {
            return id switch
            {
                1 => Striker1Positions,
                2 => Striker2Positions,
                3 => Striker3Positions,
                _ => Striker4Positions
            };
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

        public void SetAllCoinsPositions(List<Transform> allCoinsPositions)
        {
            AllCoinsPositions = allCoinsPositions;
        }

        public void SetFinePositions(List<Transform> finePositions)
        {
            FinePositions = finePositions;
        }

        public void SetAvatarPositions(List<Transform> avatarPositions)
        {
            AvatarPositions = avatarPositions;
        }


        public void SetMainCanvasPositions(List<GameObject> mainCanvasPositions)
        {
            MainCanvasPositions = mainCanvasPositions;
        }

        public void SetStrikerPositions(int id, List<GameObject> strikerPositions)
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

        public void SetBoardHeight(float boardHeight)
        {
            BoardHeight = boardHeight;
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
        