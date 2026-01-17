using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using UnityEngine;

public interface IAIBehaviour
{
    public void ExecuteShot(PlayerCoin coin);
    public void SetStriker(GameObject striker , int MyId);

}
