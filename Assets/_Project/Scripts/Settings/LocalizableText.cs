using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class LocalizableText : MonoBehaviour
{
    [TextArea(3, 5)]
    public string portugueseText;

    [TextArea(3, 5)]
    public string englishText;

    private TMP_Text textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        if (LocalizationManager.instance != null)
        {
            LocalizationManager.OnLanguageChanged += UpdateText;
            UpdateText();
        }
    }

    private void OnDisable()
    {
        if (LocalizationManager.instance != null)
        {
            LocalizationManager.OnLanguageChanged -= UpdateText;
        }
    }

    public void UpdateText()
    {
        if (textMesh == null || LocalizationManager.instance == null) return;

        if (LocalizationManager.instance.CurrentLanguage == LocalizationManager.Language.Portuguese)
        {
            textMesh.text = portugueseText;
        }
        else
        {
            textMesh.text = englishText;
        }
    }
}
