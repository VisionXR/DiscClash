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
        public List<TMP_Text> playerStatuses;
        public List<Image> playerImages;
        public GameObject StartButton;

        [Header("UI Elements")]
        public TMP_Text totalCoinsText;

        // Actions
        public Action OnEntryFeesDeductedSuccess;
        public Action OnEntryFeesDeductedFailure;

        // Keeps track of which status coroutine is running for which player ID
        private Dictionary<int, Coroutine> statusCoroutines = new Dictionary<int, Coroutine>();

        // Entry fee animation coroutine reference
        private Coroutine entryFeeCoroutine;

        private void OnEnable()
        {
            StartButton.SetActive(false);
            // When the panel opens, start the "Connecting..." animation for everyone
            for (int i = 0; i < playerStatuses.Count; i++)
            {
                int playerId = i + 1;
                StartConnectingAnimation(playerId);
            }

            for (int i = 0; i < playerCoins.Count; i++)
            {
                playerCoins[i].text = uiOutputData.EntryFee.ToString();
            }

            // initialize total coins text to zero
            totalCoinsText.text = "0";

            gameModeText.text = Enum.GetName(typeof(GameMode), uiOutputData.gameMode);

            OnEntryFeesDeductedSuccess += OnEntryFeesDeductionSuccess;
            OnEntryFeesDeductedFailure += OnEntryFeesDeductionFailure;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            statusCoroutines.Clear();

            // ensure we clear any entry fee coroutine reference (StopAllCoroutines handled)
            entryFeeCoroutine = null;

            OnEntryFeesDeductedSuccess -= OnEntryFeesDeductionSuccess;
            OnEntryFeesDeductedFailure -= OnEntryFeesDeductionFailure;
        }

        private void OnEntryFeesDeductionSuccess()
        {
            Debug.Log("Entry fees success");

            if (entryFeeCoroutine == null)
            {
                entryFeeCoroutine = StartCoroutine(AnimateEntryFeeDeduction(3f));
            }    
        }

        private void OnEntryFeesDeductionFailure()
        {
            Debug.Log("Entry fees failure");

            for (int i = 0; i < playerCoins.Count; i++)
            {
                playerCoins[i].text = uiOutputData.EntryFee.ToString();
            }

            totalCoinsText.text = "0";
        }

        public void Deductfee()
        {
           

            // call cloud to deduct (CloudManager will invoke the success/failure Actions)
            cloudData.DeductEntryFee(uiOutputData.EntryFee, OnEntryFeesDeductedSuccess, OnEntryFeesDeductedFailure);
        }

        public void SetName(int id, string name)
        {
            Debug.Log($"Setting name for player {id}: {name}");
            // Note: Changed .name to .text so it actually shows on the UI!
            playerNames[id - 1].text = name;
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

        /// <summary>
        /// Animate the entry fee deduction visually:
        /// - Each player's displayed coins count down from their entry fee to zero.
        /// - The totalCoinsText increments showing collected total (sum of deducted amounts).
        /// - The entire animation runs over the specified duration (seconds).
        /// </summary>
        private IEnumerator AnimateEntryFeeDeduction(float duration)
        {
            // Read initial values; fallback to uiOutputData.EntryFee if parsing fails
            int playerCount = Mathf.Max(1, playerCoins.Count);
            List<int> initialValues = new List<int>(playerCount);
            int totalInitial = 0;

            for (int i = 0; i < playerCount; i++)
            {
                int val;
                if (!int.TryParse(playerCoins[i].text, out val))
                {
                    val = uiOutputData.EntryFee;
                }
                initialValues.Add(val);
                totalInitial += val;
            }

            float elapsed = 0f;
            // We'll update every frame for smoothness
            while (elapsed < duration)
            {
                float t = Mathf.Clamp01(elapsed / duration);

                int remainingTotal = 0;
                for (int i = 0; i < playerCount; i++)
                {
                    // decrease from initial -> 0 using ease-out (optional): Mathf.SmoothStep
                    float eased = Mathf.SmoothStep(initialValues[i], 0f, t);
                    int remaining = Mathf.CeilToInt(eased); // ceil to avoid early zeros
                    remaining = Mathf.Max(0, remaining);
                    playerCoins[i].text = remaining.ToString();
                    remainingTotal += remaining;
                }

                int collected = totalInitial - remainingTotal;
                totalCoinsText.text = collected.ToString();

                elapsed += Time.deltaTime;
                yield return null;
            }

            // Ensure final values (exact)
            for (int i = 0; i < playerCount; i++)
            {
                playerCoins[i].text = "0";
            }
            totalCoinsText.text = totalInitial.ToString();


            // set player coins to zero and total to combined value
            int combined = uiOutputData.EntryFee * playerCoins.Count;
            for (int i = 0; i < playerCoins.Count; i++)
            {
                playerCoins[i].text = "0";
            }
            totalCoinsText.text = combined.ToString();


            StartButton.SetActive(true);
            // clear coroutine reference
            entryFeeCoroutine = null;
        }
    }
}