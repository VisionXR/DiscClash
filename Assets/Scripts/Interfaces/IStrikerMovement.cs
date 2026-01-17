using com.VisionXR.HelperClasses;
using System.Collections.Generic;
using UnityEngine;

public interface IStrikerMovement 
{
   
    public void SetStrikerID(int id);
    public int GetStrikerId();

    public void MoveStriker(Vector3 controllerPosition, Transform cameraRigTransform);
    public void MoveStriker(SwipeDirection swipeDirection);
    public void AimStriker(float yAngle);
    public void AimStriker(SwipeDirection swipeDirection);

    public void RotateTo(Vector3 pos);

    public Vector3 FindStrikerNextPosition(Vector3 finalPosition, Vector3 direction);

    public void ResetStriker();

   
}
