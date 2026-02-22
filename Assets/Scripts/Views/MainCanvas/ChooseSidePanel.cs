using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.UI;

public class ChooseSidePanel : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public PlayersDataSO playersData;
    public UIOutputDataSO uIOutputData;
    public UIInputDataSO uIInputData;
    public AppDataSO appData;

    [Header("Panels")]
    public GameObject LobbyPanel;

    [Header("Selection Images")]
    public Image WhiteCoinBg;
    public Image BlackCoinBg;


    private void OnEnable()
    {
        ResetImages();
        WhiteCoinBtnClicked();
    }

    public void WhiteCoinBtnClicked()
    {
      

        if(uIOutputData.gameMode == GameMode.PvsAI || uIOutputData.gameMode  == GameMode.P1vsP2)
        {
            SetCoin(1, PlayerCoin.White);
            SetCoin(2, PlayerCoin.Black);
        }
        else
        {
            SetCoin(1, PlayerCoin.White);
            SetCoin(2, PlayerCoin.White);
            SetCoin(3, PlayerCoin.Black);
            SetCoin(4, PlayerCoin.Black);
        }

        ResetImages();
        WhiteCoinBg.color = appData.SelectedColor;
      
    }

    public void BlackCoinBtnClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        if (uIOutputData.gameMode == GameMode.PvsAI || uIOutputData.gameMode == GameMode.P1vsP2)
        {
            SetCoin(1, PlayerCoin.Black);
            SetCoin(2, PlayerCoin.White);
        }
        else
        {
            SetCoin(1, PlayerCoin.Black);
            SetCoin(2, PlayerCoin.Black);
            SetCoin(3, PlayerCoin.White);
            SetCoin(4, PlayerCoin.White);
        }

        ResetImages();
        BlackCoinBg.color = appData.SelectedColor;
    }


    public void StartGameBtnClicked()
    {
  
        uIInputData.StartGame();
        LobbyPanel.SetActive(false);
    }



    public void SetCoin(int id,PlayerCoin playerCoin)
    {
        Player p = playersData.GetPlayer(id);
        p.myCoin = playerCoin;

    }

    private void ResetImages()
    {
        WhiteCoinBg.color = appData.IdleColor;
        BlackCoinBg.color = appData.IdleColor;
    }
}
