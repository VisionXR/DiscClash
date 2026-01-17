using System.Collections;
using TMPro;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class Toast : MonoBehaviour
    {

        public TMP_Text ToastText;

        private void OnEnable()
        {
            AudioManager.instance.PlayToastSound();
            StartCoroutine(DisableToast());
        }

        private IEnumerator DisableToast()
        {
            yield return new WaitForSeconds(3);
            gameObject.SetActive(false);
        }

        public void SetToast(string msg)
        {
            ToastText.text = msg;
        }
    }
}
