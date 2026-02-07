using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace com.VisionXR.Views
{
    public class ScorePanel2MultiPlayer : MonoBehaviour
    {

      
        [Header(" Scriptable Objects ")]
        [SerializeField] private GameDataSO gameData;
        [SerializeField] private PlayersDataSO playersData;
        [SerializeField] private UIOutputDataSO uiOutputData;
        [SerializeField] private LeaderBoardSO leaderBoard;

        [Header(" Panel Objects ")]
        public GameObject NetworkDisconnectionPanel;
        public GameObject OtherPlayerDisconnectionPanel;

        [Header(" TeamA variables ")]
        [SerializeField] private Image player1Image;     
        [SerializeField] private Image player1Timer;     
        [SerializeField] private TMP_Text Player1Name;
       
        [Header(" TeamB variables ")]
        [SerializeField] private Image player2Image;
        [SerializeField] private Image player2Timer;
        [SerializeField] private TMP_Text Player2Name;


        [Header(" Scrollers ")]
        public ImageScroller Scroller1;
        public ImageScroller Scroller2;



        private void OnEnable()
        {
           
            Start2Players();
        }

        private void OnDisable()
        {
            
        }

        public void Start2Players()
        {
            ResetStates();
            StartShowingPlayers();
        }


        public void ShowOtherPlayerLeftPopUp()
        {
            OtherPlayerDisconnectionPanel.SetActive(true);
        }


        public void StartShowingPlayers()
        {
            Player p1 = playersData.GetPlayer(1);
            if (p1 == null)
            {
                StartPlayerScroll(1);
            }

            Player p2 = playersData.GetPlayer(2);
            if (p2 == null)
            {
                StartPlayerScroll(2);
            }

        }

        public void OnPlayerJoined(Player player)
        {
            foreach (Player p in playersData.CurrentPlayers)
            {
                StopScrolling(p.myId);
                SetPlayerData(p.myId, p.myName, p.GetMyImage());
            }

        }

        private void StartPlayerScroll(int playerId)
        {
            if (playerId == 1)
            {
                Scroller1.StartScrolling();
            }
            else if (playerId == 2)
            {
                Scroller2.StartScrolling();
            }

        }
        private void StopScrolling(int playerId)
        {

            if (playerId == 1)
            {
                Scroller1.StopScrolling();
            }
            else if (playerId == 2)
            {
                Scroller2.StopScrolling();
            }
         
        }
        private void SetPlayerData(int playerId, string name, Sprite image)
        {
            TMP_Text nameText;
            Image playerImage;

            switch (playerId)
            {
                case 1:
                    nameText = Player1Name;
                    playerImage = player1Image;

                    break;
                case 2:
                    nameText = Player2Name;
                    playerImage = player2Image;

                    break;
                default:
                    return;  // Exit the function if an unexpected player ID is received
            }

            nameText.text = name;
            playerImage.sprite = image;
        }
        public void SetButton(Button b, bool state)
        {

            b.GetComponent<ButtonStates>().enabled = state;
            b.GetComponent<Animator>().SetBool("Highlight", state);
            b.interactable = state;

        }
        private void ResetStates()
        {

            Player1Name.text = "";
            Player2Name.text = "";

            player1Image.sprite = AppProperties.instance.DummyPersonIcon;
            player2Image.sprite = AppProperties.instance.DummyPersonIcon;

        }

    }
}
