using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Debug = UnityEngine.Debug;

public class MainControlUI : MonoBehaviour
{
    [Header("Panel References")]
    public GameObject startupMenu;
    public GameObject mainControlPanel;

    [Header("Back Button (Unity UI)")]
    public Button backButton;

    [Header("Status Display")]
    public TextMeshProUGUI statusText;

    void Start()
    {
        Debug.Log("=== MAIN CONTROL UI INITIALIZED ===");

        // Connect back button
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
            Debug.Log("[OK] Back button connected");
        }
        else
        {
            Debug.LogError("[ERROR] Back button reference is NULL!");
        }
    }

    public void OnBackButtonClicked()
    {
        Debug.Log("[BACK] Button clicked - Returning to Startup Menu");

        if (startupMenu == null || mainControlPanel == null)
        {
            Debug.LogError("[ERROR] Cannot switch panels - references are NULL!");
            return;
        }

        mainControlPanel.SetActive(false);
        startupMenu.SetActive(true);

        Debug.Log("[SUCCESS] Returned to Startup Menu!");
    }

    public void UpdateStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }
}