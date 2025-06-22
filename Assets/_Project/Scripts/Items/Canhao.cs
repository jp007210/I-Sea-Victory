using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Canhao : Weapon
{
    public int cannonCountPerSide = 3;
    public float spacing = 1.5f;

    public override List<ShotData> GetShots(Vector3 firePointPosition, Transform weaponTransform)
    {
        List<ShotData> shots = new List<ShotData>();

        Vector3 right = weaponTransform.right;
        Vector3 basePos = firePointPosition;

        // Lado direito
        Vector3 startPosRight = basePos + right * spacing * (cannonCountPerSide - 1) / 2f;
        for (int i = 0; i < cannonCountPerSide; i++)
        {
            Vector3 pos = startPosRight - right * spacing * i;
            pos.y = basePos.y;
            Vector3 dir = right;
            Quaternion rot = Quaternion.LookRotation(dir);
            shots.Add(new ShotData { position = pos, rotation = rot, direction = dir });
        }

        // Lado esquerdo
        Vector3 startPosLeft = basePos - right * spacing * (cannonCountPerSide - 1) / 2f;
        for (int i = 0; i < cannonCountPerSide; i++)
        {
            Vector3 pos = startPosLeft + right * spacing * i;
            pos.y = basePos.y;
            Vector3 dir = -right;
            Quaternion rot = Quaternion.LookRotation(dir);
            shots.Add(new ShotData { position = pos, rotation = rot, direction = dir });
        }

        return shots;
    }
}