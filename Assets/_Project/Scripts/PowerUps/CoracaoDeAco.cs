using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Cora��o de A�o")]
public class CoracaoDeAco : PowerUp
{
    public int bonusHealth = 20;

    public override void Apply(PlayerStats playerStats)
    {
        playerStats.IncreaseMaxHealth(bonusHealth);
    }
}