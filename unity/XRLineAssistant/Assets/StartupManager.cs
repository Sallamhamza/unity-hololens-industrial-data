using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class StartupManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button startButton;
    public Button settingsButton;
    public Button exitButton;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI subtitleText;

    [Header("Panels")]
    public GameObject startupPanel;
    public GameObject mainControlPanel;

    [Header("Data")]
    public MockDataManager dataManager;

    void Start()
    {
        SetupButtons();
        ShowStartupScreen();

        UnityEngine.Debug.Log("StartupManager initialized");
    }

    void SetupButtons()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartClicked);
            UnityEngine.Debug.Log("Start button listener added");
        }
        else
        {
            UnityEngine.Debug.LogWarning("Start button not assigned!");
        }

        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OnSettingsClicked);
        }
        else
        {
            UnityEngine.Debug.LogWarning("Settings button not assigned!");
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitClicked);
        }
        else
        {
            UnityEngine.Debug.LogWarning("Exit button not assigned!");
        }
    }

    void ShowStartupScreen()
    {
        if (startupPanel != null)
        {
            startupPanel.SetActive(true);
            UnityEngine.Debug.Log("Startup panel shown");
        }

        if (mainControlPanel != null)
        {
            mainControlPanel.SetActive(false);
        }
    }

    void OnStartClicked()
    {
        UnityEngine.Debug.Log("Start button clicked!");

        if (startupPanel != null)
        {
            startupPanel.SetActive(false);
        }

        if (mainControlPanel != null)
        {
            mainControlPanel.SetActive(true);
            UnityEngine.Debug.Log("Main control panel activated");
        }
        else
        {
            UnityEngine.Debug.LogWarning("Main control panel not assigned yet!");

            if (subtitleText != null)
            {
                subtitleText.text = "Loading main control...";
            }
        }
    }

    void OnSettingsClicked()
    {
        UnityEngine.Debug.Log("Settings clicked!");

        if (subtitleText != null)
        {
            subtitleText.text = "Settings coming soon...";
        }
    }

    void OnExitClicked()
    {
        UnityEngine.Debug.Log("Exit clicked!");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}