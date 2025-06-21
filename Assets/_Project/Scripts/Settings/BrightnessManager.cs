using UnityEngine;
using UnityEngine.UI;

public class BrightnessManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider brightnessSlider;
    public Image brightnessPanel;

    private const string BrightnessKey = "MasterBrightness";

    void Start()
    {
        if (brightnessPanel != null)
        {
            LoadBrightness();
        }

        if (brightnessSlider != null)
        {
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
            UpdateSliderPosition();
        }
    }

    public void SetBrightness(float value)
    {
        float panelAlpha = (1 - value) * 0.7f;

        if (brightnessPanel != null)
        {
            Color panelColor = brightnessPanel.color;
            panelColor.a = panelAlpha;
            brightnessPanel.color = panelColor;
        }

        PlayerPrefs.SetFloat(BrightnessKey, value);
        PlayerPrefs.Save();
    }

    private void LoadBrightness()
    {
        float savedBrightness = PlayerPrefs.GetFloat(BrightnessKey, 1f);
        SetBrightness(savedBrightness);
    }

    private void UpdateSliderPosition()
    {
        if (brightnessSlider != null)
        {
            float savedBrightness = PlayerPrefs.GetFloat(BrightnessKey, 1f);
            brightnessSlider.SetValueWithoutNotify(savedBrightness);
        }
    }
}