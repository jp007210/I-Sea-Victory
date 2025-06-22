using UnityEngine;
using System.Collections.Generic;
using static Weapon;

public class Tentaculo : Weapon
{
    public GameObject tentaclePrefab;
    public float radius = 5f;
    public float pushForce = 10f;
    public float duration = 1f;

    public override List<ShotData> GetShots(Vector3 firePointPosition, Transform weaponTransform)
    {
        GameObject effect = Instantiate(tentaclePrefab, weaponTransform.position, Quaternion.identity);
        TentaculoAOE aoe = effect.GetComponent<TentaculoAOE>();
        if (aoe != null)
        {
            aoe.Activate(radius, damage, pushForce, hitLayers, duration);
        }

        return new List<ShotData>(); // não retorna projéteis
    }
}
