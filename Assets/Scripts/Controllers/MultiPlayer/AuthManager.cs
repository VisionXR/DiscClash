using com.VisionXR.ModelClasses;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
// Add PlayFab Namespaces
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace com.VisionXR.Controllers
{
    public class AuthManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public MyPlayerSettings playerSettings;
        public CloudDataSO cloudData;
        public DeepLinkManager deepLinkManager;
        public AchievementsDataSO achievementsData;
        public PurchaseDataSO purchaseData;

        // local variables
        private string displayName;

        private void OnEnable()
        {           
            cloudData.LoginToGoogleEvent += GoogleLogin;
            cloudData.GuestLoginEvent += GuestLogin;
            cloudData.EditorLoginEvent += EditorLogin;

            playerSettings.ChangeDisplayNameEvent += SetPlayFabDisplayName;
        }

        private void OnDisable()
        {          
            cloudData.LoginToGoogleEvent -= GoogleLogin;
            cloudData.GuestLoginEvent -= GuestLogin;
            cloudData.EditorLoginEvent -= EditorLogin;

            playerSettings.ChangeDisplayNameEvent -= SetPlayFabDisplayName;
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
                TitleId = PlayFabSettings.TitleId,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetUserAccountInfo = true
                }
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
                TitleId = PlayFabSettings.TitleId,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetUserAccountInfo = true
                }
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
                Debug.Log("Real Carrom 3D: Google Login Successful!");

                // 1. First, set local UI data (Name and Image)
                displayName = PlayGamesPlatform.Instance.GetUserDisplayName();
                string googleID = PlayGamesPlatform.Instance.GetUserId();
                string imageUrl = PlayGamesPlatform.Instance.GetUserImageUrl();
                

                playerSettings.SetUserNameAndId(displayName, googleID);
                playerSettings.SetUserProfileImageUrl(imageUrl);
               
                deepLinkManager.ProcessGameFlow();
               
               StartCoroutine(LoadData(imageUrl));

            }

        }

        private IEnumerator LoadData(string imageUrl)
        {
            achievementsData.GetAllAchievements();
            yield return new WaitForSeconds(1f);
            RequestTokenAndLoginToPlayFab();
            yield return new WaitForSeconds(1f);
            purchaseData.GetAllItems();
            yield return new WaitForSeconds(1f);
            purchaseData.GetPurchasedItems();
            yield return new WaitForSeconds(1f);
            StartCoroutine(LoadProfileImage(imageUrl));
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
                    TitleId = PlayFabSettings.TitleId,
                    InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                    {
                        GetUserAccountInfo = true
                    }
                };

                PlayFabClientAPI.LoginWithGooglePlayGamesServices(request, OnPlayFabSuccess, OnPlayFabFailure);
            });
        }

        private void OnPlayFabSuccess(LoginResult result)
        {
           
            cloudData.PlayFabLoginSuccess();

            // Read the existing display name from the login payload
            string currentDisplayName = result.InfoResultPayload?.AccountInfo?.TitleInfo?.DisplayName;

            if (string.IsNullOrEmpty(currentDisplayName))
            {
                Debug.Log("DisplayName is null or empty. Setting standard display name...");
                if (!string.IsNullOrEmpty(displayName))
                {
                    SetPlayFabDisplayName(displayName);
                }
            }
            else
            {
                Debug.Log($"Existing PlayFab DisplayName found: {currentDisplayName}");
                playerSettings.SetUserName(currentDisplayName);
            }

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
            playerSettings.SetUserName(result.DisplayName);
        }

        private void OnPlayFabFailure(PlayFabError error)
        {
            Debug.Log("Real Carrom 3D : PlayFab Login Error: " + error.GenerateErrorReport());

            cloudData.PlayFabLoginFailure();
        }
        private IEnumerator LoadProfileImage(string url)
        {

            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
            {
                yield return uwr.SendWebRequest();

                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Failed to download avatar: " + uwr.error);
                    yield break;
                }

                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                Sprite s = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

                playerSettings.SetUserProfileImage(s);

            }
        }

        public Sprite ConvertTextureToSprite(Texture2D texture)
        {
            if (texture == null) return null;
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}