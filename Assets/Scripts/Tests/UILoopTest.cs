using UnityEngine;
using UnityEngine.UI;

public class UILoopTest : MonoBehaviour
{
    [Header("Buttons")]
    public Button ExitButton;
    public Button ExitYesButton;
    public Button HomeButton;
    public Button PlayAgain;

    [Header("KeyCodes")]
    public KeyCode homeBtn = KeyCode.H;
    public KeyCode exitBtn = KeyCode.E;
    public KeyCode playAgainBtn = KeyCode.P;
    public KeyCode yesBtn = KeyCode.Y;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(exitBtn))
        {
            ExitButton = GameObject.Find("ExitButton").GetComponent<Button>();
            ExitButton.onClick?.Invoke();
        }

        if (Input.GetKeyDown(yesBtn))
        {
            ExitYesButton.onClick?.Invoke();
        }

        if (Input.GetKeyDown(homeBtn))
        {
            HomeButton.onClick?.Invoke();
        }

        if (Input.GetKeyDown(playAgainBtn))
        {
            PlayAgain.onClick?.Invoke();
        }
    }
}
