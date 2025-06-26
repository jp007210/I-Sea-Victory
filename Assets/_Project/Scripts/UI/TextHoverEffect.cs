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

    void Awake()
    {
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

        if (hoverSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(hoverSound);
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
        if (clickSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(clickSound);
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
