using UnityEngine;
using UnityEngine.UI;

public class FriendsAndNotificationTest : MonoBehaviour
{
    [Header(" Buttons")]
    public Button SendChallengeBtn;
    public Button NotificationBtn;
    public Button AcceptBtn;

    [Header(" Key Codes")]
    public KeyCode sendChallengeBtn = KeyCode.U;
    public KeyCode notificationBtn = KeyCode.N;
    public KeyCode acceptBtn = KeyCode.O;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(sendChallengeBtn))
        {
            SendChallengeBtn = GameObject.Find("SendChallengeButton").GetComponent<Button>();
            SendChallengeBtn.onClick?.Invoke();
        }
        if (Input.GetKeyDown(notificationBtn))
        {
            NotificationBtn = GameObject.Find("NotificationButton").GetComponent<Button>();
            NotificationBtn.onClick?.Invoke();
        }
        if (Input.GetKeyDown(acceptBtn))
        {
            AcceptBtn = GameObject.Find("AcceptButton").GetComponent<Button>();
            AcceptBtn.onClick?.Invoke();
        }
    }
}
