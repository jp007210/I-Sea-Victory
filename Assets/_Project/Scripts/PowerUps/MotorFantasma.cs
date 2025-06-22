using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Motor Fantasma")]
public class SpeedBoost : PowerUp
{
    public float speedMultiplier = 1.3f;

    public override void Apply(PlayerStats playerStats)
    {
        playerStats.moveSpeed *= speedMultiplier;
    }
}