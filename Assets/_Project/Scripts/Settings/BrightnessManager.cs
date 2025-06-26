using UnityEngine;
using UnityEngine.UI;

public class BrightnessManager : MonoBehaviour
{
    public static BrightnessManager Instance;

    [Header("Brightness Settings")]
    public Image brightnessOverlay;
    public Slider brightnessSlider;

    private const string BrightnessKey = "MasterBrightness";
    private const float DEFAULT_BRIGHTNESS = 1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadBrightness();

        if (brightnessSlider != null)
        {
            brightnessSlider.onValueChanged.RemoveAllListeners();
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
        }
    }

    public void SetBrightness(float value)
    {
        value = Mathf.Clamp01(value);

        ApplyBrightness(value);

        PlayerPrefs.SetFloat(BrightnessKey, value);
        PlayerPrefs.Save();
    }

    private void ApplyBrightness(float value)
    {
        if (brightnessOverlay != null)
        {
            float alpha = (1f - value) * 0.7f;
            Color overlayColor = brightnessOverlay.color;
            overlayColor.a = alpha;
            brightnessOverlay.color = overlayColor;
        }
    }

    private void LoadBrightness()
    {
        float savedBrightness = PlayerPrefs.GetFloat(BrightnessKey, DEFAULT_BRIGHTNESS);

        if (brightnessSlider != null)
        {
            brightnessSlider.SetValueWithoutNotify(savedBrightness);
        }

        ApplyBrightness(savedBrightness);
    }

    public float GetBrightness()
    {
        return PlayerPrefs.GetFloat(BrightnessKey, DEFAULT_BRIGHTNESS);
    }

    public void SetBrightnessOverlay(Image overlay)
    {
        brightnessOverlay = overlay;
        LoadBrightness();
    }

    public void SetBrightnessSlider(Slider slider)
    {
        brightnessSlider = slider;
        if (slider != null)
        {
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(SetBrightness);
            slider.SetValueWithoutNotify(GetBrightness());
        }
    }
}