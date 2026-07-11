using UnityEngine;

public class Blazer : MonoBehaviour
{
    [Header("Raycast")]
    public float rayDistance = 50f;
    public LayerMask terrainMask;

    [Header("Fire Spawn")]
    public string firePoolName = "Fire";

    public float minDistanceBetweenFires = 1f;

    public LaserTurretAtk LaserTurretAtk;

    private Vector3 lastSpawnPoint;
    private bool hasLastPoint;

    private void Update()
    {
        if (LaserTurretAtk.isFiring)
        {
            ShootRay();
        }
        else
        {
            hasLastPoint = false;
        }
    }

    private void ShootRay()
    {
        Ray ray = new Ray(
            transform.position,
            transform.forward);

        if (!Physics.Raycast(
                ray,
                out RaycastHit hit,
                rayDistance,
                terrainMask))
        {
            return;
        }

        float distance =
            hasLastPoint
            ? Vector3.Distance(hit.point, lastSpawnPoint)
            : float.MaxValue;

        if (!hasLastPoint ||
            distance >= minDistanceBetweenFires)
        {
            Quaternion rotation =
                Quaternion.FromToRotation(
                    Vector3.up,
                    hit.normal);

            ObjectPoolManager.Instance.SpawnFromPool(
                firePoolName,
                hit.point,
                rotation);

            lastSpawnPoint = hit.point;
            hasLastPoint = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawRay(
            transform.position,
            transform.forward * rayDistance);
    }
}