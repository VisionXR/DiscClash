using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;


namespace com.VisionXR.GameElements
{
    public class BlackAndWhiteLogic : MonoBehaviour
    {

        [Header("Scriptable Objects")]
        public GameDataSO data;
        public CoinDataSO coinData;
        public PlayersDataSO playerData;

        /// <summary>
        /// Determines if the currentPlayer continues their turn based on their move.
        /// </summary>
        /// <returns>True if the player continues their turn, otherwise false.</returns>
        public bool ShouldPlayerContinueTurn(Player currentPlayer, int Whites, int Blacks, int Reds, bool isFoul)
        {
          

            if (currentPlayer.myCoin == PlayerCoin.White)
            {
                return ShouldWhiteCoinPlayerContinue(Whites, Reds,isFoul);
            }
            else
            {
                return ShouldBlackCoinPlayerContinue(Blacks, Reds, isFoul);
            }
        }


 

        /// <summary>
        /// Determines if the white coin player should continue their turn.
        /// </summary>
        /// <returns>True if the player continues, otherwise false.</returns>
        private bool ShouldWhiteCoinPlayerContinue(int Whites, int Reds,bool isFoul)
        {
            if (!isFoul)
            {
                // Player pots at least one white coin or the red coin.
                if (Whites > 0 || Reds > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if the black coin player should continue their turn.
        /// </summary>
        /// <returns>True if the player continues, otherwise false.</returns>
        private bool ShouldBlackCoinPlayerContinue(int Blacks, int Reds,bool isFoul)
        {
            if (!isFoul)
            {
                // Player pots at least one black coin or the red coin.
                if (Blacks > 0 || Reds > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public GameResult CheckWinningCondition(Player currentPlayer)
        {
            GameResult result = new GameResult();
            PlayerCoin currentPlayerCoin = currentPlayer.myCoin;
            bool is4PlayerGame = playerData.NoOfPlayers() == 4;

            if (data.TotalWhites <= 0 || data.TotalBlacks <= 0)
            {

                result.isVictory = true; // Someone has won, we'll determine who next

                if (currentPlayerCoin == PlayerCoin.White && data.TotalWhites <= 0)
                {
                    if (data.isRedCovered)
                    {
                        result.winningPlayerId = currentPlayer.myId;
                    }
                    else
                    {
                        result.winningPlayerId = playerData.GetPlayer(PlayerCoin.Black).myId;
                    }
                }
                else if (currentPlayerCoin == PlayerCoin.White && data.TotalBlacks <= 0)
                {

                    result.winningPlayerId = playerData.GetPlayer(PlayerCoin.Black).myId;

                }
                else if (currentPlayerCoin == PlayerCoin.Black && data.TotalBlacks <= 0)
                {
                    if (data.isRedCovered)
                    {
                        result.winningPlayerId = currentPlayer.myId;
                    }
                    else
                    {
                        result.winningPlayerId = playerData.GetPlayer(PlayerCoin.White).myId;
                    }
                }
                else if (currentPlayerCoin == PlayerCoin.Black && data.TotalWhites <= 0)
                {

                    result.winningPlayerId = playerData.GetPlayer(PlayerCoin.White).myId;

                }
                else // Current player did not win
                {
                    result.isVictory = false;
                    return result;
                }

        
                 result.winningTeam = playerData.GetPlayer(result.winningPlayerId).myTeam;
                
            }
            else
            {
                result.isVictory = false;
            }

            return result;
        }

     
    }
}


