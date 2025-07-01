using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class PowerUpOptionUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Referências da UI")]
    public Image iconImage;
    public TMP_Text nameText;
    public RectTransform descriptionPanel;
    public TMP_Text descriptionText;

    [Header("Configurações da Animação")]
    public float animationDuration = 0.3f;

    private Coroutine animationCoroutine;

    void Awake()
    {
        AspectRatioFitter existingFitter = iconImage.GetComponent<AspectRatioFitter>();
        if (existingFitter != null)
        {
            DestroyImmediate(existingFitter);
        }
    }

    public void Setup(PowerUp powerUpData)
    {
        iconImage.sprite = powerUpData.icon;
        nameText.text = powerUpData.powerUpName;
        descriptionText.text = powerUpData.description;

        if (powerUpData.icon != null)
        {
            iconImage.preserveAspect = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        animationCoroutine = StartCoroutine(AnimateDescriptionPanel(true));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        animationCoroutine = StartCoroutine(AnimateDescriptionPanel(false));
    }

    private IEnumerator AnimateDescriptionPanel(bool show)
    {
        float targetScaleY = show ? 1f : 0f;
        float startScaleY = descriptionPanel.localScale.y;
        float timer = 0f;

        while (timer < animationDuration)
        {
            timer += Time.unscaledDeltaTime;
            float newScaleY = Mathf.Lerp(startScaleY, targetScaleY, timer / animationDuration);
            descriptionPanel.localScale = new Vector3(1, newScaleY, 1);
            yield return null;
        }

        descriptionPanel.localScale = new Vector3(1, targetScaleY, 1);
    }
}
