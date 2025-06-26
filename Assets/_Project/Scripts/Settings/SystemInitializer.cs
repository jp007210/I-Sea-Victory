using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SystemInitializer : MonoBehaviour
{
    [Header("System Prefabs")]
    public GameObject menuManagerPrefab;
    public GameObject audioManagerPrefab;
    public GameObject settingsManagerPrefab;
    public GameObject brightnessManagerPrefab;
    public GameObject localizationManagerPrefab;

    [Header("Auto Setup")]
    public bool autoSetupOnStart = true;
    public float initializationDelay = 0.1f;

    void Awake()
    {
        // Garantir que os sistemas essenciais existam
        if (autoSetupOnStart)
        {
            StartCoroutine(InitializeSystemsWithDelay());
        }
    }

    private IEnumerator InitializeSystemsWithDelay()
    {
        yield return new WaitForSeconds(initializationDelay);
        InitializeAllSystems();
    }

    public void InitializeAllSystems()
    {
        InitializeAudioManager();
        InitializeLocalizationManager();
        InitializeBrightnessManager();
        InitializeSettingsManager();
        InitializeMenuManager();

        Debug.Log("Todos os sistemas foram inicializados com sucesso!");
    }

    private void InitializeAudioManager()
    {
        if (AudioManager.Instance == null && audioManagerPrefab != null)
        {
            GameObject audioManager = Instantiate(audioManagerPrefab);
            audioManager.name = "AudioManager";
            Debug.Log("AudioManager inicializado");
        }
    }

    private void InitializeLocalizationManager()
    {
        if (LocalizationManager.instance == null && localizationManagerPrefab != null)
        {
            GameObject localizationManager = Instantiate(localizationManagerPrefab);
            localizationManager.name = "LocalizationManager";
            Debug.Log("LocalizationManager inicializado");
        }
    }

    private void InitializeBrightnessManager()
    {
        if (BrightnessManager.Instance == null && brightnessManagerPrefab != null)
        {
            GameObject brightnessManager = Instantiate(brightnessManagerPrefab);
            brightnessManager.name = "BrightnessManager";
            Debug.Log("BrightnessManager inicializado");
        }
    }

    private void InitializeSettingsManager()
    {
        if (SettingsManager.Instance == null && settingsManagerPrefab != null)
        {
            GameObject settingsManager = Instantiate(settingsManagerPrefab);
            settingsManager.name = "SettingsManager";
            Debug.Log("SettingsManager inicializado");
        }
    }

    private void InitializeMenuManager()
    {
        if (MenuManager.Instance == null && menuManagerPrefab != null)
        {
            GameObject menuManager = Instantiate(menuManagerPrefab);
            menuManager.name = "MenuManager";
            Debug.Log("MenuManager inicializado");
        }
    }

    // Método para inicializar manualmente um sistema específico
    public void InitializeSystem(string systemName)
    {
        switch (systemName.ToLower())
        {
            case "audio":
                InitializeAudioManager();
                break;
            case "localization":
                InitializeLocalizationManager();
                break;
            case "brightness":
                InitializeBrightnessManager();
                break;
            case "settings":
                InitializeSettingsManager();
                break;
            case "menu":
                InitializeMenuManager();
                break;
            default:
                Debug.LogWarning($"Sistema {systemName} não reconhecido.");
                break;
        }
    }

    // Método para verificar se todos os sistemas estão inicializados
    public bool AreAllSystemsInitialized()
    {
        return AudioManager.Instance != null &&
               LocalizationManager.instance != null &&
               BrightnessManager.Instance != null &&
               SettingsManager.Instance != null &&
               MenuManager.Instance != null;
    }

    // Método para destruir todos os sistemas (útil para testes)
    public void DestroyAllSystems()
    {
        if (AudioManager.Instance != null)
            Destroy(AudioManager.Instance.gameObject);
        if (LocalizationManager.instance != null)
            Destroy(LocalizationManager.instance.gameObject);
        if (BrightnessManager.Instance != null)
            Destroy(BrightnessManager.Instance.gameObject);
        if (SettingsManager.Instance != null)
            Destroy(SettingsManager.Instance.gameObject);
        if (MenuManager.Instance != null)
            Destroy(MenuManager.Instance.gameObject);
    }
}