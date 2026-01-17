using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.GameElements
{ 
    public class StrikerRegistration : MonoBehaviour
    {
        public StrikerDataSO strikerData;
         private void Start()
         {
             strikerData.AvailableStrikersinGame.Add(gameObject);
             
         }

         private void OnDestroy()
         {
            strikerData.AvailableStrikersinGame.Remove(gameObject);
            
        }
    }
}
