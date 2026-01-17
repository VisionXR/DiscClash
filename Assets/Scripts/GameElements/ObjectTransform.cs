using UnityEngine;

public class ObjectTransform : MonoBehaviour,ITransform
{ 
    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }

    public Vector3 GetRotation()
    {
        return gameObject.transform.eulerAngles;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetRotation(Vector3 eulerAngles)
    {
        transform.eulerAngles = eulerAngles;
    }

}
