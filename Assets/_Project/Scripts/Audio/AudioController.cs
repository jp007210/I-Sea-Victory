using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource musicSource;

    void Start()
    {
        if (musicSource != null && !musicSource.isPlaying)
            musicSource.Play();
    }
}