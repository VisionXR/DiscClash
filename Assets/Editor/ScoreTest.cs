using com.VisionXR.GameElements;
using UnityEditor;
using UnityEngine;


namespace com.VisionXR.EditorTools
{
    public class ScoreTest : EditorWindow
    {
        [Header("References")]
        public GameDataSO gameData;

        // This is a [System.Serializable] class instance
        public Player targetPlayer;

        [Header("Display")]
        [SerializeField] private string scoreDisplayString = "Score result will appear here...";

        // Serialized object references for handling serializable classes in EditorWindows
        private SerializedObject serializedWindow;
        private SerializedProperty targetPlayerProperty;

        [MenuItem("VisionXR/Score Test Tool")]
        public static void ShowWindow()
        {
            GetWindow<ScoreTest>("Score Test");
        }

        private void OnEnable()
        {
            // Initialize SerializedObject for this window instance
            serializedWindow = new SerializedObject(this);
            targetPlayerProperty = serializedWindow.FindProperty("targetPlayer");
        }

        private void OnGUI()
        {
            // Always update the serialized object at the start of OnGUI
            serializedWindow.Update();

            GUILayout.Label("Score Testing Tool", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            // GameDataSO is a ScriptableObject, so standard ObjectField works
            gameData = (GameDataSO)EditorGUILayout.ObjectField("Game Data SO", gameData, typeof(GameDataSO), false);

            EditorGUILayout.Space(5);

            // Player is a [System.Serializable] class, so PropertyField draws its inspector layout automatically
            EditorGUILayout.PropertyField(targetPlayerProperty, true);

            // Apply modifications back to the window object
            serializedWindow.ApplyModifiedProperties();

            EditorGUILayout.Space(10);

            // String display for score output
            EditorGUILayout.LabelField("Score Output", EditorStyles.boldLabel);
            scoreDisplayString = EditorGUILayout.TextField(scoreDisplayString);

            EditorGUILayout.Space(15);

            // Button validation
            GUI.enabled = gameData != null;

            if (GUILayout.Button("Check Score", GUILayout.Height(35)))
            {
                RunScoreCheckLogic();
            }

            GUI.enabled = true;
        }

        private void RunScoreCheckLogic()
        {
            if (targetPlayer == null)
            {
                Debug.LogWarning("[ScoreTest] Target Player reference is missing!");
                scoreDisplayString = gameData.GetBWMatchPoints(targetPlayer).ToString();
                return;
            }

            // TODO: Implement your custom score check logic here using 'gameData' and 'targetPlayer'
            // Example: scoreDisplayString = $"Player ID: {targetPlayer.myId}, Score: ...";

            Debug.Log("[ScoreTest] Check Score button clicked successfully.");
           // scoreDisplayString = "Logic executed successfully!";
        }
    }
}