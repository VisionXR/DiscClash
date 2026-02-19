using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class LobbyPanelView : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public UIOutputDataSO uiOutPutData;
        public DestinationDataSO destinationData;
        public PlayersDataSO playersData;
        public CloudDataSO cloudData;


        [Header(" Panels ")]
        public WaitingPanel BetPanel2Players;
        public WaitingPanel BetPanel4Players;
        public ChooseSidePanel ChooseSidePanel;

        // local variables
        private Coroutine playerRoutine;

        private void OnEnable()
        {
            BetPanel2Players.gameObject.SetActive(false);
            BetPanel4Players.gameObject.SetActive(false);
            ChooseSidePanel.gameObject.SetActive(false);

            Show();

            if(playerRoutine == null)
            {
                playerRoutine = StartCoroutine(WaitAndSetPlayers());
            }
        }

        private void OnDisable()
        {
            Stop();
        }

        private void Stop()
        {
            if (playerRoutine != null)
            {
                StopCoroutine(playerRoutine);
                playerRoutine = null;
            }
        }


        private void Show()
        {
            if(uiOutPutData.gameMode == GameMode.PvsAI || uiOutPutData.gameMode == GameMode.P1vsP2)
            {
                BetPanel2Players.gameObject.SetActive(true);
            }
            else
            {
                BetPanel4Players.gameObject.SetActive(true);
            }
        }

        private IEnumerator WaitAndSetPlayers()
        {
            while(true)
            {
              
                foreach(Player p in playersData.CurrentPlayers)
                {
                    if (uiOutPutData.gameMode == GameMode.PvsAI || uiOutPutData.gameMode == GameMode.P1vsP2)
                    {
                      

                        BetPanel2Players.SetImage(p.myId, p.GetMyImage());
                        BetPanel2Players.SetName(p.myId, p.myName);
                        BetPanel2Players.SetStatus(p.myId, "Joined");

                      

                    }
                    else
                    {
                      

                        BetPanel4Players.SetImage(p.myId, p.GetMyImage());
                        BetPanel4Players.SetName(p.myId, p.myName);
                        BetPanel4Players.SetStatus(p.myId, "Joined");


                    }
                }

                if (uiOutPutData.gameMode == GameMode.PvsAI || uiOutPutData.gameMode == GameMode.P1vsP2)
                {
                    if(playersData.NoOfPlayers() == 2)
                    {
                        BetPanel2Players.Deductfee();
                        Stop();
                    }
                }
                else
                {
                    if (playersData.NoOfPlayers() == 4)
                    {
                        BetPanel4Players.Deductfee();
                        Stop();
                    }
                }

                 yield return new WaitForSeconds(3);
            }
        }

    }
}
