using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.GameElements
{
    public class AIPlayer : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public GameDataSO data;
        public UIOutputDataSO uiOutputData;  

        [Header("Game Objects")]
        public Player currentPlayer;


        public void AIShouldPlay()
        {
            if (uiOutputData.game == Game.BlackAndWhite)
            {
                PlayBlackAndWhite();
            }
            else
            {
                PlayFreeStyle();
            }

        }
        private void PlayBlackAndWhite()
        {
            
            if ((currentPlayer.myCoin == PlayerCoin.White && data.TotalWhites < 3) || (currentPlayer.myCoin == PlayerCoin.Black && data.TotalBlacks < 3))
            {
                if (data.isRedCovered || data.ShouldICoverCoin)
                {
                     currentPlayer.myAvatar.GetComponent<IAIBehaviour>().ExecuteShot(currentPlayer.myCoin);
                }
                else
                {
                     currentPlayer.myAvatar.GetComponent<IAIBehaviour>().ExecuteShot(PlayerCoin.Red);
                }
            }
            else if ((currentPlayer.myCoin == PlayerCoin.White && data.TotalWhites >= 3) || (currentPlayer.myCoin == PlayerCoin.Black && data.TotalBlacks >= 3))
            {
              
                   currentPlayer.myAvatar.GetComponent<IAIBehaviour>().ExecuteShot(currentPlayer.myCoin);
            }
        }
        private void PlayFreeStyle()
        {
           
            if ((data.TotalWhites + data.TotalBlacks) >= 4 || data.ShouldICoverCoin || data.isRedCovered)
            {
                 currentPlayer.myAvatar.GetComponent<IAIBehaviour>().ExecuteShot(PlayerCoin.All);
            }
            else if ((data.TotalWhites + data.TotalBlacks) <= 3 && !data.isRedCovered)
            {
                 currentPlayer.myAvatar.GetComponent<IAIBehaviour>().ExecuteShot(PlayerCoin.Red);
            }
        }
    }
}
        