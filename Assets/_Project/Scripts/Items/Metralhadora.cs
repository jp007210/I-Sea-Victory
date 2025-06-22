using System.Collections.Generic;
using UnityEngine;

public class Metralhadora : Weapon
{
    public override List<ShotData> GetShots(Vector3 firePointPosition, Transform weaponTransform)
    {
        Vector3 direction = GetMouseDirection(firePointPosition);
        Quaternion rotation = Quaternion.LookRotation(direction);

        List<ShotData> shots = new List<ShotData>
        {
            new ShotData
            {
                position = firePointPosition,
                rotation = rotation,
                direction = direction
            }
        };

        return shots;
    }

    Vector3 GetMouseDirection(Vector3 origin)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, origin);
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 target = ray.GetPoint(distance);
            return (target - origin).normalized;
        }

        return Vector3.forward;
    }
}
