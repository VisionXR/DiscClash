using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.GameElements
{
    public class FreeStyleLogic : MonoBehaviour
    {

        [Header("Scriptable Objects")]
        public GameDataSO data;
        public CoinDataSO coinData;
        public PlayersDataSO playerData;
      
        private GameResult result;


        // Suggested Function Name: ShouldPlayerContinueTurn
        public bool ShouldPlayerContinueTurn(Player currentPlayer, int Whites, int Blacks, int Reds, bool isFoul)
        {
            if (!isFoul)
            {

                return Whites > 0 || Blacks > 0 || Reds > 0;
            }
            return false;
        }

        // Suggested Function Name: DetermineVictory
        public GameResult CheckWinningCondition(Player currentPlayer)
        {
            result = new GameResult();
            if (data.TotalWhites <= 0 && data.TotalBlacks <= 0)
            {
                result.isVictory = true;
                CalculateWinningPlayerOrTeam(currentPlayer);
            }
            return result;
        }

        // Helper Method
        private void CalculateWinningPlayerOrTeam(Player currentPlayer)
        {
            if (playerData.NoOfPlayers() == 2)
            {
                if (data.isRedCovered)
                {
                    result.winningPlayerId = (data.P1Whites + data.P1Blacks + 3 * data.P1Red) > (data.P2Whites + data.P1Blacks + 3 * data.P2Red) ? 1 : 2;

                    if(result.winningPlayerId == 1)
                    {
                        result.winningTeam = Team.TeamA;
                    }
                    else
                    {
                        result.winningTeam = Team.TeamB;
                    }
                }
                else
                {
                    if (currentPlayer.myId == 1)
                    {
                        result.winningPlayerId = 2;
                        result.winningTeam = Team.TeamB;
                    }
                    else
                    {
                        result.winningPlayerId = 1;
                        result.winningTeam = Team.TeamA;
                    }
                }
            }
            else if (playerData.NoOfPlayers() == 4)
            {
                if (data.isRedCovered)
                {
                    int TeamATotal = CalculateTeamATotal();
                    int TeamBTotal = CalculateTeamBTotal();
                    result.winningPlayerId = TeamATotal >= TeamBTotal ? 1 : 3;
                    result.winningTeam = TeamATotal >= TeamBTotal ? Team.TeamA : Team.TeamB;
                }
                else
                {
                    if (currentPlayer.myId == 1 || currentPlayer.myId == 2)
                    {
                        result.winningPlayerId = 3;
                        result.winningTeam = Team.TeamB;
                    }
                    else
                    {
                        result.winningPlayerId = 1;
                        result.winningTeam = Team.TeamA;
                    }
                }
            }
        }

        private int CalculateTeamATotal()
        {
            return data.P1Whites + data.P1Blacks + 3 * data.P1Red + data.P2Whites + data.P2Blacks + 3 * data.P2Red;
        }

        private int CalculateTeamBTotal()
        {
            return data.P3Whites + data.P3Blacks + 3 * data.P3Red + data.P4Whites + data.P4Blacks + 3 * data.P4Red;
        }
       
    }
}
