using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Treinamento Viking")]
public class CooldownReduction : PowerUp
{
    public float cooldownMultiplier = 0.8f; // 20% mais rápido

    public override void Apply(PlayerStats playerStats)
    {
        playerStats.cooldownMultiplier *= cooldownMultiplier;
    }
}
