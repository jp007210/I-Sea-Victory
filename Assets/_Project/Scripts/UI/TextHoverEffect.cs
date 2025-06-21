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

    // Não precisamos mais de uma referência para um AudioSource aqui.

    void Awake()
    {
        // Apenas obtemos a referência para o texto, como antes.
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
        // Ele usará o sfxSource automaticamente.
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
        // Ele usará o sfxSource automaticamente.
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
