using UnityEngine;
using UnityEngine.UI;

public class MultiPlayerUITest : MonoBehaviour
{
    [Header(" Buttons ")]
    public Button Player1StartButton;
    public Button Player2StartButton;
    public Button Player3StartButton;
    public Button Player4StartButton;

    [Header(" Key Codes ")]
    public KeyCode player1ReadyBtn = KeyCode.Alpha1;
    public KeyCode player2ReadyBtn = KeyCode.Alpha2;
    public KeyCode player3ReadyBtn = KeyCode.Alpha3;
    public KeyCode player4ReadyBtn = KeyCode.Alpha4;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(player1ReadyBtn))
        {

            Debug.Log(" 1 pressed ");
            Player1StartButton = GameObject.Find("PressWhenReadyButton1").GetComponent<Button>();
            Player1StartButton.onClick.Invoke();
           
            
        }

        if (Input.GetKeyDown(player2ReadyBtn))
        {

            Debug.Log(" 2 pressed ");
            Player2StartButton = GameObject.Find("PressWhenReadyButton2").GetComponent<Button>();
            Player2StartButton.onClick.Invoke();
          

        }

        if (Input.GetKeyDown(player3ReadyBtn))
        {
            Player3StartButton = GameObject.Find("PressWhenReadyButton3").GetComponent<Button>();
            Player3StartButton.onClick.Invoke();
           

        }

        if (Input.GetKeyDown(player4ReadyBtn))
        {
            Player4StartButton = GameObject.Find("PressWhenReadyButton4").GetComponent<Button>();
            Player4StartButton.onClick.Invoke();
           

        }
    }
}
