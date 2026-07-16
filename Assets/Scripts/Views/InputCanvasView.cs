using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class InputCanvasView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public MyPlayerSettings playerSettings;
        public UIDataSO uiData;


        [Header("Panel Objects")]
        public PanelOnOff rightSidePanel;
        public PanelOnOff leftSidePanel;
      
        public void TurnOff()
        {
           if(playerSettings.myDominantHand == DominantHand.Right)
            {
                rightSidePanel.TurnOffPanel();
            }
            else
            {
                leftSidePanel.TurnOffPanel();
            }

        }

        public void TurnOn()
        {

            if (playerSettings.myDominantHand == DominantHand.Right)
            {
                rightSidePanel.TurnOnPanel();
            }
            else
            {
                leftSidePanel.TurnOnPanel();
            }

        }

    }
}
