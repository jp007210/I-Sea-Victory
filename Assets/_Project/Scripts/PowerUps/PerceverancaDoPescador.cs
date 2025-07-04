using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Perseveranša do Pescador")]
public class PerseverancaDoPescador : PowerUp
{
    public int healAmount = 1;
    public float healInterval = 2f;

    public override void Apply(PlayerStats playerStats)
    {
        playerStats.EnablePassiveHeal(healAmount, healInterval);
    }
}