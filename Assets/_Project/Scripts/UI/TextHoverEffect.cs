using UnityEngine;
using UnityEngine.EventSystems; 
using TMPro;                 

public class TextHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text textMesh;

    [Header("Configurações do Contorno")]
    public float outlineOnHover = 0.1f;
    public float underlayOnHover = 0.2f;

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
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (textMesh != null)
        {
            RemoveOutline();
        }
    }

    private void RemoveOutline()
    {
        textMesh.outlineWidth = 0;
        textMesh.fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayDilate, 0);
    }
}
