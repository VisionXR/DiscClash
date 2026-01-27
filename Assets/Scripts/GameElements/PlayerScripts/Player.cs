using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace com.VisionXR.GameElements
{
    [Serializable]
    public class Player : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AIDataSO aiData;
        public MyPlayerSettings playerSettings;
        public StrikerDataSO strikerData;   
        public CamPositionSO camPositionData;
        public BoardDataSO boardData;
        public PlayersDataSO playersData;

        [Header("Local Objects")]
        public GameObject playerAvatarObj;
        public GameObject myStriker;
        public Rigidbody strikerRigidBody;
        public GameObject myAvatar;


        [Header("Local Scripts")]      
        public AIPlayer aiPlayer;
        public StrikerMovement strikerMovement;
        public StrikerShooting strikerShoot;
        public StrikerArrow strikerArrow;
        public StrikerProperties strikerProperties;
        public AIMovement aIMovement;

        [Header(" Player Properties ")]
        // Serialized fields
        public int myId;
        public int myAvatarID = 1;
        public int myStrikerID = 1;
        public ulong myOculusID;  
        private Sprite myImage;
        public string myName;
        public string imageURL;
        public PlayerControl myPlayerControl;
        public PlayerRole myPlayerRole;
        public PlayerCoin myCoin;
        public Team myTeam;
        public AIDifficulty myAiDifficulty;
        public float Offset = 0.1f;

        // local variables
        public Action<string> aIMovementEvent;
        public Action<GameObject> strikerCreatedEvent;
        public Action<GameObject> aICreatedEvent;
        public Action<float,Vector3> strikeStartedEvent;
        public Action strikeEndedEvent;
        public Action strikeForceStartedEvent;
        public Action<float> AllCoinsRotatedEvent;



        private void OnDisable()
        {
            DeRegisterStrikerEvents();
            DeRegisterAIEvents();
        }

        public void SetProperties(PlayerProperties data)
        {
            myId = data.myId;

            myStrikerID = data.myStrikerID;
            myOculusID = data.myOculusID;

            myName = data.myName;
            imageURL = data.imageURL;
            myPlayerControl = data.myPlayerControl;
            myPlayerRole = data.myPlayerRole;
            myCoin = data.myCoin;
            myTeam = data.myTeam;

            myAiDifficulty = data.myAiDifficulty;

            gameObject.name = "Player " + myId;

          
           StartCoroutine(ConstructPlayer());
        }
    
        public void SetMyImage(Sprite image)
        {
            myImage = image;
            playersData.PlayerImageLoaded();
        }

        public Sprite GetMyImage()
        {
            return myImage;
        }

        public IEnumerator ConstructPlayer()
        {

            // change view angle and canvas
            if (myPlayerRole == PlayerRole.Human)
            {
                if (myPlayerControl == PlayerControl.Local)
                {
                    ChangePlayerView(myId); // only local player changes his 
                    SetMyImage(playerSettings.MyProfileImage);
                }
                else
                {
                    
                    ChangeRemotePlayerAvatar(myId);// remote player change
                }
            }
            else if (myPlayerRole == PlayerRole.AI)
            {
                ChangeRemoteAIAvatar(myId);
            }        

            // Define the callback here
            strikerCreatedEvent = (GameObject striker) =>
            {
                
                myStriker = striker;
                striker.transform.parent = gameObject.transform;
                strikerMovement = striker.GetComponent<StrikerMovement>();
                strikerShoot = striker.GetComponent<StrikerShooting>();
                strikerArrow = striker.GetComponent<StrikerArrow>();
                strikerRigidBody = striker.GetComponent<Rigidbody>();
                strikerProperties = striker.GetComponent<StrikerProperties>();
                RegisterStrikerEvents();
            };

            strikerData.CreateStriker(myId, myStrikerID, strikerCreatedEvent);

            yield return new WaitForSeconds(0.1f);

            // Define the callback here
            aICreatedEvent = (GameObject aIAvatar) =>
            {
                
                myAvatar = aIAvatar;
                myAvatar.transform.parent = gameObject.transform;
               
                aIMovement = myAvatar.GetComponent<AIMovement>();
                SetMyImage(aIMovement.AIIcon);
                RegisterAIEvents();
            };

            // setting avatar properties
            if (myPlayerRole == PlayerRole.AI)
            {
                aiData.CreateBot(myStriker,myId, myAiDifficulty,aICreatedEvent);
               
            }

        }

        public void ChangeRemotePlayerAvatar(int id)
        {
            transform.position = boardData.GetPlayerPosition(id).position+ new Vector3(0,Offset,0);
            transform.rotation = boardData.GetPlayerPosition(id).rotation;
                        
        }

        public void ChangeRemoteAIAvatar(int id)
        {
            transform.position = boardData.GetPlayerPosition(id).position;
            transform.rotation = boardData.GetPlayerPosition(id).rotation;

        }

        public void ChangePlayerView(int id)
        {

            transform.position = boardData.GetPlayerPosition(id).position;
            transform.rotation = boardData.GetPlayerPosition(id).rotation;

       
            camPositionData.SetCamPosition(id);

        }

        private void RegisterStrikerEvents()
        {
            if(myStriker != null)
            {
                strikerShoot.StrikeStartedEvent += StrikeStarted;
                strikerShoot.StrikeFinishedEvent += StrikeFinished;
                strikerShoot.StrikeForceStartedEvent += StrikeForceStarted;
            }
        }

        private void DeRegisterStrikerEvents()
        {
            if (myStriker != null)
            {
                strikerShoot.StrikeStartedEvent -= StrikeStarted;
                strikerShoot.StrikeFinishedEvent -= StrikeFinished;
                strikerShoot.StrikeForceStartedEvent -= StrikeForceStarted;
            }
        }

        private void RegisterAIEvents()
        {
            if(aIMovement != null)
            {
                aIMovement.AIBotAnimationEvent += AIMoved;
            }
        }

        private void DeRegisterAIEvents() 
        {
            if (aIMovement != null)
            {
                aIMovement.AIBotAnimationEvent -= AIMoved;
            }
        }

        private void StrikeForceStarted()
        {
            strikeForceStartedEvent?.Invoke();
        }


        public void StrikeStarted(float a, Vector3 dir)
        {
            strikeStartedEvent?.Invoke(a,dir);
            playersData.PlayerStrikeStartedEvent?.Invoke(myId, a);
        }

        public void StrikeFinished()
        {
            strikeEndedEvent?.Invoke();
            playersData.PlayerStrikeFinishedEvent?.Invoke(myId);
        }

        private void AIMoved(string data)
        {
            aIMovementEvent?.Invoke(data);
         
        }


        // Fetch Remote player Image
        public IEnumerator FetchAvatarImageURL(string imageUrl)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(imageUrl))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Failed to fetch avatar image URL: " + www.error);
                    yield break;
                }

                string avatarImageUrl = www.url;
                // Use the avatar image URL as needed (e.g., load the image using UnityWebRequest)
                StartCoroutine(LoadAvatarImage(avatarImageUrl));
            }
        }

        private IEnumerator LoadAvatarImage(string url)
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

                SetMyImage(s);
            }
        }
    }

}

