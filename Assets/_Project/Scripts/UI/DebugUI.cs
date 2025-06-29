using UnityEngine;
using TMPro;

public class DebugUI : MonoBehaviour
{
    public PlayerStats player;
    public TextMeshProUGUI debugText;

    void Update()
    {
        if (player == null || debugText == null) return;

        debugText.text =
            $"<b>[DEBUG STATS]</b>\n" +
            $"Vida: {player.currentHealth} / {player.baseMaxHealth}\n" +
            $"Dano x: {player.damageMultiplier:F2}\n" +
            $"Redu��o de Dano: {player.damageReduction:P0}\n" +
            $"Cura/s: {player.healingPerSecond:F2}\n" +
            $"Velocidade: {player.moveSpeed:F2}\n" +
            $"Cooldown x: {player.cooldownMultiplier:F2}\n" +
            $"Proj�teis Extras: {player.extraProjectiles}\n" +
            $"Tamanho Proj�til x: {player.projectileSizeMultiplier:F2}\n" +
            $"Escudo Ativo: {(player.hasShield ? "Sim" : "N�o")}";
    }
}