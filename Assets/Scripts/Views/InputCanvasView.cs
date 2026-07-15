using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class InputCanvasView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public MyPlayerSettings playerSettings;
        public UIDataSO uiData;


        [Header("Panel Objects")]
        public List<PanelOnOff> panels;
      
        public void TurnOff()
        {
           foreach (var item in panels)
            {
                item.TurnOffPanel();
            }

          // StartCoroutine(WaitAndTurnOff(uiData.disableTime));
        }

        public void TurnOn()
        {
           
            foreach (var item in panels)
            {
                item.TurnOnPanel();
            }
        }   
        
        private IEnumerator WaitAndTurnOff(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            gameObject.SetActive(false);
        }
    }
}
