using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class StartupManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject startupMenu;
    public GameObject mainControlPanel;

    [Header("Buttons (Unity UI)")]
    public Button startButton;
    public Button settingsButton;
    public Button exitButton;

    void Start()
    {
        Debug.Log("=== STARTUP MANAGER INITIALIZED ===");

        // Show startup menu, hide main panel
        if (startupMenu != null)
        {
            startupMenu.SetActive(true);
            Debug.Log("[OK] Startup menu activated");
        }
        else
        {
            Debug.LogError("[ERROR] Startup menu reference is NULL!");
        }

        if (mainControlPanel != null)
        {
            mainControlPanel.SetActive(false);
            Debug.Log("[OK] Main control panel hidden");
        }
        else
        {
            Debug.LogError("[ERROR] Main control panel reference is NULL!");
        }

        // Connect button events
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
            Debug.Log("[OK] Start button connected");
        }
        else
        {
            Debug.LogError("[ERROR] Start button reference is NULL!");
        }

        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            Debug.Log("[OK] Settings button connected");
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
            Debug.Log("[OK] Exit button connected");
        }
    }

    void OnStartButtonClicked()
    {
        Debug.Log("[START] Button clicked - Switching to Control Panel");

        if (startupMenu == null || mainControlPanel == null)
        {
            Debug.LogError("[ERROR] Cannot switch panels - references are NULL!");
            return;
        }

        startupMenu.SetActive(false);
        mainControlPanel.SetActive(true);

        Debug.Log("[SUCCESS] Panel switch complete!");
    }

    void OnSettingsButtonClicked()
    {
        Debug.Log("[SETTINGS] Button clicked - Not implemented yet");
    }

    void OnExitButtonClicked()
    {
        Debug.Log("[EXIT] Button clicked - Quitting application");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}