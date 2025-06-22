using System.Collections.Generic;
using UnityEngine;

public class PowerUpStorage : MonoBehaviour
{
    public static PowerUpStorage Instance;

    private const string PlayerPrefsKey = "UnlockedPowerUps";

    // Guarda os nomes (IDs) dos power ups escolhidos
    private HashSet<string> unlockedPowerUps = new HashSet<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public IEnumerable<string> GetAllUnlockedPowerUps()
    {
        return unlockedPowerUps;
    }

    // Adiciona um power up à lista e salva
    public void AddPowerUp(string powerUpName)
    {
        if (!unlockedPowerUps.Contains(powerUpName))
        {
            unlockedPowerUps.Add(powerUpName);
            SaveData();
        }
    }

    // Verifica se o power up já foi desbloqueado
    public bool IsUnlocked(string powerUpName)
    {
        return unlockedPowerUps.Contains(powerUpName);
    }

    // Salva a lista como string serializada no PlayerPrefs
    private void SaveData()
    {
        string serialized = string.Join(",", unlockedPowerUps);
        PlayerPrefs.SetString(PlayerPrefsKey, serialized);
        PlayerPrefs.Save();
    }

    // Carrega a lista do PlayerPrefs
    private void LoadData()
    {
        unlockedPowerUps.Clear();

        if (PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            string serialized = PlayerPrefs.GetString(PlayerPrefsKey);
            string[] powerUps = serialized.Split(',');
            foreach (var p in powerUps)
            {
                if (!string.IsNullOrEmpty(p))
                    unlockedPowerUps.Add(p);
            }
        }
    }
}