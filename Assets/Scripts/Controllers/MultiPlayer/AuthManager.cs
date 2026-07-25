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

        // local variables
        private string displayName;

        private void OnEnable()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            cloudData.LoginToGoogleEvent += GoogleLogin;
            cloudData.GuestLoginEvent += GuestLogin;
            cloudData.EditorLoginEvent += EditorLogin;
        }

        private void OnDisable()
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
            cloudData.LoginToGoogleEvent -= GoogleLogin;
            cloudData.GuestLoginEvent -= GuestLogin;
            cloudData.EditorLoginEvent -= EditorLogin;
        }

        private void EditorLogin()
        {
            displayName = "Guest_ " + SystemInfo.deviceUniqueIdentifier.Substring(0, 5);

            // Simplified Editor Mock
            playerSettings.SetUserNameAndId(displayName, SystemInfo.deviceUniqueIdentifier);
            deepLinkManager.ProcessGameFlow();



            // If in Editor, use a fixed string so you always log into the same test account
            // If on Mobile, use the unique Device ID
            string customId =  SystemInfo.deviceUniqueIdentifier;

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
            displayName = "Guest_ " + SystemInfo.deviceUniqueIdentifier.Substring(0, 5);

            // Simplified Editor Mock
            playerSettings.SetUserNameAndId(displayName, SystemInfo.deviceUniqueIdentifier);

            deepLinkManager.ProcessGameFlow();

            // If in Editor, use a fixed string so you always log into the same test account
            // If on Mobile, use the unique Device ID
            string customId = SystemInfo.deviceUniqueIdentifier;

            var request = new LoginWithCustomIDRequest
            {
                CustomId = customId,
                CreateAccount = true,
                TitleId = PlayFabSettings.TitleId
            };


            PlayFabClientAPI.LoginWithCustomID(request, OnPlayFabSuccess, OnPlayFabFailure);
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
                displayName = PlayGamesPlatform.Instance.GetUserDisplayName();
                string googleID = PlayGamesPlatform.Instance.GetUserId();
                string imageUrl = PlayGamesPlatform.Instance.GetUserImageUrl();
                StartCoroutine(LoadProfileImage());

                playerSettings.SetUserNameAndId(displayName, googleID);
                playerSettings.SetUserProfileImageUrl(imageUrl);
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
            Debug.Log("Real Carrom : PlayFab Login Success! PlayFabID: ");

            playerSettings.SetLogIn(true);
            playerSettings.SaveSettings();
            cloudData.PlayFabLoginSuccess();

            if (result.NewlyCreated)
            {
                Debug.Log("New PlayFab account detected! Setting up display name...");

                if (!string.IsNullOrEmpty(displayName))
                {
                    SetPlayFabDisplayName(displayName);
                }
            }
            else
            {
                Debug.Log("Existing user logged in. Skipping display name update to save API calls.");
                if (!string.IsNullOrEmpty(displayName))
                {
                    SetPlayFabDisplayName(displayName);
                }
            }
            //// OPTIONAL: Update PlayFab display name to match Google name
            //UpdatePlayFabDisplayName(Social.localUser.userName);
        }

        private void OnPlayFabFailure(PlayFabError error)
        {
            Debug.Log("Real Carrom 3D : PlayFab Login Error: " + error.GenerateErrorReport());

            cloudData.PlayFabLoginFailure();
        }

        public void SetPlayFabDisplayName(string displayName)
        {
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = displayName
            };

            PlayFabClientAPI.UpdateUserTitleDisplayName(request,
                OnDisplayNameUpdateSuccess,
                OnPlayFabFailure
            );
        }

        private void OnDisplayNameUpdateSuccess(UpdateUserTitleDisplayNameResult result)
        {
            Debug.Log($"PlayFab Display Name successfully set to: {result.DisplayName}");
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
                Debug.Log("Real Carrom: Profile Image Loaded!");
            }
        }

        public Sprite ConvertTextureToSprite(Texture2D texture)
        {
            if (texture == null) return null;
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}