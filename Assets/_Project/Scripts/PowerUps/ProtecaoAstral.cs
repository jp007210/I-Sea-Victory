using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Proteção Astral")]
public class ProtecaoAstral : PowerUp
{
    public float rechargeTime = 10f;

    public override void Apply(PlayerStats playerStats)
    {
        playerStats.RechargeShield(rechargeTime);
    }
}
