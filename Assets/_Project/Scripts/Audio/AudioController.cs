using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Header("Configurações Globais de Áudio")]
    [Range(0f, 1f)]
    public float masterVolume = 1.0f;
    [Range(0f, 1f)]
    public float sfxVolume = 1.0f;
    [Range(0f, 1f)]
    public float musicVolume = 1.0f;

    public AudioSource uiAudioSource;

    public static AudioController Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        if (uiAudioSource == null)
        {
            uiAudioSource = GetComponent<AudioSource>();
            if (uiAudioSource == null)
            {
                uiAudioSource = FindObjectOfType<AudioSource>();
            }
        }
    }

    void Start()
    {
        AudioListener.volume = masterVolume;
        AudioListener.pause = false;

    }

    public void PlaySFX(AudioClip clip)
    {
        if (uiAudioSource != null && clip != null)
        {
            uiAudioSource.PlayOneShot(clip, sfxVolume);
        }
    }
}