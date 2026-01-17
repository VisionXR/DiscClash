using com.VisionXR.Controllers;
using com.VisionXR.Views;
using UnityEngine;

public class SinglePlayerManagerTest : MonoBehaviour
{
    [Header(" Panels ")]
    public MainPanelView mainPanelView;
    public SinglePlayerPanelView singlePlayerPanelView;
    public int turnId;

    [Header(" Key Codes ")]
    public KeyCode singlePlayerBtn = KeyCode.S;
    public KeyCode startGameBtn = KeyCode.Space;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(singlePlayerBtn))
        {
            mainPanelView.OnSinglePlayerClicked();
        }
        else if (Input.GetKeyDown(startGameBtn))
        {
            singlePlayerPanelView.StartSinglePlayerClicked();
            
        }
     

    }
}
