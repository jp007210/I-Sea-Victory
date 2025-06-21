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

    // N�o precisamos mais de uma refer�ncia para um AudioSource aqui.

    void Awake()
    {
        // Apenas obtemos a refer�ncia para o texto, como antes.
        textMesh = GetComponentInChildren<TMP_Text>();

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

        // Pedimos ao AudioManager para tocar o som de hover.
        // Ele usar� o sfxSource automaticamente.
        if (hoverSound != null && AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(hoverSound);
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
        // Pedimos ao AudioManager para tocar o som de clique.
        // Ele usar� o sfxSource automaticamente.
        if (clickSound != null && AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(clickSound);
        }
    }

    private void RemoveOutline()
    {
        if (textMesh != null)
        {
            textMesh.outlineWidth = 0;
            textMesh.fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayDilate, 0);
        }
    }
}
