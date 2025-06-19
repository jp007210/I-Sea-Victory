using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TextHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private TMP_Text textMesh;
    public float outlineOnHover = 0.1f;
    public float underlayOnHover = 0.2f;
    public AudioClip hoverSound;
    public AudioClip clickSound;
    private static AudioSource uiAudioSource;

    void Awake()
    {
        textMesh = GetComponentInChildren<TMP_Text>();

        if (AudioController.Instance != null && AudioController.Instance.uiAudioSource != null)
        {
            uiAudioSource = AudioController.Instance.uiAudioSource;
        }
        else if (uiAudioSource == null)
        {
            uiAudioSource = FindObjectOfType<AudioSource>();
        }

        if (textMesh != null)
        {
            RemoveOutline();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (textMesh != null)
        {
            textMesh.outlineWidth = outlineOnHover;
            textMesh.fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayDilate, underlayOnHover);
        }
        if (uiAudioSource != null && hoverSound != null)
        {
            uiAudioSource.PlayOneShot(hoverSound);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (textMesh != null)
        {
            RemoveOutline();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (uiAudioSource != null && clickSound != null)
        {
            uiAudioSource.PlayOneShot(clickSound);
        }
    }

    private void RemoveOutline()
    {
        textMesh.outlineWidth = 0;
        textMesh.fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayDilate, 0);
    }
}
