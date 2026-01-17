using com.VisionXR.Views;
using UnityEngine;
using UnityEngine.UI;

public class UITest : MonoBehaviour
{
    [Header(" Panels and buttons")]
    public MainPanelView mainPanelView;
    public PrivatePublicPanelView privatePublicPanelView;
    public PlayWithFriendPanel playWithFriendPanel;
    public PlayWithStrangerPanel playWithStrangerPanel;
    public RandomRoomListPanel randomRoomListPanel;
    public Button joinBtn;

    [Header("KeyCodes")]
    public KeyCode multiPlayerBtn = KeyCode.M;
    public KeyCode playWithFriendsBtn = KeyCode.F;
    public KeyCode playWithStrangersBtn = KeyCode.S;
    public KeyCode createRoomBtn = KeyCode.C;
    public KeyCode joinRoomBtn = KeyCode.J;
    public KeyCode joinRoomFriendsBtn = KeyCode.Space;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(multiPlayerBtn))
        {
            mainPanelView.OnMultiPlayerClicked();
        }

        else if(Input.GetKeyDown(playWithFriendsBtn))
        {
            privatePublicPanelView.PlayWithFriendsBtnClicked();
        }

        else if (Input.GetKeyDown(playWithStrangersBtn))
        {
            privatePublicPanelView.PlayWithStarngersClicked();
        }

        else if (Input.GetKeyDown(createRoomBtn))
        {
            if (playWithFriendPanel.gameObject.activeInHierarchy)
            {
                playWithFriendPanel.CreateRoomBtnClicked();
            }
            else
            {
               // playWithStrangerPanel.CreateRoomBtnClicked();
            }
        }

        else if (Input.GetKeyDown(joinRoomBtn))
        {
            playWithStrangerPanel.JoinRoomBtnClicked();
        }

        else if (Input.GetKeyDown(joinRoomFriendsBtn))
        {
            Button b = GameObject.Find("JoinRoomBtn").GetComponent<Button>();
            b.onClick?.Invoke();

        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            randomRoomListPanel.RefreshButtonClicked();

        }
    }
}
