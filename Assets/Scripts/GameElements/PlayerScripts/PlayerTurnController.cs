using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;


namespace com.VisionXR.GameElements
{
    public class PlayerTurnController : MonoBehaviour
    {

        [Header("Scriptable Objects")]
        
        public GameDataSO gameData;
        public CoinDataSO coinData;
        public StrikerDataSO strikerData;
        public InputDataSO inputData;
        public BoardPropertiesSO boardProperties;

        [Header("local variables")]
        public Player player;   
        public PlayerInput playerInput;
        public AIPlayer aiPlayer;

        private void OnEnable()
        {
            gameData.TurnChangedEvent += OnTurnChanged;
        }

        private void OnDisable()
        {
            gameData.TurnChangedEvent -= OnTurnChanged;
        }

        private void OnTurnChanged(int currentPlayerId)
        {

            coinData.ResetData();
            strikerData.ResetStrikerData();
          

         
            if (player.myId == currentPlayerId) // Id matches
            {
                player.strikerMovement.ResetStriker(); // Reset the striker
                player.myStriker.SetActive(true);

                if (player.myPlayerControl == PlayerControl.Local) // local player
                {
                    boardProperties.TurnOnHoles();
                    if (player.myPlayerRole == PlayerRole.Human)
                    {

                        player.strikerArrow.TurnOnArrow();
                        inputData.ActivateInput();
                        playerInput.enabled = true;
                    }
                    else // Its AI
                    {
                        player.strikerArrow.TurnOffArrow();
                        inputData.DeactivateInput();
                        aiPlayer.AIShouldPlay();

                    }
                }
                else // Remote player
                {
                    boardProperties.TurnOffHoles();
                    inputData.DeactivateInput();

                    if (player.myPlayerRole == PlayerRole.Human)
                    {
                        player.strikerArrow.TurnOnArrow();
                    }
                    else // Its AI
                    {
                        player.strikerArrow.TurnOffArrow();
                    }
                }

              
            }   // turn id matches my id
            else // Id does not match
            {

                  player.myStriker.SetActive(false);
                  playerInput.enabled = false;   
            }
        }

    }
}
