using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Arsenal Etéreo")]
public class ArsenalEtereo : PowerUp
{
    public int extraCount = 1;

    public override void Apply(PlayerStats playerStats)
    {
        playerStats.extraProjectiles += extraCount;
    }
}