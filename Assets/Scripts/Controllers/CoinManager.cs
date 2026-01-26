using System.Collections.Generic;
using UnityEngine;
using com.VisionXR.ModelClasses;
using com.VisionXR.HelperClasses;

/// <summary>
/// Manages coin creation, destruction, registration, and game events for coins on the board.
/// Handles coin instantiation, removal, and event-driven updates.
/// </summary>
public class CoinManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the coin data scriptable object.
    /// </summary>
    [Header("Scriptable Objects")]
    public CoinDataSO coinData;

    /// <summary>
    /// Reference to the player settings scriptable object.
    /// </summary>
    public UIOutputDataSO uIOutputData;

    /// <summary>
    /// Reference to the board properties scriptable object.
    /// </summary>
    public BoardPropertiesSO boardProperties;

    /// <summary>
    /// Subscribes to coin-related events when enabled.
    /// </summary>
    private void OnEnable()
    {
        coinData.CreateAllCoinsEvent += CreateCoins;
        coinData.DestroyAllCoinsEvent += DestroyCoins;

        coinData.CoinFellInHoleEvent += CoinFellinHole;
        coinData.CoinFellOnGroundEvent += CoinFellOnGround;

        coinData.CreateCoinEvent += CreateCoin;
        coinData.DestroyCoinsFellInThisTurnEvent += DestroyCoinsFellInThisTurn;

        coinData.RegisterCoinEvent += RegisterCoin;
        coinData.DeRegisterCoinEvent += DeRegisterCoin;
    }

    /// <summary>
    /// Unsubscribes from coin-related events when disabled.
    /// </summary>
    private void OnDisable()
    {
        coinData.CreateAllCoinsEvent -= CreateCoins;
        coinData.DestroyAllCoinsEvent -= DestroyCoins;

        coinData.CoinFellInHoleEvent -= CoinFellinHole;
        coinData.CoinFellOnGroundEvent -= CoinFellOnGround;

        coinData.CreateCoinEvent -= CreateCoin;
        coinData.DestroyCoinsFellInThisTurnEvent -= DestroyCoinsFellInThisTurn;

        coinData.RegisterCoinEvent -= RegisterCoin;
        coinData.DeRegisterCoinEvent -= DeRegisterCoin;
    }

    /// <summary>
    /// Registers a coin's Rigidbody in the available coins list if not already present.
    /// </summary>
    /// <param name="coin">The Rigidbody of the coin to register.</param>
    private void RegisterCoin(Rigidbody coin)
    {
        if (coinData.AvailableCoinsInGame.Contains(coin))
        {
            return;
        }
        coinData.AvailableCoinsInGame.Add(coin);
    }

    /// <summary>
    /// Deregisters a coin's Rigidbody from the available coins list if present.
    /// </summary>
    /// <param name="coin">The Rigidbody of the coin to deregister.</param>
    private void DeRegisterCoin(Rigidbody coin)
    {
        if (coinData.AvailableCoinsInGame.Contains(coin))
        {
            coinData.AvailableCoinsInGame.Remove(coin);
        }
    }

    /// <summary>
    /// Creates all coins by instantiating the coin prefab and placing it on the board.
    /// </summary>
    public void CreateCoins()
    {
        DestroyCoins();

        string resourcePath = "NewCoins/Coins" + uIOutputData.MyCoinsId + "/AllCoins";
        GameObject allCoinsPrefab = Resources.Load<GameObject>(resourcePath);

        if (allCoinsPrefab != null)
        {
            coinData.AllCoinsReference = Instantiate(
                allCoinsPrefab,
                boardProperties.GetAllCoinsTransform().position,
                boardProperties.GetAllCoinsTransform().rotation
            );
        }
        else
        {
            Debug.LogError($"AllCoins prefab not found at Resources/{resourcePath}");
            coinData.AllCoinsReference = null;
        }
    }

    /// <summary>
    /// Destroys all the coins in the game by despawning them and clearing the available coins list.
    /// </summary>
    public void DestroyCoins()
    {
        if (coinData.AllCoinsReference != null)
        {
            Destroy(coinData.AllCoinsReference);
            coinData.AvailableCoinsInGame.Clear();
            coinData.AllCoinsReference = null;
        }
    }

    /// <summary>
    /// Handles the event when a coin falls into a hole, updating counts and marking as pocketed.
    /// </summary>
    /// <param name="coin">The coin GameObject that fell into the hole.</param>
    private void CoinFellinHole(GameObject coin)
    {
        if (!coinData.CoinFellInThisTurn.Contains(coin.name))
        {
            coinData.CoinPocketed(coin.name);

            if (coin.tag == "White")
            {
                coinData.Whites++;
                return;
            }
            else if (coin.tag == "Black")
            {
                coinData.Blacks++;
                return;
            }
            else if (coin.tag == "Red")
            {
                coinData.Red++;
                return;
            }
        }
    }

    /// <summary>
    /// Creates a single coin of the specified type and places it on the board.
    /// </summary>
    /// <param name="coin">The type of coin to create.</param>
    public void CreateCoin(PlayerCoin coin)
    {
        GameObject newCoin = null;
        Vector3 newCoinPos = FindCoinPosition();
        string resourcePath = "";

        if (coin == PlayerCoin.White)
        {
            resourcePath = "NewCoins/Coins" + uIOutputData.MyCoinsId + "/WhiteCoin";
            newCoin = Resources.Load<GameObject>(resourcePath);
            if (newCoin != null)
            {
                newCoin = Instantiate(newCoin, newCoinPos, Quaternion.identity);
                newCoin.name = "White" + coinData.WhiteCount;
                coinData.WhiteCount++;
            }
            else
            {
                Debug.LogError($"White coin prefab not found at Resources/{resourcePath}");
            }
        }
        else if (coin == PlayerCoin.Black)
        {
            resourcePath = "NewCoins/Coins" + uIOutputData.MyCoinsId + "/BlackCoin";
            newCoin = Resources.Load<GameObject>(resourcePath);
            if (newCoin != null)
            {
                newCoin = Instantiate(newCoin, newCoinPos, Quaternion.identity);
                newCoin.name = "Black" + coinData.BlackCount;
                coinData.BlackCount++;
            }
            else
            {
                Debug.LogError($"Black coin prefab not found at Resources/{resourcePath}");
            }
        }
        else if (coin == PlayerCoin.Red)
        {
            resourcePath = "NewCoins/Coins" + uIOutputData.MyCoinsId + "/RedCoin";
            newCoin = Resources.Load<GameObject>(resourcePath);
            if (newCoin != null)
            {
                newCoin = Instantiate(newCoin, newCoinPos, Quaternion.identity);
                newCoin.name = "Red" + coinData.RedCount;
                coinData.RedCount++;
            }
            else
            {
                Debug.LogError($"Red coin prefab not found at Resources/{resourcePath}");
            }
        }
    }

    /// <summary>
    /// Handles the event when a coin falls on the ground, resetting its position and rotation.
    /// </summary>
    /// <param name="coin">The coin GameObject that fell on the ground.</param>
    private void CoinFellOnGround(GameObject coin)
    {
        Debug.Log(" coin fell on ground ");
        coin.transform.position = FindCoinPosition();
        coin.transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// Destroys coins that fell in this turn by name and removes them from the available coins list.
    /// </summary>
    /// <param name="coins">List of coin names to destroy.</param>
    private void DestroyCoinsFellInThisTurn(List<string> coins)
    {
        foreach (string name in coins)
        {
            GameObject coin = GameObject.Find(name);
            if (coin != null)
            {
                coinData.AvailableCoinsInGame.Remove(coin.GetComponent<Rigidbody>());
                Destroy(coin);
            }
        }
    }

    /// <summary>
    /// Finds a valid position on the board to place a coin, avoiding overlap with existing coins.
    /// </summary>
    /// <returns>A Vector3 representing the correct position for a new coin.</returns>
    public Vector3 FindCoinPosition()
    {
        bool isThisCorrectPosition = false;
        Vector3 correctPosition = Vector3.zero;
        foreach (Transform fine in boardProperties.GetFinePositions())
        {
            isThisCorrectPosition = true;
            correctPosition = fine.position;
            Collider[] cols = Physics.OverlapSphere(fine.position, boardProperties.GetCoinRadius());
            foreach (Collider c in cols)
            {
                if (c.gameObject.tag == "White" || c.gameObject.tag == "Red" || c.gameObject.tag == "Black")
                {
                    isThisCorrectPosition = false;
                }
            }
            if (isThisCorrectPosition)
            {
                break;
            }
        }
        if (!isThisCorrectPosition)
        {
            correctPosition = boardProperties.GetFinePositions()[0].position + new Vector3(0, 0.1f, 0);
        }
        return correctPosition;
    }
}

