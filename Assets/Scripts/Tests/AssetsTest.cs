
using com.VisionXR.Views;
using UnityEngine;

public class AssetsTest : MonoBehaviour
{
    public AssetDisplay assetDisplay;
    public BoardsPanelView boardPanel;
    public int boardId; 

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            assetDisplay.BoardButtonClicked();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            boardPanel.OnBoardSelected(boardId);
        }
    }
}
