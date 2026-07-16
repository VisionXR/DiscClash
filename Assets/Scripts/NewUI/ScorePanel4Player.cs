using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.VisionXR.Views
{
    public class ScorePanel4Player : MonoBehaviour
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
        public PlayerDetailsView leftPlayer1;
        public PlayerDetailsView leftPlayer2;
        public PlayerDetailsView rightPlayer1;
        public PlayerDetailsView rightPlayer2;
        public GameResultPanelView gameResultPanelView;
        public string gameResultState;
        public string pauseState;


        [Header(" UI Elements")]
        public Image camViewImage;
        public Sprite FrontViewSprite;
        public Sprite TopViewSprite;

        [Header("Off Panels")]
        public List<PanelOnOff> panelsToOff;
        public PanelOnOff bottomRightPanel;
        public PanelOnOff bottomLeftPanel;

        // local variables
        public float blinkTime = 0.2f;
        private Coroutine turnIndicatorCoroutine;

        public void TurnOn()
        {
            foreach (var item in panelsToOff)
            {
                item.TurnOnPanel();
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
            leftPlayer1.SetScore(gameData.TeamAScore);
            leftPlayer2.SetScore(gameData.TeamAScore);
            rightPlayer1.SetScore(gameData.TeamBScore);
            rightPlayer2.SetScore(gameData.TeamBScore);
        }

        public void ShowImages()
        {
            Player p1 = playerData.GetPlayer(1);
            Player p2 = playerData.GetPlayer(2);

            if (p1 != null)
            {
                leftPlayer1.SetPlayerImage(p1.GetMyImage());
                leftPlayer1.SetPlayerName(p1.myName);
                SetCoins(p1.myId);
            }

            if (p2 != null)
            {
                leftPlayer2.SetPlayerImage(p2.GetMyImage());
                leftPlayer2.SetPlayerName(p2.myName);
                SetCoins(p2.myId);
            }

            Player p3 = playerData.GetPlayer(3);
            Player p4 = playerData.GetPlayer(4);

            if (p3 != null)
            {
                rightPlayer1.SetPlayerImage(p3.GetMyImage());
                rightPlayer1.SetPlayerName(p3.myName);
                SetCoins(p3.myId);
            }

            if (p4 != null)
            {
                rightPlayer2.SetPlayerImage(p4.GetMyImage());
                rightPlayer2.SetPlayerName(p4.myName);
                SetCoins(p4.myId);
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
                leftPlayer1.SetPlayerName(p.myName);
                leftPlayer1.SetPlayerImage(p.GetMyImage());
            }
            else if (p.myId == 2)
            {
                leftPlayer2.SetPlayerName(p.myName);
                leftPlayer2.SetPlayerImage(p.GetMyImage());
            }
            else if (p.myId == 3)
            {
                rightPlayer1.SetPlayerName(p.myName);
                rightPlayer1.SetPlayerImage(p.GetMyImage());

            }
            else if (p.myId == 4)
            {
                rightPlayer2.SetPlayerName(p.myName);
                rightPlayer2.SetPlayerImage(p.GetMyImage());

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
        }


        private IEnumerator TurnIndicator(int id)
        {
            while (true)
            {
                if (id == 1)
                {
                    leftPlayer1.SetPlayerTurnIndicator(true);
                    yield return new WaitForSeconds(blinkTime);
                    leftPlayer1.SetPlayerTurnIndicator(false);
                    yield return new WaitForSeconds(blinkTime);
                }
                else if (id == 2)
                {
                    leftPlayer2.SetPlayerTurnIndicator(true);
                    yield return new WaitForSeconds(blinkTime);
                    leftPlayer2.SetPlayerTurnIndicator(false);
                    yield return new WaitForSeconds(blinkTime);
                }
                else if (id == 3)
                {
                    rightPlayer1.SetPlayerTurnIndicator(true);
                    yield return new WaitForSeconds(blinkTime);
                    rightPlayer1.SetPlayerTurnIndicator(false);
                    yield return new WaitForSeconds(blinkTime);
                }
                else if (id == 4)
                {
                    rightPlayer2.SetPlayerTurnIndicator(true);
                    yield return new WaitForSeconds(blinkTime);
                    rightPlayer2.SetPlayerTurnIndicator(false);
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
                        leftPlayer1.SetCoinImage(uiOutputData.WhiteCoin);
                        leftPlayer1.SetRedImage(uiOutputData.RedCoin);
                    }
                    else
                    {
                        leftPlayer1.SetCoinImage(uiOutputData.BlackCoin);
                        leftPlayer1.SetRedImage(uiOutputData.RedCoin);
                    }
                }
                else if (uiOutputData.challenge == Challenge.FreeStyle)
                {
                    leftPlayer1.SetCoinImage(uiOutputData.BlackAndWhiteCoin);
                    leftPlayer1.SetRedImage(uiOutputData.RedCoin);
                }
            }
            else if (id == 2)
            {
                if (uiOutputData.challenge == Challenge.BlackAndWhite)
                {
                    if (p.myCoin == PlayerCoin.White)
                    {
                        leftPlayer2.SetCoinImage(uiOutputData.WhiteCoin);
                        leftPlayer2.SetRedImage(uiOutputData.RedCoin);
                    }
                    else
                    {
                        leftPlayer2.SetCoinImage(uiOutputData.BlackCoin);
                        leftPlayer2.SetRedImage(uiOutputData.RedCoin);
                    }
                }
                else if (uiOutputData.challenge == Challenge.FreeStyle)
                {
                    leftPlayer2.SetCoinImage(uiOutputData.BlackAndWhiteCoin);
                    leftPlayer2.SetRedImage(uiOutputData.RedCoin);
                }
            }
            else if (id == 3)
            {
                if (uiOutputData.challenge == Challenge.BlackAndWhite)
                {
                    if (p.myCoin == PlayerCoin.White)
                    {
                        rightPlayer1.SetCoinImage(uiOutputData.WhiteCoin);
                        rightPlayer1.SetRedImage(uiOutputData.RedCoin);
                    }
                    else
                    {
                        rightPlayer1.SetCoinImage(uiOutputData.BlackCoin);
                        rightPlayer1.SetRedImage(uiOutputData.RedCoin);
                    }
                }
                else if (uiOutputData.challenge == Challenge.FreeStyle)
                {
                    rightPlayer1.SetCoinImage(uiOutputData.BlackAndWhiteCoin);
                    rightPlayer1.SetRedImage(uiOutputData.RedCoin);
                }
            }
            else if (id == 4)
            {
                if (uiOutputData.challenge == Challenge.BlackAndWhite)
                {
                    if (p.myCoin == PlayerCoin.White)
                    {
                        rightPlayer2.SetCoinImage(uiOutputData.WhiteCoin);
                        rightPlayer2.SetRedImage(uiOutputData.RedCoin);
                    }
                    else
                    {
                        rightPlayer2.SetCoinImage(uiOutputData.BlackCoin);
                        rightPlayer2.SetRedImage(uiOutputData.RedCoin);
                    }
                }
                else if (uiOutputData.challenge == Challenge.FreeStyle)
                {
                    rightPlayer2.SetCoinImage(uiOutputData.BlackAndWhiteCoin);
                    rightPlayer2.SetRedImage(uiOutputData.RedCoin);
                }
            }

        }

        private void ResetImages()
        {
            leftPlayer1.SetPlayerImage(null);
            leftPlayer2.SetPlayerImage(null);
            rightPlayer1.SetPlayerImage(null);
            rightPlayer2.SetPlayerImage(null);
        }
        private void Reset(int id)
        {
           
            leftPlayer1.SetScore(0);
            leftPlayer2.SetScore(0);
            rightPlayer1.SetScore(0);
            rightPlayer2.SetScore(0);
        }
    }
}


