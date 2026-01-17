using UnityEngine;

public interface IStrikerShoot
{

    public void FireStriker();
    public void FireStriker(float force);
    public void FireStriker(Vector3 direction, float force);
}
