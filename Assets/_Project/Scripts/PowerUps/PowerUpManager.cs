using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    public List<PowerUp> allPowerUps;
    public GameObject panelUI;
    public Button[] optionButtons;

    private PowerUp[] currentChoices;
    private Action onPowerUpChosen;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        panelUI.SetActive(false);
    }

    public void ShowPowerUpChoices(Action onChosenCallback)
    {
        panelUI.SetActive(true);
        Time.timeScale = 0f;
        currentChoices = new PowerUp[3];
        onPowerUpChosen = onChosenCallback;

        List<int> usedIndices = new List<int>();

        for (int i = 0; i < 3; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = UnityEngine.Random.Range(0, allPowerUps.Count);
            } while (usedIndices.Contains(randomIndex));
            usedIndices.Add(randomIndex);

            currentChoices[i] = allPowerUps[randomIndex];
            optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentChoices[i].powerUpName;

            int index = i;
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => SelectPowerUp(index));
        }
    }

    public void SelectPowerUp(int index)
    {
        PlayerStats stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        if (stats != null)
        {
            currentChoices[index].Apply(stats);

            // ✅ Salva nome no PowerUpStorage
            PowerUpStorage.Instance?.AddPowerUp(currentChoices[index].powerUpName);
        }
        else
        {
            Debug.LogError("PlayerStats não encontrado no Player!");
        }

        panelUI.SetActive(false);
        Time.timeScale = 1f;
        onPowerUpChosen?.Invoke();
    }
}