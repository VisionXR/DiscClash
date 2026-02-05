using UnityEngine;
using UnityEditor;
using com.VisionXR.ModelClasses;
using com.VisionXR.HelperClasses;

namespace com.VisionXR.EditorTools
{
    public class PlayerTest : EditorWindow
    {
        [Header("Scriptable Objects")]
        public CoinDataSO coinData;
        public PlayersDataSO playersDataSO;
        public GameDataSO gameDataSO;

        [Header("Test Parameters")]
        public PlayerProperties testPlayerProperties = new PlayerProperties();
        private bool _showPlayerProperties = true;

        public int playerId = 1;
        public int turnId = 1;

        [MenuItem("VisionXR/Player Test Tool")]
        public static void ShowWindow()
        {
            GetWindow<PlayerTest>("Player Test");
        }

        private void OnGUI()
        {
            GUILayout.Label("Player & Game State Testing", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // 1. Scriptable Object Fields
            coinData = (CoinDataSO)EditorGUILayout.ObjectField(
                "Coin Data SO",
                coinData,
                typeof(CoinDataSO),
                false);


            playersDataSO = (PlayersDataSO)EditorGUILayout.ObjectField(
                "Players Data SO",
                playersDataSO,
                typeof(PlayersDataSO),
                false);

            gameDataSO = (GameDataSO)EditorGUILayout.ObjectField(
                "Game Data SO",
                gameDataSO,
                typeof(GameDataSO),
                false);

            EditorGUILayout.Space();

            // 2. Draw PlayerProperties manually
            _showPlayerProperties = EditorGUILayout.BeginFoldoutHeaderGroup(_showPlayerProperties, "Test Player Properties");
            if (_showPlayerProperties)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                testPlayerProperties.myId = EditorGUILayout.IntField("My ID", testPlayerProperties.myId);
                testPlayerProperties.myStrikerID = EditorGUILayout.IntField("Striker ID", testPlayerProperties.myStrikerID);
                testPlayerProperties.myOculusID = (ulong)EditorGUILayout.LongField("Oculus ID", (long)testPlayerProperties.myOculusID);
                testPlayerProperties.myName = EditorGUILayout.TextField("Name", testPlayerProperties.myName);
                testPlayerProperties.imageURL = EditorGUILayout.TextField("Image URL", testPlayerProperties.imageURL);

                EditorGUILayout.Space(5);
                testPlayerProperties.myPlayerControl = (PlayerControl)EditorGUILayout.EnumPopup("Control Type", testPlayerProperties.myPlayerControl);
                testPlayerProperties.myPlayerRole = (PlayerRole)EditorGUILayout.EnumPopup("Player Role", testPlayerProperties.myPlayerRole);
                testPlayerProperties.myCoin = (PlayerCoin)EditorGUILayout.EnumPopup("Coin Type", testPlayerProperties.myCoin);
                testPlayerProperties.myTeam = (Team)EditorGUILayout.EnumPopup("Team", testPlayerProperties.myTeam);
                testPlayerProperties.myAiDifficulty = (AIDifficulty)EditorGUILayout.EnumPopup("AI Difficulty", testPlayerProperties.myAiDifficulty);

                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.Space();

            // 3. Simple ID fields
            playerId = EditorGUILayout.IntField("Player ID to Create", playerId);
            turnId = EditorGUILayout.IntField("Set Turn ID", turnId);

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            // 4. Create Player Button
            if (GUILayout.Button("Create Player", GUILayout.Height(30)))
            {
                if (playersDataSO != null)
                {
                  

                    coinData.CreateAllCoins();

                    Debug.Log($"[PlayerTest] Attempting to create player: {testPlayerProperties.myName} with ID: {playerId}");

                    // Trigger your actual logic here:
                     playersDataSO.CreatePlayer(testPlayerProperties);
                }
                else
                {
                    Debug.LogError("Assign PlayersDataSO first!");
                }
            }

            // 5. Change Turn Button
            if (GUILayout.Button("Change Turn", GUILayout.Height(30)))
            {
                if (gameDataSO != null)
                {
                    Undo.RecordObject(gameDataSO, "Change Turn ID");
                    gameDataSO.ChangeTurn(turnId);
                    Debug.Log($"[PlayerTest] Turn changed to: {turnId}");
                }
                else
                {
                    Debug.LogError("Assign GameDataSO first!");
                }
            }

            EditorGUILayout.EndHorizontal();

            // 6. Status Feedback
            EditorGUILayout.Space();
            if (playersDataSO == null || gameDataSO == null)
            {
                EditorGUILayout.HelpBox("Please assign all Scriptable Objects to enable testing.", MessageType.Warning);
            }
        }
    }
}