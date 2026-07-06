using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class MainCanvasView : MonoBehaviour
    {
        [Header("All Panels")]
        public List<GameObject> allMainPanels;

        public void ShowMainPanel(int id)
        {          
            allMainPanels[id].GetComponent<PanelOnOff>().TurnOnPanel();
        }

        public void HideMainPanel(int id)
        {
           
            allMainPanels[id].GetComponent<PanelOnOff>().TurnOffPanel();
        }

    }
}
