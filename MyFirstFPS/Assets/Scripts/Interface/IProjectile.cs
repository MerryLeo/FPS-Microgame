using UnityEngine;
public interface IProjectile
{
    void SendProjectile(Vector3 direction, Vector3 position);
    void SendProjectile(Vector3 direction, Vector3 position, float speed);
}
