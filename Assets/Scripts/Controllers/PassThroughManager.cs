using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Controllers
{

    public class PassThroughManager : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public MyPlayerSettings playerSettings;

        [Header(" Camera Objects")]
        public Camera CenterCam;
        public Camera LeftCam;
        public Camera RightCam;

        [Header(" Game Objects")]
        public GameObject Arena;
        public GameObject PassThroughObject;

        void OnEnable()
        {

            playerSettings.PassThroughChangedEvent += SetMode;
        }

        void OnDisable()
        {
            playerSettings.PassThroughChangedEvent -= SetMode;
        }

        public void SetMode()
        {
            
            if(playerSettings.isPassThrough)
            {
                SetPassThroughMode();
            }
            else
            {
                SetVRMode();
            }
        }

        private void SetPassThroughMode()
        {

            CenterCam.clearFlags = CameraClearFlags.SolidColor;
            LeftCam.clearFlags = CameraClearFlags.SolidColor;
            RightCam.clearFlags = CameraClearFlags.SolidColor;


            // VERY IMPORTANT: Set the background color to be fully transparent
            // Color(R, G, B, Alpha)
            Color transparentBlack = new Color(0, 0, 0, 0);
            CenterCam.backgroundColor = transparentBlack;
            LeftCam.backgroundColor = transparentBlack;
            RightCam.backgroundColor = transparentBlack;

            Arena.SetActive(false);
            PassThroughObject.SetActive(true);
          
        }

        private void SetVRMode()
        {

            CenterCam.clearFlags = CameraClearFlags.Skybox;
            LeftCam.clearFlags = CameraClearFlags.Skybox;
            RightCam.clearFlags = CameraClearFlags.Skybox;

            PassThroughObject.SetActive(false);
            Arena.SetActive(true);

        }
    }
}
