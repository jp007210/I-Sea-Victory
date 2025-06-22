using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Prote��o Astral")]
public class ShieldPowerUp : PowerUp
{
    public float rechargeTime = 10f;

    public override void Apply(PlayerStats playerStats)
    {
        playerStats.RechargeShield(rechargeTime);
    }
}
