using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public  class CoinSorter : MonoBehaviour
{
    public static CoinSorter instance;

    [Header(" Scriptable Objects")]
    public BoardPropertiesSO boardProperties;
    public CoinDataSO coinData;
    public AIDataSO aiData;

    [Header(" Local Objects")]
    public List<CoinInfo> coinInfoList = new List<CoinInfo>();
    private List<GameObject> blackCoins;
    private List<GameObject> whiteCoins;
    private GameObject redCoin;
    private int id;


    private void Awake()
    {
        instance = this;
    }
    public  void  SortAllCoins(int id,PlayerCoin myCoin,List<GameObject> holes, List<GameObject> strikerPositions)
    {
          
        coinInfoList.Clear();
        this.id = id;
        StartCoroutine(Sort(myCoin,holes,strikerPositions));
      
    }

    private IEnumerator Sort(PlayerCoin myCoin, List<GameObject> holes, List<GameObject> strikerPositions)
    {
        blackCoins = coinData.BlackCoins;
        whiteCoins = coinData.WhiteCoins;
        redCoin = coinData.RedCoin;

        if (myCoin == PlayerCoin.White)
        {
            foreach (GameObject coin in whiteCoins)
            {

                foreach (GameObject hole in holes)
                {
                    foreach (GameObject pos in strikerPositions)
                    {

                        GetCoinInfo(coin, hole, pos);
                        yield return new WaitForEndOfFrame();
                    }
                  
                }
                
            }

        }
        else if (myCoin == PlayerCoin.Black)
        {
            foreach (GameObject coin in blackCoins)
            {
                foreach (GameObject hole in holes)
                {
                    foreach (GameObject pos in strikerPositions)
                    {
                        GetCoinInfo(coin, hole, pos);
                        yield return new WaitForEndOfFrame();
                    }
                   
                }
               
            }

        }
        else if (myCoin == PlayerCoin.Red)
        {
            foreach (GameObject hole in holes)
            {
                foreach (GameObject pos in strikerPositions)
                {
                    GetCoinInfo(redCoin, hole, pos);
                    yield return new WaitForEndOfFrame();
                }
                
            }

        }
        else if (myCoin == PlayerCoin.All)
        {
            foreach (GameObject coin in whiteCoins)
            {

                foreach (GameObject hole in holes)
                {
                    foreach (GameObject pos in strikerPositions)
                    {
                        GetCoinInfo(coin, hole, pos);
                        yield return new WaitForEndOfFrame();
                    }
                   
                }
              
            }
            foreach (GameObject coin in blackCoins)
            {

                foreach (GameObject hole in holes)
                {
                    foreach (GameObject pos in strikerPositions)
                    {
                        GetCoinInfo(coin, hole, pos);
                        yield return new WaitForEndOfFrame();
                    }
                   
                }
               
            }
            foreach (GameObject hole in holes)
            {
                foreach (GameObject pos in strikerPositions)
                {
                    if (redCoin != null)
                    {
                        GetCoinInfo(redCoin, hole, pos);
                        yield return new WaitForEndOfFrame();
                    }
                }
              
            }
        }

        if (coinInfoList.Count > 1)
        {
            SortCoinsByBlockedParameter();
            SortCoinsByAngleParameter();
        }

        aiData.CoinInformationReceived(id, coinInfoList);
    }

    private void GetCoinInfo(GameObject coin, GameObject hole, GameObject StrikerPosition)
    {

        if (coin == null)
        {
            return;
        }
        CoinInfo coinInfo = new CoinInfo();
        RaycastHit hitInfo;
        coinInfo.Coin = coin;
        coinInfo.Hole = hole;
        coinInfo.StrikerPos = StrikerPosition;
        Vector3 holedir = (hole.transform.position - coin.transform.position).normalized;
        Vector3 finalPos = coin.transform.position - holedir * (boardProperties.GetCoinRadius() + boardProperties.GetStrikerRadius());
        coinInfo.FinalPos = finalPos;
        Vector3 Strikerdir = (finalPos - StrikerPosition.transform.position).normalized;
        coinInfo.angle = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(holedir, Strikerdir));
        coinInfo.distance = Vector3.Distance(coin.transform.position, hole.transform.position) + Vector3.Distance(coin.transform.position, StrikerPosition.transform.position);
        if (Physics.SphereCast(coin.transform.position, boardProperties.GetCoinRadius(), holedir, out hitInfo))
        {
            GameObject tmpobj = hitInfo.collider.gameObject;
            if (tmpobj.tag == "White" || tmpobj.tag == "Black" || tmpobj.tag == "Red")
            {
                coinInfo.isBlockedH = true;
                coinInfo.blockedCoinAlongHole = tmpobj;

            }
            else
            {
                coinInfo.isBlockedH = false;
                coinInfo.blockedCoinAlongHole = tmpobj;
            }
        }
        if (Physics.SphereCast(StrikerPosition.transform.position, boardProperties.GetStrikerRadius(), Strikerdir, out hitInfo))
        {
            GameObject tmpobj = hitInfo.collider.gameObject;
            if (tmpobj.tag == "White" || tmpobj.tag == "Black" || tmpobj.tag == "Red")
            {
                if (tmpobj.name != coinInfo.Coin.name)
                {
                    coinInfo.isBlockedC = true;
                    coinInfo.blockedCoinAlongStriker = tmpobj;
                }
                else
                {
                    coinInfo.isBlockedC = false;
                    coinInfo.blockedCoinAlongStriker = tmpobj;
                }
            }
            else
            {
                coinInfo.isBlockedC = false;
                coinInfo.blockedCoinAlongStriker = tmpobj;
            }
        }
        this.coinInfoList.Add(coinInfo);

    }
    private void SortCoinsByBlockedParameter()
    {
        for (int j = 0; j <= coinInfoList.Count - 1; j++)
        {
            for (int i = 0; i <= coinInfoList.Count - 2; i++)
            {

                CoinInfo b1 = coinInfoList[i];
                CoinInfo b2 = coinInfoList[i + 1];
                if (b1.isBlockedC && b2.isBlockedC)
                {
                    if (b1.isBlockedH && !b2.isBlockedH)
                    {
                        CopyData(i);
                    }

                }
                else if (b1.isBlockedC && !b2.isBlockedC)
                {
                    if (b1.isBlockedH && b2.isBlockedH)
                    {
                        CopyData(i);
                    }
                    else if (b1.isBlockedH && !b2.isBlockedH)
                    {
                        CopyData(i);
                    }
                    else if (!b1.isBlockedH && !b2.isBlockedH)
                    {
                        CopyData(i);
                    }
                }
                else if (!b1.isBlockedC && !b2.isBlockedC)
                {
                    if (b1.isBlockedH && !b2.isBlockedH)
                    {
                        CopyData(i);
                    }
                }

            }
        }
       
    }

    private void SortCoinsByAngleParameter()
    {
        for (int j = 0; j <= coinInfoList.Count - 1; j++)
        {
            for (int i = 0; i <= coinInfoList.Count - 2; i++)
            {
                CoinInfo b1 = coinInfoList[i];
                CoinInfo b2 = coinInfoList[i + 1];
                if (!b1.isBlockedC && !b1.isBlockedH && !b2.isBlockedC && !b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i);
                    }
                }
                else if (b1.isBlockedC && b1.isBlockedH && b2.isBlockedC && b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i);
                    }
                }
                else if (b1.isBlockedC && !b1.isBlockedH && b2.isBlockedC && !b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i);
                    }
                }
                else if (!b1.isBlockedC && b1.isBlockedH && !b2.isBlockedC && b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i);
                    }
                }
                else if (b1.isBlockedC && !b1.isBlockedH && !b2.isBlockedC && b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i);
                    }
                }
                else if (!b1.isBlockedC && b1.isBlockedH && b2.isBlockedC && !b2.isBlockedH)
                {
                    if (b1.angle > b2.angle)
                    {
                        CopyData(i);
                    }
                }

            }
        }
       
    }
    private void CopyData(int i)
    {
        CoinInfo b = new CoinInfo();
        b = coinInfoList[i];
        coinInfoList[i] = coinInfoList[i + 1];
        coinInfoList[i + 1] = b;
    }
}
