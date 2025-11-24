using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainControlUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI recipeText;
    public Button backButton;

    [Header("Navigation")]
    public GameObject mainPanel;
    public GameObject startupPanel;

    [Header("Data")]
    public MockDataManager dataManager;

    [Header("Settings")]
    public float updateInterval = 2f;

    void Start()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackClicked);
            UnityEngine.Debug.Log("Back button listener added");
        }
        else
        {
            UnityEngine.Debug.LogWarning("Back button not assigned!");
        }

        InvokeRepeating("UpdateDisplay", 0f, updateInterval);

        UnityEngine.Debug.Log("MainControlUI initialized");
    }

    void UpdateDisplay()
    {
        if (dataManager == null)
        {
            UnityEngine.Debug.LogWarning("DataManager not assigned to MainControlUI!");

            if (statusText != null)
            {
                statusText.text = "Status: No Data Manager";
                statusText.color = Color.red;
            }

            if (recipeText != null)
            {
                recipeText.text = "Recipe: ERROR";
            }

            return;
        }

        StationData data = dataManager.GetStationData();

        if (data != null)
        {
            if (statusText != null)
            {
                statusText.text = "Status: " + data.status;

                // Change color based on status
                string statusLower = data.status.ToLower();

                if (statusLower.Contains("ready"))
                {
                    statusText.color = Color.green;
                }
                else if (statusLower.Contains("running"))
                {
                    statusText.color = Color.cyan;
                }
                else if (statusLower.Contains("error") || statusLower.Contains("refill") || statusLower.Contains("missing"))
                {
                    statusText.color = Color.yellow;
                }
                else if (statusLower.Contains("emergency"))
                {
                    statusText.color = Color.red;
                }
                else
                {
                    statusText.color = Color.white;
                }
            }

            if (recipeText != null)
            {
                recipeText.text = "Recipe: " + data.recipe;
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning("Station data is null!");
        }
    }

    void OnBackClicked()
    {
        UnityEngine.Debug.Log("Back button clicked!");

        if (mainPanel != null)
        {
            mainPanel.SetActive(false);
        }

        if (startupPanel != null)
        {
            startupPanel.SetActive(true);
        }
    }
}