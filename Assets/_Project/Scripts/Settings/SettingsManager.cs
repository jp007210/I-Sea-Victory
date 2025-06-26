using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("Audio UI References")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Video UI References")]
    public Slider brightnessSlider;
    public Image brightnessPanel;
    public TMP_Dropdown languageDropdown;

    [Header("Resolution Settings")]
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    // Eventos para notificar mudanças
    public static event Action OnSettingsChanged;

    // Chaves para PlayerPrefs
    private const string BrightnessKey = "MasterBrightness";
    private const string ResolutionKey = "Resolution";
    private const string FullscreenKey = "Fullscreen";
    private const string LanguageKey = "SelectedLanguage";

    // Valores padrão
    private const float DEFAULT_BRIGHTNESS = 1f;
    private const float DEFAULT_VOLUME = 0.8f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeSettings();
        LoadAllSettings();
    }

    private void InitializeSettings()
    {
        // Configurar listeners para sliders de áudio
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.onValueChanged.RemoveAllListeners();
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.RemoveAllListeners();
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.RemoveAllListeners();
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        // Configurar listeners para configurações de vídeo
        if (brightnessSlider != null)
        {
            brightnessSlider.onValueChanged.RemoveAllListeners();
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
        }

        if (languageDropdown != null)
        {
            languageDropdown.onValueChanged.RemoveAllListeners();
            languageDropdown.onValueChanged.AddListener(SetLanguage);
        }

        if (resolutionDropdown != null)
        {
            resolutionDropdown.onValueChanged.RemoveAllListeners();
            resolutionDropdown.onValueChanged.AddListener(SetResolution);
            PopulateResolutionDropdown();
        }

        if (fullscreenToggle != null)
        {
            fullscreenToggle.onValueChanged.RemoveAllListeners();
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        }
    }

    private void LoadAllSettings()
    {
        LoadAudioSettings();
        LoadVideoSettings();
        LoadLanguageSettings();
    }

    #region Audio Settings
    public void SetMasterVolume(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMasterVolume(value);
        }
        OnSettingsChanged?.Invoke();
    }

    public void SetMusicVolume(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(value);
        }
        OnSettingsChanged?.Invoke();
    }

    public void SetSFXVolume(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(value);
        }
        OnSettingsChanged?.Invoke();
    }

    private void LoadAudioSettings()
    {
        if (AudioManager.Instance != null)
        {
            if (masterVolumeSlider != null)
            {
                float masterVolume = AudioManager.Instance.GetMasterVolume();
                masterVolumeSlider.SetValueWithoutNotify(masterVolume);
            }

            if (musicVolumeSlider != null)
            {
                float musicVolume = AudioManager.Instance.GetMusicVolume();
                musicVolumeSlider.SetValueWithoutNotify(musicVolume);
            }

            if (sfxVolumeSlider != null)
            {
                float sfxVolume = AudioManager.Instance.GetSFXVolume();
                sfxVolumeSlider.SetValueWithoutNotify(sfxVolume);
            }
        }
    }
    #endregion

    #region Video Settings
    public void SetBrightness(float value)
    {
        if (brightnessPanel != null)
        {
            float panelAlpha = (1 - value) * 0.7f;
            Color panelColor = brightnessPanel.color;
            panelColor.a = panelAlpha;
            brightnessPanel.color = panelColor;
        }

        PlayerPrefs.SetFloat(BrightnessKey, value);
        PlayerPrefs.Save();
        OnSettingsChanged?.Invoke();
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutionDropdown != null && resolutionIndex < Screen.resolutions.Length)
        {
            Resolution resolution = Screen.resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

            PlayerPrefs.SetInt(ResolutionKey, resolutionIndex);
            PlayerPrefs.Save();
            OnSettingsChanged?.Invoke();
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt(FullscreenKey, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
        OnSettingsChanged?.Invoke();
    }

    private void PopulateResolutionDropdown()
    {
        if (resolutionDropdown == null) return;

        resolutionDropdown.ClearOptions();

        foreach (Resolution resolution in Screen.resolutions)
        {
            string resolutionText = resolution.width + " x " + resolution.height;
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolutionText));
        }

        resolutionDropdown.RefreshShownValue();
    }

    private void LoadVideoSettings()
    {
        // Carregar brilho
        float savedBrightness = PlayerPrefs.GetFloat(BrightnessKey, DEFAULT_BRIGHTNESS);
        if (brightnessSlider != null)
        {
            brightnessSlider.SetValueWithoutNotify(savedBrightness);
        }
        SetBrightness(savedBrightness);

        // Carregar resolução
        int savedResolution = PlayerPrefs.GetInt(ResolutionKey, Screen.resolutions.Length - 1);
        if (resolutionDropdown != null)
        {
            resolutionDropdown.SetValueWithoutNotify(savedResolution);
        }

        // Carregar fullscreen
        bool savedFullscreen = PlayerPrefs.GetInt(FullscreenKey, 1) == 1;
        if (fullscreenToggle != null)
        {
            fullscreenToggle.SetIsOnWithoutNotify(savedFullscreen);
        }
        Screen.fullScreen = savedFullscreen;
    }
    #endregion

    #region Language Settings
    public void SetLanguage(int languageIndex)
    {
        if (LocalizationManager.instance != null)
        {
            LocalizationManager.instance.ChangeLanguage(languageIndex);
        }
        OnSettingsChanged?.Invoke();
    }

    private void LoadLanguageSettings()
    {
        if (languageDropdown != null && LocalizationManager.instance != null)
        {
            int savedLanguage = PlayerPrefs.GetInt(LanguageKey, 0);
            languageDropdown.SetValueWithoutNotify(savedLanguage);
        }
    }
    #endregion

    #region Utility Methods
    public void ResetToDefaults()
    {
        // Reset audio
        SetMasterVolume(DEFAULT_VOLUME);
        SetMusicVolume(DEFAULT_VOLUME);
        SetSFXVolume(DEFAULT_VOLUME);

        // Reset video
        SetBrightness(DEFAULT_BRIGHTNESS);
        SetFullscreen(true);
        if (Screen.resolutions.Length > 0)
        {
            SetResolution(Screen.resolutions.Length - 1);
        }

        // Reset language
        SetLanguage(0);

        // Update UI
        LoadAllSettings();
    }

    public void ApplySettings()
    {
        // Método para aplicar configurações se necessário
        // Por exemplo, aplicar mudanças que requerem confirmação
        OnSettingsChanged?.Invoke();
    }

    // Método para salvar todas as configurações
    public void SaveAllSettings()
    {
        PlayerPrefs.Save();
    }

    public void RefreshUI()
    {
        LoadAllSettings();
    }
    #endregion
}