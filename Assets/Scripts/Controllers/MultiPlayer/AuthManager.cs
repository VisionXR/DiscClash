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
            playerSettings.SetUserNameAndId("Guest_Player", "12345");
            playerSettings.SetLogIn(true);
            playerSettings.SaveSettings();

            // If in Editor, use a fixed string so you always log into the same test account
            // If on Mobile, use the unique Device ID
            string customId = Application.isEditor ? "Editor_Test_User" : SystemInfo.deviceUniqueIdentifier;

            var request = new LoginWithCustomIDRequest
            {
                CustomId = customId,
                CreateAccount = true,
                TitleId = PlayFabSettings.TitleId
            };

            Debug.Log("Disc Clash: Logging in as Guest/Editor...");
            PlayFabClientAPI.LoginWithCustomID(request, OnPlayFabSuccess, OnPlayFabFailure);
        }

        private void GuestLogin()
        {
            // Simplified Editor Mock
            playerSettings.SetUserNameAndId("Guest_Player", "12345");
        }

        public void GoogleLogin()
        {
            if (!Application.isEditor)
            {
                Debug.Log("Trying to login!");
                PlayGamesPlatform.Activate();
                PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
            }
            else
            {
                // Simplified Editor Mock
                playerSettings.SetUserNameAndId("Editor_Player", "12345");
            }
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
                playerSettings.SetLogIn(true);
                playerSettings.SaveSettings();

                // 2. Trigger PlayFab Login
                RequestTokenAndLoginToPlayFab();
            }
            else
            {
                Debug.LogError("Disc Clash: Google Login Failed: " + status);

                // Simplified Editor Mock
                playerSettings.SetUserNameAndId("TejaBhai", "12345");
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

            cloudData.PlayFabLoginSuccess();

            // OPTIONAL: Update PlayFab display name to match Google name
            UpdatePlayFabDisplayName(Social.localUser.userName);
        }

        private void OnPlayFabFailure(PlayFabError error)
        {
            Debug.Log("Disc Clash: PlayFab Login Error: " + error.GenerateErrorReport());

            cloudData.PlayFabLoginFailure();
        }

        private void UpdatePlayFabDisplayName(string name)
        {
            var request = new UpdateUserTitleDisplayNameRequest { DisplayName = name };
            PlayFabClientAPI.UpdateUserTitleDisplayName(request,
                res => Debug.Log("PlayFab Display Name Updated"),
                err => Debug.LogWarning("Could not update PlayFab Display Name"));
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