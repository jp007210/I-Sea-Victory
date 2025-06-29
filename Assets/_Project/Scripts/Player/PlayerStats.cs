using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [Header("Vida")]
    public int baseMaxHealth = 100;
    public int currentHealth;

    [Header("UI de Vida")]
    public Slider healthBarSlider;
    public Image damageGlowImage;
    public Color damageGlowColor = new Color(0.5f, 0, 0, 0.7f);
    public float glowDuration = 0.4f;
    public float healthAnimationDuration = 0.3f;
    public System.Action<int, int> UpdateHealthUI;

    [Header("Cura Passiva")]
    private bool passiveHealEnabled = false;
    private int passiveHealAmount = 0;
    private float passiveHealInterval = 0f;
    private float passiveHealTimer = 0f;

    [Header("Modificadores base")]
    public float baseDamageMultiplier = 1f;       // base para multiplicar dano do player
    public float baseDamageReduction = 1f;        // base para reduzir dano recebido (1 = sem redução)
    public float baseHealingPerSecond = 0f;       // base de cura passiva
    public float baseMoveSpeed = 5f;
    public float baseCooldownMultiplier = 1f;

    [Header("Modificadores de Projéteis")]
    public float projectileSizeMultiplier = 1f;
    public int extraProjectiles = 0;

    [Header("Status atualizados")]
    public float damageMultiplier;
    public float damageReduction;
    public float healingPerSecond;
    public float moveSpeed;
    public float cooldownMultiplier;

    [Header("Escudo")]
    public bool hasShield = false;
    public float shieldCooldown = 5f;
    private float shieldTimer = 0f;
    private bool shieldActive = false;

    private Coroutine healthAnimationCoroutine;
    private Coroutine glowAnimationCoroutine;

    void Awake()
    {
        ResetStatsToBase();

        currentHealth = baseMaxHealth;

        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = baseMaxHealth;
            healthBarSlider.value = currentHealth;
        }

        if (damageGlowImage != null)
        {
            damageGlowImage.color = new Color(damageGlowColor.r, damageGlowColor.g, damageGlowColor.b, 0);
        }

        // ✅ Aplica todos os power ups salvos
        if (PowerUpStorage.Instance != null)
        {
            foreach (var powerUpName in PowerUpStorage.Instance.GetAllUnlockedPowerUps())
            {
                PowerUp powerUpAsset = FindPowerUpByName(powerUpName);
                if (powerUpAsset != null)
                {
                    powerUpAsset.Apply(this);
                    Debug.Log($"[PowerUp reaplicado] {powerUpAsset.powerUpName}");
                }
                else
                {
                    Debug.LogWarning($"PowerUp '{powerUpName}' não encontrado!");
                }
            }
        }
    }

    void Update()
    {
        if (passiveHealEnabled)
        {
            passiveHealTimer += Time.deltaTime;
            if (passiveHealTimer >= passiveHealInterval)
            {
                passiveHealTimer = 0f;
                Heal(passiveHealAmount);
            }
        }
        RechargeShield(Time.deltaTime);
    }
    PowerUp FindPowerUpByName(string name)
    {
        foreach (var pu in PowerUpManager.Instance.allPowerUps)
        {
            if (pu.powerUpName == name)
                return pu;
        }
        return null;
    }

    public void ResetStatsToBase()
    {
        damageMultiplier = baseDamageMultiplier;
        damageReduction = baseDamageReduction;
        healingPerSecond = baseHealingPerSecond;
        moveSpeed = baseMoveSpeed;
        cooldownMultiplier = baseCooldownMultiplier;
        hasShield = false;
        shieldTimer = 0f;
        shieldActive = false;
    }
    public void EnablePassiveHeal(int amount, float interval)
    {
        passiveHealEnabled = true;
        passiveHealAmount = amount;
        passiveHealInterval = interval;
        passiveHealTimer = 0f;
    }

    public void ApplyPowerUp(PowerUp powerUp)
    {
        switch (powerUp.powerUpType)
        {
            case PowerUpType.Damage:
                damageMultiplier += powerUp.value;
                break;

            case PowerUpType.Defense:
                damageReduction -= powerUp.value; // espera-se valor pequeno (ex: 0.1 = 10% menos dano)
                damageReduction = Mathf.Clamp(damageReduction, 0f, 1f);
                break;

            case PowerUpType.ProjectileSize:
                // Pode ser tratado externamente em armas/projetéis, apenas guarde o valor
                // Por exemplo: adicionar um campo neste PlayerStats para usar depois
                break;

            case PowerUpType.MaxHealth:
                baseMaxHealth += Mathf.RoundToInt(powerUp.value);
                currentHealth += Mathf.RoundToInt(powerUp.value);
                if (healthBarSlider != null)
                    healthBarSlider.maxValue = baseMaxHealth;
                break;

            case PowerUpType.MoveSpeed:
                moveSpeed += powerUp.value;
                break;

            case PowerUpType.CooldownReduction:
                cooldownMultiplier -= powerUp.value; // valor esperado: 0.1 = 10% menos cooldown
                cooldownMultiplier = Mathf.Clamp(cooldownMultiplier, 0.1f, 1f);
                break;

            case PowerUpType.PassiveHealing:
                healingPerSecond += powerUp.value;
                break;

            case PowerUpType.ExtraProjectile:
                // Guarda um valor para ser usado no sistema de armas (arma checar PlayerStats)
                // Pode guardar em uma variável aqui para o sistema de armas consultar
                break;

            case PowerUpType.Shield:
                hasShield = true;
                shieldCooldown = powerUp.duration > 0 ? powerUp.duration : shieldCooldown;
                shieldActive = true;
                break;
        }

        Debug.Log($"PowerUp aplicado: {powerUp.powerUpName} - Valor: {powerUp.value}");
    }
    public void IncreaseMaxHealth(int amount)
    {
        baseMaxHealth += amount;
        UpdateHealthUI?.Invoke(baseMaxHealth, currentHealth);
    }

    public void TakeDamage(int amount)
    {
        int finalDamage = Mathf.CeilToInt(amount * damageReduction);

        if (hasShield && shieldActive)
        {
            shieldActive = false;
            shieldTimer = 0f;
            Debug.Log("Escudo bloqueou o dano!");
            StartDamageGlow();
            return;
        }

        currentHealth -= finalDamage;
        currentHealth = Mathf.Max(currentHealth, 0);

        StartHealthAnimation();
        StartDamageGlow();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, baseMaxHealth);
        StartHealthAnimation();
    }

    public void PassiveHeal(float deltaTime)
    {
        if (healingPerSecond > 0f && currentHealth < baseMaxHealth)
        {
            Heal(Mathf.FloorToInt(healingPerSecond * deltaTime));
        }
    }

    public void RechargeShield(float deltaTime)
    {
        if (hasShield && !shieldActive)
        {
            shieldTimer += deltaTime;
            if (shieldTimer >= shieldCooldown)
            {
                shieldActive = true;
                Debug.Log("Escudo recarregado!");
            }
        }
    }

    void StartHealthAnimation()
    {
        if (healthAnimationCoroutine != null) StopCoroutine(healthAnimationCoroutine);
        healthAnimationCoroutine = StartCoroutine(AnimateHealthBar());
    }

    IEnumerator AnimateHealthBar()
    {
        if (healthBarSlider == null) yield break;

        float startValue = healthBarSlider.value;
        float targetValue = currentHealth;
        float timer = 0f;

        while (timer < healthAnimationDuration)
        {
            healthBarSlider.value = Mathf.Lerp(startValue, targetValue, timer / healthAnimationDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        healthBarSlider.value = targetValue;
    }

    void StartDamageGlow()
    {
        if (glowAnimationCoroutine != null) StopCoroutine(glowAnimationCoroutine);
        glowAnimationCoroutine = StartCoroutine(DamageGlowEffect());
    }

    IEnumerator DamageGlowEffect()
    {
        if (damageGlowImage == null) yield break;

        damageGlowImage.color = damageGlowColor;

        float timer = 0f;
        while (timer < glowDuration)
        {
            float alpha = Mathf.Lerp(damageGlowColor.a, 0f, timer / glowDuration);
            damageGlowImage.color = new Color(damageGlowColor.r, damageGlowColor.g, damageGlowColor.b, alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        damageGlowImage.color = new Color(damageGlowColor.r, damageGlowColor.g, damageGlowColor.b, 0);
    }

    void Die()
    {
        Debug.Log("Player morreu!");
        gameObject.SetActive(false);

        if (GameOverScreen.Instance != null)
        {
            GameOverScreen.Instance.ShowGameOver();
        }
        else
        {
            Time.timeScale = 0f;
        }
    }
}