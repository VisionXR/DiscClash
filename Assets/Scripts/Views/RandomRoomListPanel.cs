using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.VisionXR.Views
{
    public class RandomRoomListPanel : MonoBehaviour
    {
        [Header(" Scriptable Objects ")]
        public UIOutputDataSO uiOutputData;
        public MyPlayerSettings playerSettings;

        [Header(" Panel Objects ")]
        public GameObject PlayWithStrangerPanel;
        public GameObject TwoPlayerPanel;
        public GameObject FourPlayerPanel;
        public GameObject RoomJoinFailedPanel;
        public GameObject UiBlocker;
        public GameObject NoRoomsText;

        [Header(" Game Objects ")]
        public GameObject randomRoomPrefab;
        public Transform contentContainer;

        // local variables
        private Coroutine roomStatusRoutine;

        private void OnEnable()
        {
          
            Invoke("DisplayRooms", 1);
        }

        private void OnDisable()
        {
           
          
        }

        private void DisplayRooms()
        {
          //  DisplayRandomRoomsList(networkData.roomsAvailable);
            
        }

        public void RefreshButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            DisplayRooms();
        }
        public void OnBackButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
  
            PlayWithStrangerPanel.SetActive(true);
            gameObject.SetActive(false);
        }

        public void DisplayRandomRoomsList(List<AvailableRooms> rooms)
        {
            // Clear existing friends in the panel
            foreach (Transform child in contentContainer)
            {
                Destroy(child.gameObject);
            }

            if(rooms.Count > 0)
            {
                NoRoomsText.SetActive(false);
            }
            else
            {
                NoRoomsText.SetActive(true);
            }

            // Create UI objects for each friend
            foreach (AvailableRooms room in rooms)
            {
                // Instantiate friend prefab
                GameObject randomObjectprefab = Instantiate(randomRoomPrefab, contentContainer);
                RandomRoomObject roomObject = randomObjectprefab.GetComponent<RandomRoomObject>();


                // Set friend name text
                roomObject.gameModeText.text = room.gameMode.ToString();
                roomObject.roomNo.text = (rooms.IndexOf(room) + 1).ToString();
                roomObject.gameTypeText.text = room.game.ToString();


                // Set challenge button callback
                Button challengeButton = roomObject.jonRoomButton;
                    
                challengeButton.onClick.AddListener(() =>
                {
                    JoinRoom(room,roomObject.joinBtnText);
                });
            }
        }

        private void JoinRoom(AvailableRooms room,TMP_Text joinBtnText)
        {
            AudioManager.instance.PlayButtonClickSound();
        //    uiData.SetRoomName(room.roomName);
            uiOutputData.SetGameMode(room.gameMode);
            uiOutputData.SetGame(room.game);
            uiOutputData.SetAIDifficulty(room.aiDifficulty);
            playerSettings.SetBoard(room.MyBoard);
        //    networkData.JoinRoom(room.roomName);
            if(roomStatusRoutine == null)
            {
                roomStatusRoutine = StartCoroutine(ConnectingToRoom(joinBtnText));
                UiBlocker.SetActive(true);
            }
        }

        private IEnumerator ConnectingToRoom(TMP_Text roomName)
        {
            while (true)
            {
                roomName.text = "Connecting.";
                yield return new WaitForSeconds(0.5f);
                roomName.text = "Connecting..";
                yield return new WaitForSeconds(0.5f);
                roomName.text = "Connecting...";
                yield return new WaitForSeconds(0.5f);
                roomName.text = "Connecting....";
                yield return new WaitForSeconds(0.5f);
                roomName.text = "Connecting.....";
                yield return new WaitForSeconds(0.5f);
                roomName.text = "Connecting......";
            }
        }
        private void RoomSuccess()
        {
            if (roomStatusRoutine != null)
            {
                StopCoroutine(roomStatusRoutine);
                roomStatusRoutine = null;
                UiBlocker.SetActive(false);
            }


            if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2)
            {
                TwoPlayerPanel.SetActive(true);
                gameObject.SetActive(false);
            }
            else
            {
                FourPlayerPanel.SetActive(true);
                gameObject.SetActive(false);
            }
        }

        private void RoomFailed(string reason)
        {
            AudioManager.instance.PlayPopUpSound();

            if (roomStatusRoutine != null)
            {
                StopCoroutine(roomStatusRoutine);
                roomStatusRoutine = null;
                UiBlocker.SetActive(false);
            }

            RoomJoinFailedPanel.SetActive(true);
        }
    }
}
