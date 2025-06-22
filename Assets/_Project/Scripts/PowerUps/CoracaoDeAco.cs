using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Coração de Aço")]
public class MaxHealthBoost : PowerUp
{
    public int bonusHealth = 20;

    public override void Apply(PlayerStats playerStats)
    {
        playerStats.IncreaseMaxHealth(bonusHealth);
    }
}