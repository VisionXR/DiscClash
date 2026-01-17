using com.VisionXR.ModelClasses;
using UnityEngine;
using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;

public class AIManagerTest : MonoBehaviour
{
    public AIBotAnimationDetails animationDetails = new AIBotAnimationDetails();
    public AIDataSO aIdata;
    public int id;
    public AIDifficulty difficulty;
    public Player p;
    public GameDataSO data;
    public GameObject Striker, Coin;
    
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
           
            data.ChangeTurn(2);
        }
     
     
    }
}
