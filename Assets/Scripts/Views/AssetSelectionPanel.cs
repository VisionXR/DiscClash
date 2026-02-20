using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class AssetSelectionPanel : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public MyPlayerSettings playerSettings;
        public StrikerDataSO strikerData;
        public CoinDataSO coinData;
        public UIOutputDataSO uiOutputData;
        public UIInputDataSO uiInputData;

        // local variables
        public Destination destination;

        private void OnEnable()
        {
            CreateStriker();
        }

        private void OnDisable()
        {
            DestroyStriker();
        }

        private void CreateStriker()
        {
            strikerData.CreateStriker(1, playerSettings.MyStrikerId, null);
            coinData.CreateAllCoins(1);
        }

        private void DestroyStriker()
        {
            strikerData.DestroyStriker(1);
            coinData.DestroyAllCoins();
        }

        public void BoardBtnClicked()
        {

        }

        public void StrikerBtnClicked()
        {

        }

        public void CoinsBtnClciked()
        {

        }

        public void NextBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            destination.gameMode = uiOutputData.gameMode;
            destination.gameType = uiOutputData.gameType;
            destination.challenge = uiOutputData.challenge;

            uiInputData.ShowDestination(destination);
            gameObject.SetActive(false);
        }
    }
}

