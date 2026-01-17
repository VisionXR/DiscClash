using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace com.VisionXR.GameElements
{

    public class AIMedium : MonoBehaviour, IAIBehaviour
    {
        [Header(" Scriptable Objects")]
        public AIDataSO aIData;
        public InputDataSO inputData;
        public BoardPropertiesSO boardProperties;
        public PlayersDataSO playersData;
        public StrikerDataSO strikerData;


        [Header(" Local Variables")]
        [SerializeField] private Sprite AIIcon;
        [SerializeField] private int comparisonDepth = 4;
        [SerializeField] private AIMovement aIMovement;
        [SerializeField] private float CutOffAngle = 30;
        private bool isPaused;
        private bool isExcecuting = false;

        // local variables
        private GameObject Striker;


        private List<CoinInfo> hitCoinList = new List<CoinInfo>();
        public List<CoinInfo> lastHitCoins = new List<CoinInfo>(); // Stores history of recent coins


        private List<GameObject> strikerPositions;
        private List<GameObject> holes;
        private PlayerCoin playerCoin = PlayerCoin.White;
        private Vector3 dir;
        private float force;
        private int MyId;




        void OnEnable()
        {
            inputData.PauseGameEvent += PauseAI;
            inputData.ResumeGameEvent += ResumeAI;
            aIData.CoinInformationReceivedEvent += OnHitListReceived;
        }

        void OnDisable()
        {
            inputData.PauseGameEvent -= PauseAI;
            inputData.ResumeGameEvent -= ResumeAI;
            aIData.CoinInformationReceivedEvent -= OnHitListReceived;
        }



        public void SetStriker(GameObject striker, int id)
        {
            gameObject.name = "AI" + id;
            MyId = id;
            Striker = striker;
            holes = boardProperties.GetHoles();
            transform.position = boardProperties.GetAvatarPositions(id).position;
            transform.rotation = boardProperties.GetAvatarPositions(id).rotation;
            GetStrikerPositions();
            aIMovement.SetStriker(Striker, id);
            isExcecuting = false;

        }


        private void GetStrikerPositions()
        {

            strikerPositions = boardProperties.GetStrikerPosition(MyId);

        }
        public void ExecuteShot(PlayerCoin coin)
        {
            if (!isExcecuting)
            {
                isExcecuting = true;
                playerCoin = coin;
                aIMovement.MoveHandToStriker();
                hitCoinList.Clear();
                CoinSorter.instance.SortAllCoins(MyId, playerCoin, holes, strikerPositions);
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
            Striker.transform.position = Striker.GetComponent<IStrikerMovement>()
                .FindStrikerNextPosition(currentSelectedCoin.StrikerPos.transform.position, currentSelectedCoin.StrikerPos.transform.right);

            // Strike if angle is within range
            if (currentSelectedCoin.angle < CutOffAngle)
            {
                dir = (currentSelectedCoin.FinalPos - Striker.transform.position).normalized;
                yield return Strike(dir, force, currentSelectedCoin);
            }
            else
            {
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
            Striker.GetComponent<IStrikerShoot>().FireStriker(direction, strikeForce);
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
                    if (hitCoinList[i].Coin == recentCoin.Coin && hitCoinList[i].StrikerPos == recentCoin.StrikerPos && hitCoinList[i].Hole == recentCoin.Hole)
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


        public void PauseAI()
        {
            isPaused = true;
        }

        public void ResumeAI()
        {
            isPaused = false;
        }

    }
}

