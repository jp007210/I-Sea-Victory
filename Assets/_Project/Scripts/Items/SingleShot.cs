using System.Collections.Generic;
using UnityEngine;

public class SingleShot : Weapon
{
    public override List<ShotData> GetShots(Vector3 firePointPosition, Transform weaponTransform)
    {
        List<ShotData> shots = new List<ShotData>();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, firePointPosition);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            Vector3 direction = (targetPoint - firePointPosition).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);

            shots.Add(new ShotData
            {
                position = firePointPosition,
                rotation = rotation,
                direction = direction
            });
        }

        return shots;
    }
}