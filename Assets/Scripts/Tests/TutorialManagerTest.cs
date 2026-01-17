using com.VisionXR.Views;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManagerTest : MonoBehaviour
{
    public MainPanelView mainPanelView;
    public Button NxtButton;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            mainPanelView.OnTutorialClicked();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            NxtButton = GameObject.Find("NextButton").GetComponent<Button>();
            NxtButton.onClick?.Invoke();
        }
    }
}
