using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Mixer")]
    public AudioMixer masterMixer;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource uiAudioSource;

    [Header("Menu Audio Clips")]
    public AudioClip waveIntroSound;
    public AudioClip menuMusic;
    public float musicStartDelay = 5.0f;

    [Header("UI Sound Effects")]
    public AudioClip buttonClickSound;
    public AudioClip buttonHoverSound;
    public AudioClip panelOpenSound;
    public AudioClip panelCloseSound;

    [Header("Scene Management")]
    public string menuSceneName = "MainMenu";

    // Chaves para PlayerPrefs
    private const string MasterVolumeKey = "MasterVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";

    // Volume padrão
    private const float DEFAULT_VOLUME = 0.8f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void InitializeAudio()
    {
        // Configurar AudioSources se não estiverem configurados
        if (musicSource == null)
        {
            GameObject musicGO = new GameObject("MusicSource");
            musicGO.transform.SetParent(transform);
            musicSource = musicGO.AddComponent<AudioSource>();
            musicSource.playOnAwake = false;
            musicSource.loop = true;
        }

        if (sfxSource == null)
        {
            GameObject sfxGO = new GameObject("SFXSource");
            sfxGO.transform.SetParent(transform);
            sfxSource = sfxGO.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
        }

        if (uiAudioSource == null)
        {
            GameObject uiGO = new GameObject("UIAudioSource");
            uiGO.transform.SetParent(transform);
            uiAudioSource = uiGO.AddComponent<AudioSource>();
            uiAudioSource.playOnAwake = false;
        }

        LoadVolumeSettings();
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == menuSceneName)
        {
            StartCoroutine(PlayMenuAudioSequence());
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == menuSceneName)
        {
            if (!musicSource.isPlaying)
            {
                StartCoroutine(PlayMenuAudioSequence());
            }
        }
        else
        {
            StopMenuAudio();
        }
    }

    private IEnumerator PlayMenuAudioSequence()
    {
        // Parar música atual
        musicSource.Stop();

        // Tocar som das ondas
        if (sfxSource != null && waveIntroSound != null)
        {
            sfxSource.clip = waveIntroSound;
            sfxSource.loop = true;
            sfxSource.Play();
        }

        // Esperar antes de começar a música
        yield return new WaitForSeconds(musicStartDelay);

        // Tocar música principal
        if (musicSource != null && menuMusic != null)
        {
            musicSource.clip = menuMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    private void StopMenuAudio()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }

        if (sfxSource != null && sfxSource.isPlaying && sfxSource.clip == waveIntroSound)
        {
            sfxSource.Stop();
        }
    }

    #region Volume Control
    public void SetMasterVolume(float value)
    {
        // Garantir que o valor está entre 0.0001 e 1
        value = Mathf.Clamp(value, 0.0001f, 1f);

        if (masterMixer != null)
        {
            float dbValue = Mathf.Log10(value) * 20;
            masterMixer.SetFloat(MasterVolumeKey, dbValue);
        }

        PlayerPrefs.SetFloat(MasterVolumeKey, value);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);

        if (masterMixer != null)
        {
            float dbValue = Mathf.Log10(value) * 20;
            masterMixer.SetFloat(MusicVolumeKey, dbValue);
        }

        PlayerPrefs.SetFloat(MusicVolumeKey, value);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);

        if (masterMixer != null)
        {
            float dbValue = Mathf.Log10(value) * 20;
            masterMixer.SetFloat(SFXVolumeKey, dbValue);
        }

        PlayerPrefs.SetFloat(SFXVolumeKey, value);
        PlayerPrefs.Save();
    }

    public float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat(MasterVolumeKey, DEFAULT_VOLUME);
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MusicVolumeKey, DEFAULT_VOLUME);
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFXVolumeKey, DEFAULT_VOLUME);
    }

    private void LoadVolumeSettings()
    {
        float masterValue = GetMasterVolume();
        float musicValue = GetMusicVolume();
        float sfxValue = GetSFXVolume();

        SetMasterVolume(masterValue);
        SetMusicVolume(musicValue);
        SetSFXVolume(sfxValue);
    }
    #endregion

    #region Sound Effects
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlaySFX(AudioClip clip, float volume)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }

    public void PlayUISound(AudioClip clip)
    {
        if (uiAudioSource != null && clip != null)
        {
            uiAudioSource.PlayOneShot(clip);
        }
    }

    // Métodos de conveniência para sons de UI
    public void PlayButtonClick()
    {
        PlayUISound(buttonClickSound);
    }

    public void PlayButtonHover()
    {
        PlayUISound(buttonHoverSound);
    }

    public void PlayPanelOpen()
    {
        PlayUISound(panelOpenSound);
    }

    public void PlayPanelClose()
    {
        PlayUISound(panelCloseSound);
    }
    #endregion

    #region Music Control
    public void PlayMusic(AudioClip musicClip, bool loop = true)
    {
        if (musicSource != null && musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.loop = loop;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void PauseMusic()
    {
        if (musicSource != null)
        {
            musicSource.Pause();
        }
    }

    public void UnpauseMusic()
    {
        if (musicSource != null)
        {
            musicSource.UnPause();
        }
    }

    public void FadeOutMusic(float duration = 1f)
    {
        if (musicSource != null)
        {
            StartCoroutine(FadeAudioSource(musicSource, musicSource.volume, 0f, duration));
        }
    }

    public void FadeInMusic(float targetVolume = 1f, float duration = 1f)
    {
        if (musicSource != null)
        {
            StartCoroutine(FadeAudioSource(musicSource, musicSource.volume, targetVolume, duration));
        }
    }

    private IEnumerator FadeAudioSource(AudioSource audioSource, float startVolume, float targetVolume, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float progress = elapsedTime / duration;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, progress);
            yield return null;
        }

        audioSource.volume = targetVolume;

        if (targetVolume <= 0f)
        {
            audioSource.Stop();
        }
    }
    #endregion
}