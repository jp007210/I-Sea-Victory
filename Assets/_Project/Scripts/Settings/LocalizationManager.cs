using UnityEngine;
using System;
using TMPro;
using System.Collections.Generic;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance;

    [Header("UI Reference")]
    public TMP_Dropdown languageDropdown;

    public enum Language { English, Portuguese }
    public Language CurrentLanguage { get; private set; }

    public static event Action OnLanguageChanged;

    private const string LanguageKey = "SelectedLanguage";

    // Dicionário para textos (pode ser expandido facilmente)
    private Dictionary<string, Dictionary<Language, string>> textDictionary;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeTextDictionary();
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

    private void InitializeTextDictionary()
    {
        textDictionary = new Dictionary<string, Dictionary<Language, string>>();

        // Exemplo de como adicionar textos (pode ser expandido)
        AddText("start_game", "Start Game", "Iniciar Jogo");
        AddText("options", "Options", "Opções");
        AddText("credits", "Credits", "Créditos");
        AddText("exit", "Exit", "Sair");
        AddText("continue", "Continue", "Continuar");
        AddText("restart", "Restart", "Reiniciar");
        AddText("main_menu", "Main Menu", "Menu Principal");
        AddText("game_over", "Game Over", "Fim de Jogo");
        AddText("retry", "Retry?", "Tentar Novamente?");
        AddText("yes", "Yes", "Sim");
        AddText("no", "No", "Não");
        AddText("audio", "Audio", "Áudio");
        AddText("video", "Video", "Vídeo");
        AddText("controls", "Controls", "Controles");
        AddText("master_volume", "Master Volume", "Volume Geral");
        AddText("music_volume", "Music Volume", "Volume da Música");
        AddText("sfx_volume", "Sound Effects", "Efeitos Sonoros");
        AddText("brightness", "Brightness", "Brilho");
        AddText("language", "Language", "Idioma");
        AddText("fullscreen", "Fullscreen", "Tela Cheia");
        AddText("resolution", "Resolution", "Resolução");
        AddText("apply", "Apply", "Aplicar");
        AddText("reset", "Reset", "Resetar");
        AddText("back", "Back", "Voltar");
        AddText("score", "Score", "Pontuação");
        AddText("lives", "Lives", "Vidas");
        AddText("time", "Time", "Tempo");
        AddText("final_score", "Final Score", "Pontuação Final");
        AddText("new_game", "New Game", "Novo Jogo");
        AddText("load_game", "Load Game", "Carregar Jogo");
        AddText("save_game", "Save Game", "Salvar Jogo");
        AddText("quit_game", "Quit Game", "Sair do Jogo");
        AddText("pause", "Pause", "Pausar");
        AddText("resume", "Resume", "Continuar");
        AddText("settings", "Settings", "Configurações");
    }

    private void AddText(string key, string englishText, string portugueseText)
    {
        if (!textDictionary.ContainsKey(key))
        {
            textDictionary[key] = new Dictionary<Language, string>();
        }

        textDictionary[key][Language.English] = englishText;
        textDictionary[key][Language.Portuguese] = portugueseText;
    }

    public string GetText(string key)
    {
        if (textDictionary.ContainsKey(key) && textDictionary[key].ContainsKey(CurrentLanguage))
        {
            return textDictionary[key][CurrentLanguage];
        }

        // Fallback para inglês se não encontrar na linguagem atual
        if (textDictionary.ContainsKey(key) && textDictionary[key].ContainsKey(Language.English))
        {
            return textDictionary[key][Language.English];
        }

        // Se não encontrar nada, retorna a chave
        return key;
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

    // Método para adicionar novos textos em runtime (útil para expansões)
    public void AddNewText(string key, string englishText, string portugueseText)
    {
        AddText(key, englishText, portugueseText);
        OnLanguageChanged?.Invoke(); // Atualizar textos existentes
    }

    // Método para obter todas as chaves disponíveis
    public List<string> GetAllKeys()
    {
        return new List<string>(textDictionary.Keys);
    }
}