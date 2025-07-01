using System;
using UnityEngine;

public enum PowerUpType
{
    Damage,
    Defense,
    ProjectileSize,
    MaxHealth,
    MoveSpeed,
    CooldownReduction,
    PassiveHealing,
    ExtraProjectile,
    Shield
}
public abstract class PowerUp : ScriptableObject
{
    public PowerUpType powerUpType;
    public string powerUpName;

    [TextArea(3, 6)]
    public string description;

    public Sprite icon;
    public float value;
    public float duration;

    public abstract void Apply(PlayerStats playerStats);

    internal void Apply(GameObject gameObject)
    {
        throw new NotImplementedException();
    }
}