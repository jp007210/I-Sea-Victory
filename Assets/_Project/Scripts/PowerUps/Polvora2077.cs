using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/P�lvora de 2077")]
public class Polvora2077 : PowerUp
{
    public float damageBonus = 1.2f;

    public override void Apply(PlayerStats playerStats)
    {
        playerStats.damageMultiplier *= damageBonus;
    }
}