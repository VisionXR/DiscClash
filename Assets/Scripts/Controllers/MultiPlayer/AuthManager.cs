using com.VisionXR.ModelClasses;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using UnityEngine;
// Add PlayFab Namespaces
using PlayFab;
using PlayFab.ClientModels;
using System;

namespace com.VisionXR.Controllers
{
    public class AuthManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public MyPlayerSettings playerSettings;
        public CloudDataSO cloudData;
        public DeepLinkManager deepLinkManager;

        private void OnEnable()
        {
            cloudData.LoginToGoogleEvent += GoogleLogin;
            cloudData.GuestLoginEvent += GuestLogin;
            cloudData.EditorLoginEvent += EditorLogin;
        }

        private void OnDisable()
        {
            cloudData.LoginToGoogleEvent -= GoogleLogin;
            cloudData.GuestLoginEvent -= GuestLogin;
            cloudData.EditorLoginEvent -= EditorLogin;
        }

        private void EditorLogin()
        {
            // Simplified Editor Mock
            playerSettings.SetUserNameAndId("Guest_Player", UnityEngine.Random.Range(0, 9999).ToString());
            deepLinkManager.ProcessGameFlow();

            // If in Editor, use a fixed string so you always log into the same test account
            // If on Mobile, use the unique Device ID
            string customId = Application.isEditor ? "Editor_Test_User" : SystemInfo.deviceUniqueIdentifier;

            var request = new LoginWithCustomIDRequest
            {
                CustomId = customId,
                CreateAccount = true,
                TitleId = PlayFabSettings.TitleId
            };

   
            PlayFabClientAPI.LoginWithCustomID(request, OnPlayFabSuccess, OnPlayFabFailure);
        }

        private void GuestLogin()
        {
            // Simplified Editor Mock
            playerSettings.SetUserNameAndId("Guest_Player", "12345");
            playerSettings.SetLogIn(false);
            playerSettings.SaveSettings();
            deepLinkManager.ProcessGameFlow();
        }

        public void GoogleLogin()
        {
            Debug.Log("Trying to login!");
            PlayGamesPlatform.Activate();
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        }

        internal void ProcessAuthentication(SignInStatus status)
        {
            if (status == SignInStatus.Success)
            {
                Debug.Log("Disc Clash: Google Login Successful!");

                // 1. First, set local UI data (Name and Image)
                string name = Social.localUser.userName;
                string googleID = Social.localUser.id;
                StartCoroutine(LoadProfileImage());

                playerSettings.SetUserNameAndId(name, googleID);

                deepLinkManager.ProcessGameFlow();

                // 2. Trigger PlayFab Login
                RequestTokenAndLoginToPlayFab();
            }

        }

        private void RequestTokenAndLoginToPlayFab()
        {
            PlayGamesPlatform.Instance.RequestServerSideAccess(true, (authCode) =>
            {
                if (string.IsNullOrEmpty(authCode)) return;


                Debug.Log("Disc Clash: Received Google Auth Code, logging into PlayFab...");
                // Use LoginWithGooglePlayGamesServices instead of LoginWithGoogleAccount
                var request = new LoginWithGooglePlayGamesServicesRequest
                {
                    ServerAuthCode = authCode,
                    CreateAccount = true,
                    TitleId = PlayFabSettings.TitleId
                };

                PlayFabClientAPI.LoginWithGooglePlayGamesServices(request, OnPlayFabSuccess, OnPlayFabFailure);
            });
        }

        private void OnPlayFabSuccess(LoginResult result)
        {
            Debug.Log("Disc Clash: PlayFab Login Success! PlayFabID: " + result.PlayFabId);

            playerSettings.SetLogIn(true);
            playerSettings.SaveSettings();
            cloudData.PlayFabLoginSuccess();

            //// OPTIONAL: Update PlayFab display name to match Google name
            //UpdatePlayFabDisplayName(Social.localUser.userName);
        }

        private void OnPlayFabFailure(PlayFabError error)
        {
            Debug.Log("Disc Clash: PlayFab Login Error: " + error.GenerateErrorReport());

            cloudData.PlayFabLoginFailure();
        }

        private IEnumerator LoadProfileImage()
        {
            float timeout = 5f;
            while (Social.localUser.image == null && timeout > 0)
            {
                timeout -= Time.deltaTime;
                yield return null;
            }

            if (Social.localUser.image != null)
            {
                playerSettings.SetUserProfileImage(ConvertTextureToSprite(Social.localUser.image));
                Debug.Log("Disc Clash: Profile Image Loaded!");
            }
        }

        public Sprite ConvertTextureToSprite(Texture2D texture)
        {
            if (texture == null) return null;
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}