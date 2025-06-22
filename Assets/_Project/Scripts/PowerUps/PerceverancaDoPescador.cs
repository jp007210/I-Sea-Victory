using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Perseverança do Pescador")]
public class PassiveHeal : PowerUp
{
    public int healAmount = 1;
    public float healInterval = 2f;

    public override void Apply(PlayerStats playerStats)
    {
        playerStats.EnablePassiveHeal(healAmount, healInterval);
    }
}