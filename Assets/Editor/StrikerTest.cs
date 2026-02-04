using UnityEngine;
using UnityEditor;
using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;

namespace com.VisionXR.EditorTools
{
    public class StrikerTest : EditorWindow
    {
        [Header("References")]
        public StrikerMovement strikerMovement;

        [Header("Test Parameters")]
        public int strikerId = 1;
        [Range(0f, 1f)]
        public float moveValue = 0.5f;

        [MenuItem("VisionXR/Striker Test Tool")]
        public static void ShowWindow()
        {
            GetWindow<StrikerTest>("Striker Test");
        }

        private void OnGUI()
        {
            GUILayout.Label("Striker Testing Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // 1. Reference Fields
            strikerMovement = (StrikerMovement)EditorGUILayout.ObjectField(
                "Striker Object",
                strikerMovement,
                typeof(StrikerMovement),
                true);

            EditorGUILayout.Space();

            // 2. ID and Slider Fields
            strikerId = EditorGUILayout.IntField("Striker ID", strikerId);
            moveValue = EditorGUILayout.Slider("Move Value (0-1)", moveValue, 0f, 1f);

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            // 3. Set Striker Button
            if (GUILayout.Button("Set Striker", GUILayout.Height(30)))
            {
                if (strikerMovement != null)
                {
                    // Record undo so the change is saved in the scene
                    Undo.RecordObject(strikerMovement.gameObject.transform, "Set Striker ID");
                    strikerMovement.SetStrikerID(strikerId);
                    Debug.Log($"[StrikerTest] Set ID to {strikerId} on {strikerMovement.name}");
                }
                else
                {
                    Debug.LogError("Please assign a Striker Object!");
                }
            }

            // 4. Move Striker Button
            if (GUILayout.Button("Move Striker", GUILayout.Height(30)))
            {
                if (strikerMovement != null)
                {
                    Undo.RecordObject(strikerMovement.gameObject.transform, "Move Striker");
                    strikerMovement.MoveStriker(moveValue);
                    Debug.Log($"[StrikerTest] Moved Striker to {moveValue}");
                }
                else
                {
                    Debug.LogError("Please assign a Striker Object!");
                }
            }

            EditorGUILayout.EndHorizontal();

            // Visual feedback if in play mode
            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Note: Some physics-based overlap checks in MoveStriker may behave differently outside of Play Mode.", MessageType.Info);
            }
        }
    }
}