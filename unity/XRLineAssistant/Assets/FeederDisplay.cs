using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Debug = UnityEngine.Debug;
using Image = UnityEngine.UI.Image;

public class FeederDisplay : MonoBehaviour
{
    [Header("Data Source")]
    public ApiDataManager apiDataManager;

    [Header("Panel Title")]
    public TextMeshProUGUI titleText;

    [Header("Feeder 1 (Round Caps) - UI Panel")]
    public TextMeshProUGUI feeder1Label;
    public Image feeder1FillBar;
    public TextMeshProUGUI feeder1CountText;

    [Header("Feeder 2 (Square Caps) - UI Panel")]
    public TextMeshProUGUI feeder2Label;
    public Image feeder2FillBar;
    public TextMeshProUGUI feeder2CountText;

    [Header("3D Scene Texts (Floating Labels)")]
    // CHANGED THIS: TMP_Text accepts both "TextMeshPro" and "TextMeshProUGUI"
    public TMP_Text feeder1SceneText;
    public TMP_Text feeder2SceneText;

    [Header("Settings")]
    public int maxCaps = 50;
    public float animationSpeed = 2f;

    [Header("Colors")]
    public Color roundCapColor = new Color(0.2f, 0.6f, 1f);
    public Color squareCapColor = new Color(1f, 0.6f, 0.2f);
    public Color lowLevelColor = new Color(1f, 0.3f, 0.3f);

    private float targetFill1 = 0f;
    private float targetFill2 = 0f;
    private float currentFill1 = 0f;
    private float currentFill2 = 0f;

    void Start()
    {
        if (titleText != null)
            titleText.text = "Station 405 - Feeders";

        if (apiDataManager == null)
            apiDataManager = FindObjectOfType<ApiDataManager>();

        if (apiDataManager != null)
        {
            apiDataManager.OnFeedersUpdated += OnFeedersReceived;
        }

        if (feeder1FillBar != null)
            feeder1FillBar.color = roundCapColor;
        if (feeder2FillBar != null)
            feeder2FillBar.color = squareCapColor;
    }

    void OnDestroy()
    {
        if (apiDataManager != null)
            apiDataManager.OnFeedersUpdated -= OnFeedersReceived;
    }

    void OnFeedersReceived(FeederInfo[] feeders)
    {
        UpdateFromFeeders(feeders);
    }

    public void UpdateFromFeeders(FeederInfo[] feeders)
    {
        if (feeders == null) return;

        foreach (var feeder in feeders)
        {
            UpdateFeederTarget(feeder);
        }
    }

    void UpdateFeederTarget(FeederInfo feeder)
    {
        float fillAmount = Mathf.Clamp01((float)feeder.caps / maxCaps);

        if (feeder.feeder == 1) // Round Caps
        {
            targetFill1 = fillAmount;

            // Update Panel UI
            if (feeder1Label != null) feeder1Label.text = "Round Caps";
            if (feeder1CountText != null) feeder1CountText.text = $"{feeder.caps}";
            if (feeder1FillBar != null)
                feeder1FillBar.color = feeder.caps < 5 ? lowLevelColor : roundCapColor;

            // Update 3D Scene Text
            if (feeder1SceneText != null)
            {
                feeder1SceneText.text = $"Round: {feeder.caps}";
                feeder1SceneText.color = feeder.caps < 5 ? lowLevelColor : Color.white;
            }
        }
        else if (feeder.feeder == 2) // Square Caps
        {
            targetFill2 = fillAmount;

            // Update Panel UI
            if (feeder2Label != null) feeder2Label.text = "Square Caps";
            if (feeder2CountText != null) feeder2CountText.text = $"{feeder.caps}";
            if (feeder2FillBar != null)
                feeder2FillBar.color = feeder.caps < 5 ? lowLevelColor : squareCapColor;

            // Update 3D Scene Text
            if (feeder2SceneText != null)
            {
                feeder2SceneText.text = $"Square: {feeder.caps}";
                feeder2SceneText.color = feeder.caps < 5 ? lowLevelColor : Color.white;
            }
        }
    }

    void Update()
    {
        currentFill1 = Mathf.Lerp(currentFill1, targetFill1, Time.deltaTime * animationSpeed);
        currentFill2 = Mathf.Lerp(currentFill2, targetFill2, Time.deltaTime * animationSpeed);

        if (feeder1FillBar != null) feeder1FillBar.fillAmount = currentFill1;
        if (feeder2FillBar != null) feeder2FillBar.fillAmount = currentFill2;
    }
}