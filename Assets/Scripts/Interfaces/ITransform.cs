using UnityEngine;
public interface ITransform
{
    public void SetPosition(Vector3 position);
    public void SetRotation(Vector3 eulerAngles);
    public Vector3 GetPosition();
    public Vector3 GetRotation();
}
