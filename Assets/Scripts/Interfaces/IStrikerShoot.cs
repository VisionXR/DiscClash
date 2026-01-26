using UnityEngine;

public interface IStrikerShoot
{

    public void FireStriker();
    public void SetStrikerForce(float normalisedForce);
    public void FireStriker(Vector3 direction, float force);
}
