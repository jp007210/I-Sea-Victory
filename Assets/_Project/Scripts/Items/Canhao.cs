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

        Vector3 forward = weaponTransform.forward;   // direção que as balas vão (eixo X ou Z)
        Vector3 right = weaponTransform.right;       // eixo lateral (usado para espalhar)
        Vector3 up = Vector3.up;

        float height = firePointPosition.y;

        // Vamos espalhar as balas no eixo Z (ou seja, ao longo do eixo 'forward')
        // Direção dos tiros será ao longo do eixo right (eixo X)

        // Lado direito (balas disparando para a direita)
        Vector3 startPosRight = firePointPosition + forward * spacing * (cannonCountPerSide - 1) / 2f;

        for (int i = 0; i < cannonCountPerSide; i++)
        {
            Vector3 pos = startPosRight - forward * spacing * i;
            pos.y = height;

            Vector3 dir = right;   // tiro para a direita (eixo X)
            Quaternion rot = Quaternion.LookRotation(dir, up);

            shots.Add(new ShotData { position = pos, rotation = rot, direction = dir });
        }

        // Lado esquerdo (balas disparando para a esquerda)
        Vector3 startPosLeft = firePointPosition - forward * spacing * (cannonCountPerSide - 1) / 2f;

        for (int i = 0; i < cannonCountPerSide; i++)
        {
            Vector3 pos = startPosLeft + forward * spacing * i;
            pos.y = height;

            Vector3 dir = -right;  // tiro para a esquerda
            Quaternion rot = Quaternion.LookRotation(dir, up);

            shots.Add(new ShotData { position = pos, rotation = rot, direction = dir });
        }

        return shots;
    }
}