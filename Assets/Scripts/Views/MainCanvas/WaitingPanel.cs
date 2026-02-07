using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.VisionXR.Views
{
    public class WaitingPanel : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public PlayersDataSO playerData;
        public UIOutputDataSO uiOutputData;
        public NetworkOutputSO networkOutputData;
        public DataManager dataManager;

        [Header("UI Elements")]
        public List<TMP_Text> playerNames;
        public List<TMP_Text> playerStatuses;
        public List<Image> playerImages;
        public GameObject StartButton;


        // Keeps track of which status coroutine is running for which player ID
        private Dictionary<int, Coroutine> statusCoroutines = new Dictionary<int, Coroutine>();

        private void OnEnable()
        {

            StartButton.SetActive(false);
            // When the panel opens, start the "Connecting..." animation for everyone
            for (int i = 0; i < playerStatuses.Count; i++)
            {
                int playerId = i + 1;
                StartConnectingAnimation(playerId);
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            statusCoroutines.Clear();
        }

        public void SetName(int id, string name)
        {
            Debug.Log($"Setting name for player {id}: {name}");
            // Note: Changed .name to .text so it actually shows on the UI!
            playerNames[id - 1].text = name;


            if(uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2)
            {
                if (playerData.NoOfPlayers() == 2 && networkOutputData.isHost)
                {
                    StartButton.SetActive(true);
                }
            }
            else
            {
                if (playerData.NoOfPlayers() == 4 && networkOutputData.isHost)
                {
                    StartButton.SetActive(true);
                }
            }
           
        }

        public void SetStatus(int id, string status)
        {
            // 1. Stop the "Connecting..." animation if it's running
            if (statusCoroutines.ContainsKey(id))
            {
                StopCoroutine(statusCoroutines[id]);
                statusCoroutines.Remove(id);
            }

            // 2. Set the final static text
            playerStatuses[id - 1].text = status;
        }

        public void SetImage(int id, Sprite image)
        {
            playerImages[id - 1].sprite = image;
        }

        public void StartGameButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            dataManager.StartGame(1);
           
        }

        private void StartConnectingAnimation(int id)
        {
            // Safety: Stop existing one if called twice
            if (statusCoroutines.ContainsKey(id))
            {
                StopCoroutine(statusCoroutines[id]);
            }

            statusCoroutines[id] = StartCoroutine(AnimateConnectingText(id));
        }

        private IEnumerator AnimateConnectingText(int id)
        {
            TMP_Text targetText = playerStatuses[id - 1];
            string baseText = "Waiting";
            int dotCount = 0;

            while (true)
            {
                dotCount = (dotCount + 1) % 4; // Cycles 0, 1, 2, 3
                targetText.text = baseText + new string('.', dotCount);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}