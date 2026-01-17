using System.IO;
using UnityEngine;

public class CaptureController : MonoBehaviour
{
    public int MaxResolution;
    void Update()
    {
        // Check if the "P" key is pressed
        if (Input.GetKeyDown("p"))
        {
            //// Call the Capture function from the I360Render class
            //byte[] capturedImage = I360Render.Capture(MaxResolution);
            //Debug.Log(capturedImage);
            //WriteToFile(capturedImage);


            // You can now use the 'capturedImage' byte array as needed.
            // For example, you can save it to a file or process it further.
        }
    }

    public void WriteToFile(byte[] imageName)
    {
        File.WriteAllBytes("C:\\Users\\vikra\\OneDrive\\Desktop\\CarromVideos\\AssetsForBuild\\image.jpg", imageName);
    }
}