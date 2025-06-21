using UnityEngine;
using System;
using TMPro;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance;

    [Header("UI Reference")]
    public TMP_Dropdown languageDropdown;

    public enum Language { English, Portuguese }
    public Language CurrentLanguage { get; private set; }

    public static event Action OnLanguageChanged;

    private const string LanguageKey = "SelectedLanguage";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadLanguage();
    }

    private void Start()
    {
        if (languageDropdown != null)
        {
            languageDropdown.onValueChanged.RemoveAllListeners();
            languageDropdown.onValueChanged.AddListener(ChangeLanguage);
        }

        UpdateDropdownSelection();
    }

    public void ChangeLanguage(int languageIndex)
    {
        Language newLanguage = (Language)languageIndex;

        if (newLanguage == CurrentLanguage) return;

        CurrentLanguage = newLanguage;
        PlayerPrefs.SetInt(LanguageKey, (int)CurrentLanguage);
        PlayerPrefs.Save();

        OnLanguageChanged?.Invoke();
    }

    private void LoadLanguage()
    {
        int savedLangIndex = PlayerPrefs.GetInt(LanguageKey, 0);
        CurrentLanguage = (Language)savedLangIndex;
    }

    private void UpdateDropdownSelection()
    {
        if (languageDropdown != null)
        {
            languageDropdown.SetValueWithoutNotify((int)CurrentLanguage);
        }
    }
}
