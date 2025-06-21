using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Mixer")]
    [Tooltip("Arraste seu Audio Mixer principal para este campo.")]
    public AudioMixer masterMixer;

    [Header("Audio Sources")]
    [Tooltip("O AudioSource para a música de fundo.")]
    public AudioSource musicSource;
    [Tooltip("O AudioSource para efeitos sonoros e som ambiente.")]
    public AudioSource sfxSource;

    [Header("Audio Clips & Startup Settings")]
    [Tooltip("O clipe de som das ondas que toca no menu.")]
    public AudioClip waveIntroSound;
    [Tooltip("O clipe da música principal do menu.")]
    public AudioClip menuMusic;

    [Tooltip("Tempo em segundos para esperar antes de a música principal começar.")]
    public float musicStartDelay = 5.0f;

    [Header("Scene Management")]
    [Tooltip("Digite o nome exato da sua cena de MENU PRINCIPAL.")]
    public string menuSceneName = "MainMenu";

    private const string MasterVolumeKey = "MasterVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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

    private void Start()
    {
        LoadVolumeSettings();
        if (SceneManager.GetActiveScene().name == menuSceneName)
        {
            StartCoroutine(PlayAudioSequence());
        }
    }

    private IEnumerator PlayAudioSequence()
    {
        musicSource.Stop();

        if (sfxSource != null && waveIntroSound != null)
        {
            sfxSource.clip = waveIntroSound;
            sfxSource.loop = true;
            sfxSource.Play();
        }

        yield return new WaitForSeconds(musicStartDelay);

        if (musicSource != null && menuMusic != null)
        {
            musicSource.clip = menuMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != menuSceneName)
        {
            StopMenuAudio();
        }
        else
        {
            if (!musicSource.isPlaying)
            {
                StartCoroutine(PlayAudioSequence());
            }
        }
    }

    private void StopMenuAudio()
    {
        if (musicSource != null && musicSource.isPlaying) musicSource.Stop();
        if (sfxSource != null && sfxSource.isPlaying && sfxSource.clip == waveIntroSound)
        {
            sfxSource.Stop();
        }
    }

    #region Volume Control

    public void SetMasterVolume(float value)
    {
        masterMixer.SetFloat(MasterVolumeKey, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(MasterVolumeKey, value);
    }

    public void SetMusicVolume(float value)
    {
        masterMixer.SetFloat(MusicVolumeKey, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(MusicVolumeKey, value);
    }

    public void SetSFXVolume(float value)
    {
        masterMixer.SetFloat(SFXVolumeKey, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(SFXVolumeKey, value);
    }

    private void LoadVolumeSettings()
    {
        float masterValue = PlayerPrefs.GetFloat(MasterVolumeKey, 1f);
        float musicValue = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        float sfxValue = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);

        masterMixer.SetFloat(MasterVolumeKey, Mathf.Log10(masterValue) * 20);
        masterMixer.SetFloat(MusicVolumeKey, Mathf.Log10(musicValue) * 20);
        masterMixer.SetFloat(SFXVolumeKey, Mathf.Log10(sfxValue) * 20);
    }
    #endregion
}
