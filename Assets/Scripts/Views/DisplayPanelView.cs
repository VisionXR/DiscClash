using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using TMPro;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class DisplayPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        [SerializeField] private GameDataSO gameData;
        [SerializeField] private CoinDataSO coinData;
        [SerializeField] private PlayersDataSO playerData;
        [SerializeField] private UIOutputDataSO uiOutputData;
        [SerializeField] private MyPlayerSettings playerSettings;

        [Header("Text Objects")]
        [SerializeField] private GameObject turnPanel;
        [SerializeField] private GameObject foulPanel;
        [SerializeField] private TMP_Text turnText;
        [SerializeField] private TMP_Text foulText;


        public bool isShown = false;
        private void OnEnable()
        {
            gameData.TurnChangedEvent += OnTurnChanged;
            coinData.CreateCoinEvent += CheckForFoul;
            uiOutputData.ExitGameEvent += ResetDisplay;
            uiOutputData.HomeEvent += ResetDisplay;
            coinData.ShowFoulEvent += ShowFoul;
        }

        private void OnDisable()
        {
            gameData.TurnChangedEvent -= OnTurnChanged;
            coinData.CreateCoinEvent -= CheckForFoul;
            uiOutputData.ExitGameEvent -= ResetDisplay;
            uiOutputData.HomeEvent -= ResetDisplay;
            coinData.ShowFoulEvent -= ShowFoul;
        }

        private void CheckForFoul(PlayerCoin coin)
        {
            if (coin == PlayerCoin.White || coin == PlayerCoin.Black)
            {
                FoulOccurred();
            }
            else if (coin == PlayerCoin.Red)
            {
                RedFoulOccurred();
            }
        }


        private void ShowFoul()
        {
            StartCoroutine(ShowFoul("Pocketing Striker is Foul", true));
            AudioManager.instance.PlayFoulSound();
        }
        private void FoulOccurred()
        {
            StartCoroutine(ShowFoul("Pocketing Striker is Foul", false));
            AudioManager.instance.PlayFoulSound();
            
        }
        private void RedFoulOccurred()
        {
            StartCoroutine(ShowFoul(" Failed to cover Queen ", true));
            AudioManager.instance.PlayFoulSound();
                   
        }
        private void OnTurnChanged(int turnId)
        {
            AudioManager.instance.PlayTurnChangedSound();
            Player currentPlayer = playerData.GetPlayer(turnId);
            Player mainPlayer = playerData.GetMainPlayer();
            if (mainPlayer.myId == currentPlayer.myId)
            {
               StartCoroutine(ShowPlayerName("Your Turn"));
            }
            else
            {
                StartCoroutine(ShowPlayerName(currentPlayer.myName + "'s Turn"));
            }
            CheckForRed(turnId);
        }

        private void CheckForRed(int turnId)
        {
           
            if (!gameData.isRedCovered && gameData.ShouldICoverCoin)
            {

                StartCoroutine(ShowFoul("Cover Queen",false));
            }
            else if (gameData.isRedCovered && !gameData.ShouldICoverCoin && !isShown)
            {
                StartCoroutine(ShowFoul("Queen Covered ",true));
                AudioManager.instance.PlayRedCoveredSound();
                isShown = true;
            }

        }
        private IEnumerator ShowFoul(string name ,bool isAll)
        {

         
            if (isAll)
            {
                foulPanel.SetActive(true);
                foulText.text = name;
                yield return new WaitForSeconds(2);
                foulText.text = "";
                foulPanel.SetActive(false);
            }
            else
            {
                Player mainPlayer = playerData.GetMainPlayer();
                Player currentPlayer = playerData.GetPlayer(gameData.currentTurnId);
                if (mainPlayer.myId == currentPlayer.myId)
                {

                    foulText.text = name;
                    foulPanel.SetActive(true);              
                    yield return new WaitForSeconds(2);
                    foulText.text = "";
                    foulPanel.SetActive(false);
                }
            }
        }
        private IEnumerator ShowPlayerName(string name = "Your Turn")
        {
            turnPanel.SetActive(true);
            turnText.text = name;
            yield return new WaitForSeconds(2);
            turnText.text = "";
            turnPanel.SetActive(false);
        }

        // write a reset function and clear all the text and panels
        private void ResetDisplay()
        {
      
            foulText.text = "";
            turnText.text = "";
            foulPanel.SetActive(false);
            turnPanel.SetActive(false);
            isShown = false;
        }
    }
}
