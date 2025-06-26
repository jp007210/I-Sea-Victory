using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class LocalizableText : MonoBehaviour
{
    [Header("Localization")]
    public string textKey;

    [Header("Legacy Support (será removido em versões futuras)")]
    [TextArea(3, 5)]
    public string portugueseText;
    [TextArea(3, 5)]
    public string englishText;

    private TMP_Text textMesh;
    private bool useLegacyMode = false;

    private void Awake()
    {
        textMesh = GetComponent<TMP_Text>();

        // Verificar se deve usar modo legado
        useLegacyMode = !string.IsNullOrEmpty(portugueseText) || !string.IsNullOrEmpty(englishText);
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

        if (useLegacyMode)
        {
            // Modo legado para compatibilidade
            if (LocalizationManager.instance.CurrentLanguage == LocalizationManager.Language.Portuguese)
            {
                textMesh.text = portugueseText;
            }
            else
            {
                textMesh.text = englishText;
            }
        }
        else
        {
            // Novo modo com dicionário
            if (!string.IsNullOrEmpty(textKey))
            {
                textMesh.text = LocalizationManager.instance.GetText(textKey);
            }
        }
    }

    // Método para definir nova chave de texto
    public void SetTextKey(string newKey)
    {
        textKey = newKey;
        useLegacyMode = false;
        UpdateText();
    }

    // Método para forçar modo legado
    public void SetLegacyTexts(string portuguese, string english)
    {
        portugueseText = portuguese;
        englishText = english;
        useLegacyMode = true;
        UpdateText();
    }
}