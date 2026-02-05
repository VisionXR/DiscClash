using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.Splines;
namespace com.VisionXR.GameElements
{

    public class AIEasy : MonoBehaviour, IAIBehaviour
    {
        [Header(" Scriptable Objects")]
        public AIDataSO aIData;
        public InputDataSO inputData;
        public BoardDataSO boardData;
        public PlayersDataSO playersData;
        public StrikerDataSO strikerData;


        [Header(" AI Variables")]
        [SerializeField] private Sprite AIIcon;
        [SerializeField] private int comparisonDepth = 4;
        [SerializeField] private AIMovement aIMovement;
        [SerializeField] private float CutOffAngle = 15;


       
        [Header(" Striker Variables")]
        public GameObject Striker;
        public List<Transform> strikerPositions;
        public List<StrikerInfo> strikerDetails;
        public SplineContainer strikerSpline;
        public List<GameObject> holes;
        public LineRenderer debugLine;

        // local variables
        private List<CoinInfo> hitCoinList = new List<CoinInfo>();
        private List<CoinInfo> lastHitCoins = new List<CoinInfo>(); // Stores history of recent coins
        private bool isPaused;
        private bool isExcecuting = false;       
       
        private PlayerCoin playerCoin = PlayerCoin.White;
        private Vector3 dir;
        private float force;
        private int MyId;
        private StrikerMovement strikerMovement;
        private StrikerShooting strikerShooting;

        void OnEnable()
        {
       
            aIData.CoinInformationReceivedEvent += OnHitListReceived;
        }

        void OnDisable()
        {
       
            aIData.CoinInformationReceivedEvent -= OnHitListReceived;
        }
        public void SetStriker(GameObject striker, int id)
        {
            gameObject.name = "AI" + id;
            MyId = id;
            Striker = striker;
            strikerMovement = Striker.GetComponent<StrikerMovement>();  
            strikerShooting = Striker.GetComponent<StrikerShooting>();
            holes = boardData.GetHoles();
            transform.position = boardData.GetAvatarPositions(id).position;
            transform.rotation = boardData.GetAvatarPositions(id).rotation;        
            strikerSpline = boardData.GetStrikerStrip(id);
            FillStrikerDetails();           
            aIMovement.SetStriker(Striker, id);
            isExcecuting = false;

        }

        private void FillStrikerDetails()
        {
            float start = 0.01f;
            float end = 0.99f;
            int totalValues = 11;

            float3 localPos;
            float3 localTangent;
            float3 localUp;

            for (int i = 0; i < totalValues; i++)
            {
                // i / 6 results in: 0, 0.166, 0.333, 0.5, 0.666, 0.833, 1.0
                float t = (float)i / (totalValues - 1);

                // Lerp calculates: start + (end - start) * t
                float normalisedValue = Mathf.Lerp(start, end, t);


                strikerSpline.Evaluate(normalisedValue, out localPos, out localTangent, out localUp);
                Vector3 nudgeDir = (normalisedValue > 0.5f) ? -localTangent : localTangent;
                nudgeDir = nudgeDir.normalized;

                StrikerInfo info = new StrikerInfo
                {
                    normalValue = normalisedValue,
                    strikerPos = localPos,
                    tangentDir = nudgeDir
                };

                strikerDetails.Add(info);

            }
        }

        public void ExecuteShot(PlayerCoin coin)
        {
           
            if (!isExcecuting)
            {
                isExcecuting = true;
                playerCoin = coin;
                aIMovement.MoveHandToStriker();
                hitCoinList.Clear();
                CoinSorter.instance.SortAllCoins(MyId, playerCoin, holes, strikerDetails);
            }
        }

        private void OnHitListReceived(int id, List<CoinInfo> list)
        {
            
            isExcecuting = false;
            if (MyId == id)
            {
                
                hitCoinList = list;
                StartCoroutine(StartExecutingStrike());
            }
        }

        private IEnumerator StartExecutingStrike()
        {

            yield return new WaitForSeconds(aIData.calculatingShotTime);
            yield return StartCoroutine(HitCoin());
        }

        private IEnumerator HitCoin()
        {
          
            while (isPaused)
            {
                yield return new WaitForEndOfFrame();
            }


            int coinIndex = GetIndexOfNextCoin();


            CoinInfo currentSelectedCoin = hitCoinList[coinIndex];

            // Add the selected coin to last hit history
            UpdateLastHitCoins(currentSelectedCoin);


            // Set force and striker position
            force = currentSelectedCoin.distance + 1.1f;
            Striker.transform.position = strikerMovement.FindStrikerNextPosition(currentSelectedCoin.strikerInfo.strikerPos, currentSelectedCoin.strikerInfo.tangentDir);
            Striker.transform.rotation = boardData.GetStrikerRotations(MyId).transform.rotation;

            Debug.Log("Angle " + currentSelectedCoin.angle);

            // Strike if angle is within range
            if (currentSelectedCoin.angle < CutOffAngle)
            {

                debugLine.positionCount = 3;
                debugLine.SetPosition(0, Striker.transform.position);
                debugLine.SetPosition(1, currentSelectedCoin.FinalPos);
                debugLine.SetPosition(2, currentSelectedCoin.Hole.transform.position);

                dir = (currentSelectedCoin.FinalPos - Striker.transform.position).normalized;
                yield return Strike(dir, force, currentSelectedCoin);

              

            }
            else
            {
                debugLine.positionCount = 3;
                debugLine.SetPosition(0, Striker.transform.position);
                debugLine.SetPosition(1, currentSelectedCoin.Coin.transform.position);
                debugLine.SetPosition(2, currentSelectedCoin.Hole.transform.position);

                dir = (currentSelectedCoin.Coin.transform.position - Striker.transform.position).normalized;
                yield return Strike(dir, force, currentSelectedCoin);
               
            }

        }

        private IEnumerator Strike(Vector3 direction, float strikeForce, CoinInfo coinInfo)
        {
         
            aIMovement.ShowFingerCloseAnimation(coinInfo.Coin.transform.position);
            yield return new WaitForSeconds(aIData.strikeWaitTime);
            aIMovement.ShowFingerStrikeAnimation(coinInfo.Coin.transform.position);
            yield return new WaitForSeconds(0.1f);
           
            strikerShooting.FireStriker(direction, strikeForce);
            yield return new WaitForSeconds(0.1f);
            hitCoinList.Clear();
        }

        private void UpdateLastHitCoins(CoinInfo coinInfo)
        {
            // Add the coin to the recent history
            lastHitCoins.Add(coinInfo);

            // Ensure list does not exceed the comparisonDepth
            if (lastHitCoins.Count > comparisonDepth)
            {
                lastHitCoins.RemoveAt(0);
            }
        }

        private int GetIndexOfNextCoin()
        {
            // Loop through each coin in the list
            for (int i = 0; i < hitCoinList.Count; i++)
            {
                bool isRecentlyHit = false;

                // Compare with the recent history up to comparisonDepth
                foreach (var recentCoin in lastHitCoins)
                {
                    if (hitCoinList[i].Coin == recentCoin.Coin && hitCoinList[i].strikerInfo.normalValue == recentCoin.strikerInfo.normalValue && hitCoinList[i].Hole == recentCoin.Hole)
                    {
                        isRecentlyHit = true;
                        break;
                    }
                }

                // If coin is not recently hit, select it
                if (!isRecentlyHit)
                {
                    return i;
                }
            }

            // Default to first coin if all are recently hit
            return 0;
        }


    }
}

