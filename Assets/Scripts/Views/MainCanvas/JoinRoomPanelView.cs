using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using TMPro;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class JoinRoomPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]

        public DestinationDataSO destinationData;
        public UIDataSO uiData;
        public UIOutputDataSO uiOutPutData;

        [Header("game Objects")]
        public Destination multiPlayerDestination;
        public TMP_InputField roomCodeInputField;
        public DestinationPanelView destinationPanelView;
       

        [Header("Next And Previous Panels")]
        public string destinationState;
        public string currentState;


        public void JoinRoomBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();

            string fullInput = roomCodeInputField.text;
            string actualRoomName = fullInput;
            ServerRegion targetRegion = ServerRegion.any;

            // Ensure the input has at least 2 characters before splitting
            if (!string.IsNullOrEmpty(fullInput) && fullInput.Length >= 2)
            {
                string regionCodeStr = fullInput.Substring(0, 2);
                actualRoomName = fullInput.Substring(2);

                // 2. Convert the 2-digit string to an integer
                if (int.TryParse(regionCodeStr, out int regionIndex))
                {
                    // 3. Explicitly cast the integer to your ServerRegion enum
                    // (Note: This assumes the parsed integer maps directly to your enum indexes)
                    targetRegion = (ServerRegion)regionIndex;
                }
                else
                {
                    Debug.LogWarning($"Could not parse '{regionCodeStr}' into an integer. Defaulting to 'any'.");
                }
            }
            else
            {
                Debug.LogWarning("Room code entered is too short! Using fallback handling.");
            }

            multiPlayerDestination.roomName = actualRoomName;
            multiPlayerDestination.region = targetRegion;
            multiPlayerDestination.gameMode = uiOutPutData.gameMode;

            uiData.uiManager.ChangeState(destinationState, true);        
            StartCoroutine(Join(multiPlayerDestination));

        }

        private IEnumerator Join(Destination destination)
        {
            yield return new WaitForSeconds(uiData.disableTime);
            destinationPanelView.ConnectToDestination(destination);
        }

        public void BackBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(currentState, false);
        }

    }
}
