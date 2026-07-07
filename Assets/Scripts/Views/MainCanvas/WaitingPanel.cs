using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
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
        public PlayersDataSO playersData;
        public UIOutputDataSO uiOutputData;
        public UIInputDataSO uiInputData;
        public CloudDataSO cloudData;

        [Header("UI Elements")]
        public GameObject ChooseSidePanel;

        [Header("UI Elements")]
        public TMP_Text gameModeText;
        public List<TMP_Text> playerCoins;
        public List<TMP_Text> playerNames;
        public List<Image> playerImages;


        [Header("UI Elements")]
        public TMP_Text totalCoinsText;

        // Actions
        public Action OnEntryFeesDeductedSuccess;
        public Action OnEntryFeesDeductedFailure;


        // Entry fee animation coroutine reference
        private Coroutine entryFeeCoroutine;

        private void OnEnable()
        {
            gameModeText.text = Enum.GetName(typeof(GameMode), uiOutputData.gameMode);
            

        }


        public void SetName(int id, string name)
        {
            Debug.Log($"Setting name for player {id}: {name}");
            // Note: Changed .name to .text so it actually shows on the UI!
            playerNames[id - 1].text = name;

        }

        public void SetImage(int id, Sprite image)
        {
            playerImages[id - 1].sprite = image;
        }

        public void StartGameButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            if (uiOutputData.challenge == Challenge.BlackAndWhite)
            {
                ChooseSidePanel.SetActive(true);
               
            }
            else
            {
                uiInputData.StartGame();
            }

            gameObject.SetActive(false);
        }

        public void InviteBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();

            // 2. Prepare the invite data
            // Example: discclash://1v1/Classic/India/Room_123
            string gameMode = "1v1";
            string gameType = "Classic";
            string region = "India";
            string roomName = "Room_" + UnityEngine.Random.Range(100, 999); // Use real room ID here

            //    string inviteLink = $"discclash://{gameMode}/{gameType}/{region}/{roomName}";
            string inviteLink = "https://www.visionxr.co.in/";
            string shareMessage = "Hey! Join me for a game of Disc Clash (Carrom). Tap the link to play: " + inviteLink;

            // 3. Trigger the Native Share Popup
            new NativeShare()
                .SetText(shareMessage)
                .SetSubject("Disc Clash Invite") // Used for Email/SMS subjects
                .SetCallback((result, shareTarget) =>
                {
                    Debug.Log($"Disc Clash: Share result: {result}, selected app: {shareTarget}");

                    if (result == NativeShare.ShareResult.Shared)
                    {
                        // You could reward the player with 10 coins here!
                        Debug.Log("Player successfully shared the invite!");
                    }
                })
                .Share();
        }


        
    }
}