using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class StrikerManager : MonoBehaviour
    {
       
        [Header("Scriptable Objects")]
        public StrikerDataSO strikerData;
        private void OnEnable()
        {         
            strikerData.CreateStrikerEvent += CreateStriker;
            strikerData.DestroyStrikerEvent += DestroyStriker;
            strikerData.StrikerFellInHoleEvent += OnFoulOccured;
        }

        private void OnDisable()
        {         
            strikerData.CreateStrikerEvent -= CreateStriker;
            strikerData.DestroyStrikerEvent -= DestroyStriker;
            strikerData.StrikerFellInHoleEvent -= OnFoulOccured;
        }

        private void OnFoulOccured(GameObject striker)
        {           
            strikerData.SetFoul();
        }

        public void CreateStriker(int playerId, int strikerId, Action<GameObject> strikerCreatedEvent)
        {
            // Build the resource path, e.g., "Strikers/Striker0"
            string resourcePath = $"Strikers/Striker{strikerId}";

            GameObject strikerPrefab = Resources.Load<GameObject>(resourcePath);

            if (strikerPrefab != null)
            {
                GameObject striker = Instantiate(strikerPrefab, strikerPrefab.transform.position, strikerPrefab.transform.rotation);
                striker.GetComponent<StrikerMovement>().SetStrikerID(playerId);
                strikerCreatedEvent?.Invoke(striker);
            }
            else
            {
                Debug.Log($"Striker prefab not found at Resources/{resourcePath}");
                strikerCreatedEvent?.Invoke(null);
            }
        }

        public void DestroyStriker(int id)
        {
            foreach(GameObject striker in strikerData.AvailableStrikersinGame)
            {
                if(striker.GetComponent<IStrikerMovement>().GetStrikerId() == id)
                {
                    Destroy(striker);
                    break;
                }
            }
        }
       
    }
}

