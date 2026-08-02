using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.VisionXR.Views
{

    public class ScorePanel2Player : MonoBehaviour
    {
        [Header(" scriptable objects")]
        public UIOutputDataSO uiOutputData;
        public UIInputDataSO uiInputData;
        public GameDataSO gameData;
        public PlayersDataSO playerData;
        public UIDataSO uiData;
        public CamPositionSO camPositionData;
        public MyPlayerSettings myPlayerSettings;

        [Header(" player objects")]
        public PlayerDetailsView leftPlayer;
        public PlayerDetailsView rightPlayer;
        public GameResultPanelView gameResultPanelView;
        public string gameResultState;
        public string pauseState;


        [Header(" UI Elements")]
        public Image camViewImage;
        public Sprite FrontViewSprite;
        public Sprite TopViewSprite;

        [Header("Off Panels")]
        public List<PanelOnOff> panelsToOff;
        public List<PanelOnOff> voicePanelsToOff;
        public PanelOnOff bottomRightPanel;
        public PanelOnOff bottomLeftPanel;

        // local variables
        [Header("This Objects")]
        public GameObject Player1ScorePanel;
        public GameObject Player2ScorePanel;
        public float scaleFactor = 1.1f;
        public float blinkTime = 0.2f;
        private Coroutine turnIndicatorCoroutine;


        public void TurnOn()
        {
            foreach (var item in panelsToOff)
            {
                item.TurnOnPanel();
            }

            if(uiOutputData.gameType == GameType.PlayWithFriends)
            {
                foreach (var item in voicePanelsToOff)
                {
                    item.TurnOnPanel();
                }
            }

            if (myPlayerSettings.myDominantHand == DominantHand.Right)
            {
                bottomLeftPanel.TurnOnPanel();
            }
            else
            {
                bottomRightPanel.TurnOnPanel();
            }
        }

        public void TurnOff()
        {
            foreach (var item in panelsToOff)
            {
                item.TurnOffPanel();
            }

            if (uiOutputData.gameType == GameType.PlayWithFriends)
            {
                foreach (var item in voicePanelsToOff)
                {
                    item.TurnOffPanel();
                }
            }

            if (myPlayerSettings.myDominantHand == DominantHand.Right)
            {
                bottomLeftPanel.TurnOffPanel();
            }
            else
            {
                bottomRightPanel.TurnOffPanel();
            }
        }

        private void OnEnable()
        {
            ShowImages();
            ShowScore();

            gameData.TurnChangedEvent += TurnChanged;

            uiInputData.ShowGameResultEvent += ShowGameResult;

            uiInputData.PlayAgainEvent += Reset;


            uiInputData.ShowPlayerDetailsEvent += ShowPlayerDetails;


            playerData.PlayerStrikeStartedEvent += PlayerStrikeStarted;
            playerData.PlayerImageLoadedEvent += ShowImages;
        }

        private void OnDisable()
        {

            gameData.TurnChangedEvent -= TurnChanged;

            uiInputData.ShowGameResultEvent -= ShowGameResult;

            uiInputData.PlayAgainEvent -= Reset;


            uiInputData.ShowPlayerDetailsEvent -= ShowPlayerDetails;

            playerData.PlayerStrikeStartedEvent -= PlayerStrikeStarted;
            playerData.PlayerImageLoadedEvent -= ShowImages;
        }

        public void ShowScore()
        {
            leftPlayer.SetScore(gameData.P1Score);
            rightPlayer.SetScore(gameData.P2Score);
        }

        public void ShowImages()
        {
            Player p1 = playerData.GetPlayer(1);
            Player p2 = playerData.GetPlayer(2);

            if (p1 != null)
            {
                leftPlayer.SetPlayerImage(p1.GetMyImage());
                leftPlayer.SetPlayerName(p1.myName);
                SetCoins(1);
            }

            if (p2 != null)
            {
                rightPlayer.SetPlayerImage(p2.GetMyImage());
                rightPlayer.SetPlayerName(p2.myName);
                SetCoins(2);
            }
        }

        private void PlayerStrikeStarted(int id, float arg2)
        {

        }
        private void ShowGameResult(GameResult result)
        {

            TurnChanged(result.winningPlayerId);
            uiData.uiManager.ShowCanvas(0);
            uiData.uiManager.ChangeState(gameResultState, true);
            gameResultPanelView.ShowResult(result);
     
            gameObject.SetActive(false);
        }

        public void ShowPlayerDetails(Player p)
        {
            if (p.myId == 1)
            {
                leftPlayer.SetPlayerName(p.myName);
                leftPlayer.SetPlayerImage(p.GetMyImage());
            }
            else
            {


                rightPlayer.SetPlayerName(p.myName);
                rightPlayer.SetPlayerImage(p.GetMyImage());

            }

            SetCoins(p.myId);
            Reset(1);

        }
        private void TurnChanged(int id)
        {
            if (turnIndicatorCoroutine != null)
            {
                StopCoroutine(turnIndicatorCoroutine);
                turnIndicatorCoroutine = null;
            }

            turnIndicatorCoroutine = StartCoroutine(TurnIndicator(id));

            if (id == 1)
            {
                Player1ScorePanel.transform.localScale = Vector3.one * scaleFactor;
                Player2ScorePanel.transform.localScale = Vector3.one;

            }
            else
            {
                Player1ScorePanel.transform.localScale = Vector3.one;
                Player2ScorePanel.transform.localScale = Vector3.one * scaleFactor;
            }
        }

        private IEnumerator TurnIndicator(int id)
        {
            ResetIndicators();

            while (true)
            {
                if (id == 1)
                {
                    leftPlayer.SetPlayerTurnIndicator(true);
                    yield return new WaitForSeconds(blinkTime);
                    leftPlayer.SetPlayerTurnIndicator(false);
                    yield return new WaitForSeconds(blinkTime);
                }
                else
                {
                    rightPlayer.SetPlayerTurnIndicator(true);
                    yield return new WaitForSeconds(blinkTime);
                    rightPlayer.SetPlayerTurnIndicator(false);
                    yield return new WaitForSeconds(blinkTime);
                }


            }
        }

        public void PauseButtonClicked()
        {
        
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(pauseState, true);
            uiInputData.PauseGame();
        }

        public void CameraBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            Player p = playerData.GetMainPlayer();
            if (camViewImage.sprite == FrontViewSprite)
            {
                camViewImage.sprite = TopViewSprite;
                camPositionData.SetCamPositionTopView(p.myId);
            }
            else
            {
                camViewImage.sprite = FrontViewSprite;
                camPositionData.SetCamPositionFrontView(p.myId);
            }
        }


        public void SetCoins(int id)
        {
            Player p = playerData.GetPlayer(id);
            if (p == null)
            {
                Debug.Log("No player found with id: " + id);
                return;
            }

            if (id == 1)
            {
                if (uiOutputData.challenge == Challenge.BlackAndWhite)
                {
                    if (p.myCoin == PlayerCoin.White)
                    {
                        leftPlayer.SetCoinImage(uiOutputData.WhiteCoin);
                        leftPlayer.SetRedImage(uiOutputData.RedCoin);
                    }
                    else
                    {
                        leftPlayer.SetCoinImage(uiOutputData.BlackCoin);
                        leftPlayer.SetRedImage(uiOutputData.RedCoin);
                    }
                }
                else if (uiOutputData.challenge == Challenge.FreeStyle)
                {
                    leftPlayer.SetCoinImage(uiOutputData.BlackAndWhiteCoin);
                    leftPlayer.SetRedImage(uiOutputData.RedCoin);
                }
            }
            else
            {
                if (uiOutputData.challenge == Challenge.BlackAndWhite)
                {
                    if (p.myCoin == PlayerCoin.White)
                    {
                        rightPlayer.SetCoinImage(uiOutputData.WhiteCoin);
                        rightPlayer.SetRedImage(uiOutputData.RedCoin);
                    }
                    else
                    {
                        rightPlayer.SetCoinImage(uiOutputData.BlackCoin);
                        rightPlayer.SetRedImage(uiOutputData.RedCoin);
                    }
                }
                else if (uiOutputData.challenge == Challenge.FreeStyle)
                {
                    rightPlayer.SetCoinImage(uiOutputData.BlackAndWhiteCoin);
                    rightPlayer.SetRedImage(uiOutputData.RedCoin);
                }
            }

        }

        private void ResetIndicators()
        {
            leftPlayer.SetPlayerTurnIndicator(false);
            rightPlayer.SetPlayerTurnIndicator(false);
        }
        private void Reset(int id)
        {
            leftPlayer.SetScore(0);
            rightPlayer.SetScore(0);
        }
    }
}


                                                                