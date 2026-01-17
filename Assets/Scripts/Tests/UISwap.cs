
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class UISwap : MonoBehaviour
{
    public Canvas mainCanvas;
    public Canvas tutorialCanvas;
    public Canvas leftCanvas;
    public Canvas rightCanvas;
    public Canvas centerCanvas;
    public Canvas trickShotCanvas;
    public StandaloneInputModule standaloneInputModule;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(Application.isEditor)
        {
            mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            tutorialCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            leftCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            rightCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            centerCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            trickShotCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            standaloneInputModule.enabled = true;
            
        }
    }


}
