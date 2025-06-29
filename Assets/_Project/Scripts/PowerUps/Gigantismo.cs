using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Gigantismo")]
public class Gigantismo : PowerUp
{
    public float sizeBonus = 1.5f;

    public override void Apply(PlayerStats playerStats)
    {
        playerStats.projectileSizeMultiplier *= sizeBonus;
    }
}