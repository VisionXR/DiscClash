using UnityEngine;
using UnityEditor;
using com.VisionXR.ModelClasses;
using com.VisionXR.HelperClasses;

namespace com.VisionXR.EditorTools
{
    public class CloudTest : EditorWindow
    {
        [Header("Scriptable Objects")]
        public CloudDataSO cloudData;
        public int amount;



        [MenuItem("VisionXR/Cloud Test Tool")]
        public static void ShowWindow()
        {
            GetWindow<CloudTest>("Cloud Test");
        }

        private void OnGUI()
        {
            GUILayout.Label("Cloud  Testing", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // 1. Scriptable Object Fields
            cloudData = (CloudDataSO)EditorGUILayout.ObjectField(
                "Cloud Data SO",
                cloudData,
                typeof(CloudDataSO),
                false);

            amount = (int)EditorGUILayout.IntField("Amount",amount);


            // 4. Create Player Button
            if (GUILayout.Button("Add Coins", GUILayout.Height(30)))
            {
                if (cloudData != null)
                {

                    cloudData.GrantWinnings(200);  
                  
                }
                else
                {
                    Debug.LogError("Assign PlayersDataSO first!");
                }
            }

            // 5. Change Turn Button
            if (GUILayout.Button("Subtract Coins", GUILayout.Height(30)))
            {
                if (cloudData != null)
                {

                  //  cloudData.DeductEntryFee(amount);

                }
                else
                {
                    Debug.LogError("Assign PlayersDataSO first!");
                }
            }



        }
    }
}