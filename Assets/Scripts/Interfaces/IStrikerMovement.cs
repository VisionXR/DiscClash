using UnityEngine;

public interface IStrikerMovement 
{
   
    public void SetStrikerID(int id);
    public int GetStrikerId();
    public void AimStriker(Vector3 direction);
   

    public void ResetStriker();

   
}
