using com.VisionXR.HelperClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace com.VisionXR.GameElements
{

    public class SetPlayerProperties : MonoBehaviour
    {
        public Player currentPlayer;
        private void Awake()
        {
            currentPlayer = GetComponent<Player>();

        }
        // Start is called before the first frame update
        void Start()
        {

        }
        public void SetCoin(Player currentPlayer, int id)
        {

            if (id == 1)
            {
                currentPlayer.myCoin = (PlayerCoin.White);
                currentPlayer.myTeam = (Team.TeamA);
            }
            else
            {
                currentPlayer.myCoin = (PlayerCoin.Black);
                currentPlayer.myTeam = (Team.TeamB);
            }
           // currentPlayer.ConstructPlayer();
        }

    }
}
