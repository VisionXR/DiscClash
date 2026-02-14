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
        [SerializeField] private float forceAdder = 1;


       
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
        public StrikerArrow strikerArrow;

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
            strikerArrow = Striker.GetComponent<StrikerArrow>();
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
            int totalValues = 5;

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

            Striker.transform.position = strikerMovement.FindStrikerNextPosition(currentSelectedCoin.strikerInfo.strikerPos, currentSelectedCoin.strikerInfo.tangentDir);
            Striker.transform.rotation = boardData.GetStrikerRotations(MyId).transform.rotation;

            yield return new WaitForSeconds(0.5f);

            Vector3 worldDir = (currentSelectedCoin.FinalPos - Striker.transform.position).normalized;

            StartCoroutine(RotateStrikerTowards(worldDir, 0.5f));
            yield return new WaitForSeconds(0.5f);


            // Strike if angle is within range
            if (currentSelectedCoin.angle < CutOffAngle)
            {

                debugLine.positionCount = 3;
                debugLine.SetPosition(0, Striker.transform.position);
                debugLine.SetPosition(1, currentSelectedCoin.FinalPos);
                debugLine.SetPosition(2, currentSelectedCoin.Hole.transform.position);

                // Non-linear weighting for angle contribution
                float a = Mathf.Clamp01(currentSelectedCoin.angle / CutOffAngle);
                // Choose one curve:
                float w = a * a;

                force = currentSelectedCoin.distance + w * forceAdder + 0.5f;

                dir = (currentSelectedCoin.FinalPos - Striker.transform.position).normalized;
                yield return Strike(dir, force, currentSelectedCoin);

              

            }
            else
            {
                debugLine.positionCount = 3;
                debugLine.SetPosition(0, Striker.transform.position);
                debugLine.SetPosition(1, currentSelectedCoin.Coin.transform.position);
                debugLine.SetPosition(2, currentSelectedCoin.Hole.transform.position);


                force = currentSelectedCoin.distance + forceAdder + 0.5f;
                dir = (currentSelectedCoin.Coin.transform.position - Striker.transform.position).normalized;
                yield return Strike(dir, force, currentSelectedCoin);
               
            }

        }

        /// <summary>
        /// Smoothly rotate the striker so its forward points toward worldDirection (keeps y-axis stable).
        /// duration is seconds for the rotation to complete.
        /// </summary>
        private IEnumerator RotateStrikerTowards(Vector3 worldDirection, float duration)
        {
            // flatten direction to horizontal plane to avoid unwanted pitch
            Vector3 flatDir = new Vector3(worldDirection.x, 0f, worldDirection.z);
            if (flatDir.sqrMagnitude <= 0.0001f)
                yield break;

            Quaternion startRot = Striker.transform.rotation;
            Quaternion targetRot = Quaternion.LookRotation(flatDir.normalized, Vector3.up);

            float t = 0f;
            // if duration is zero or tiny, snap immediately
            if (duration <= 0f)
            {
                Striker.transform.rotation = targetRot;
                yield break;
            }

            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                Striker.transform.rotation = Quaternion.Slerp(startRot, targetRot, Mathf.SmoothStep(0f, 1f, t));
                yield return null;
            }

            Striker.transform.rotation = targetRot;
        }

        private IEnumerator Strike(Vector3 direction, float strikeForce, CoinInfo coinInfo)
        {
         
            aIMovement.ShowFingerCloseAnimation(coinInfo.Coin.transform.position);
            yield return new WaitForSeconds(aIData.strikeWaitTime);
            aIMovement.ShowFingerStrikeAnimation(coinInfo.Coin.transform.position);
            yield return new WaitForSeconds(0.1f);
           
            strikerShooting.FireStriker(direction, strikeForce);

            strikerArrow.TurnOffArrow();
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

