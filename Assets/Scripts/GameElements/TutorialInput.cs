using com.VisionXR.ModelClasses;
using UnityEngine;


namespace com.VisionXR.GameElements
{
    public class TutorialInput : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public InputDataSO inputData;
        public TutorialDataSO tutorialData;
        public BoardDataSO boardData;
        public StrikerDataSO strikerData;

        [Header("Game Objects")]
        public GameObject striker;
        public StrikerShooting strikerShooting;
        public StrikerMovement strikerMovement;



        private void OnEnable()
        {
            inputData.FireStrikeEvent += FireStriker;
            inputData.RotateStrikerAbsoluteEvent += RotateStriker;

            inputData.MoveStrikerEvent += MoveStriker;
            inputData.StrikerForceChangedEvent += StrikerForceChanged;
        }


        private void OnDisable()
        {
            inputData.FireStrikeEvent -= FireStriker;
            inputData.RotateStrikerAbsoluteEvent -= RotateStriker;

            inputData.MoveStrikerEvent -= MoveStriker;
            inputData.StrikerForceChangedEvent -= StrikerForceChanged;

        }

        private void MoveStriker(float delta)
        {
            if (tutorialData.canIPosition)
            {
                strikerMovement.MoveStriker(delta);
            }
        }

        private void StrikerForceChanged(float obj)
        {
            strikerShooting.SetStrikerForce(obj);
        }

        private void RotateStriker(float angle)
        {
            if (tutorialData.canIAim)
            {
                strikerMovement.AimStriker(angle);
            }
        }

        private void FireStriker(float val)
        {
            if (tutorialData.canIFire)
            {
                strikerShooting.FireStriker(val);
            }
        }

    }
}
