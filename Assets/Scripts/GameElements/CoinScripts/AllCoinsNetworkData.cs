using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using Fusion;


public class AllCoinsNetworkData : NetworkBehaviour
{
    public AllCoinsRotation coinRotation;
    public CoinDataSO coinData;
  
    [Networked, OnChangedRender(nameof(OnAllCoinsRotationReceived))] public float allCoinsRotationData { get; set; }


    private void OnEnable()
    {
        coinData.AllCoinsReference = gameObject;
    }

    private void OnDisable()
    {
        coinData.AllCoinsReference = null;
    }
    public void SetAllCoinsRotationEvent(float value)
    {
        if (HasStateAuthority)
        {
            allCoinsRotationData = value;
        }
    }

    private void OnAllCoinsRotationReceived()
    {
        if (!HasStateAuthority)
        {
            coinRotation.SetRotation(allCoinsRotationData);
        }
    }

}
