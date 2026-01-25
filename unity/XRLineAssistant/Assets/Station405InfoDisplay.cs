using UnityEngine;
using TMPro;
using Debug = UnityEngine.Debug;

public class Station405InfoDisplay : MonoBehaviour
{
    [Header("Data Source")]
    public ApiDataManager apiDataManager;

    [Header("Station 405 Display")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI currentText;

    [Header("Colors")]
    public Color normalColor = Color.green;
    public Color warningColor = Color.yellow;

    void Start()
    {
        if (apiDataManager == null)
            apiDataManager = FindObjectOfType<ApiDataManager>();

        if (apiDataManager != null)
        {
            apiDataManager.On405DataUpdated += UpdateDisplay;

            // Initial update if data already exists
            if (apiDataManager.Current405Data != null && !string.IsNullOrEmpty(apiDataManager.Current405Data.stationId))
            {
                UpdateDisplay(apiDataManager.Current405Data);
            }

            Debug.Log("[Station405Info] Connected to ApiDataManager");
        }
        else
        {
            Debug.LogError("[Station405Info] ApiDataManager not found!");
        }
    }

    void OnDestroy()
    {
        if (apiDataManager != null)
        {
            apiDataManager.On405DataUpdated -= UpdateDisplay;
        }
    }

    public void UpdateDisplay(Station405Data data)
    {
        if (data == null)
        {
            Debug.LogWarning("[Station405Info] Data is null!");
            return;
        }

        Debug.Log($"[Station405Info] Updating - Status: {data.status}, Current: {data.current}A");

        // Update status
        if (statusText != null)
        {
            statusText.text = "Status: " + data.status;
            statusText.color = normalColor;
        }

        // Update current reading
        if (currentText != null)
        {
            currentText.text = $"Current: {data.current:F2} A";

            // Color based on current level (adjust threshold as needed)
            if (data.current > 10f)
                currentText.color = warningColor;
            else
                currentText.color = normalColor;
        }

        Debug.Log("[Station405Info] Display updated successfully");
    }
}