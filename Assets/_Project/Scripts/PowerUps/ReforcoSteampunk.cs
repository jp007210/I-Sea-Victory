using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Reforço SteamPunk")]

public class ReforcoSteampunk : PowerUp
{
    public float defenseBonus = 1.2f; // 20% a mais

    public override void Apply(PlayerStats playerStats)
    {
        playerStats.baseDamageReduction *= defenseBonus;
    }
}