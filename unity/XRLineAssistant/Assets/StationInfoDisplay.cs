using UnityEngine;
using TMPro;

public class StationInfoDisplay : MonoBehaviour
{
    [Header("Data Manager (Drag DataManager object here)")]
    public ApiDataManager apiDataManager;  // Only uses ApiDataManager now

    [Header("Station Info UI")]
    public TextMeshProUGUI stationStatusText;
    public TextMeshProUGUI recipeText;

    [Header("Hopper Data UI")]
    public TextMeshProUGUI blueHopperText;
    public TextMeshProUGUI yellowHopperText;
    public TextMeshProUGUI redHopperText;

    [Header("Optional")]
    public TextMeshProUGUI connectionStatusText;
    public TextMeshProUGUI alarmText;

    void Start()
    {
        if (apiDataManager == null)
        {
            apiDataManager = FindObjectOfType<ApiDataManager>();
        }

        if (apiDataManager != null)
        {
            apiDataManager.OnDataUpdated += OnDataReceived;
            apiDataManager.OnConnectionError += OnError;
        }
    }

    void OnDestroy()
    {
        if (apiDataManager != null)
        {
            apiDataManager.OnDataUpdated -= OnDataReceived;
            apiDataManager.OnConnectionError -= OnError;
        }
    }

    void OnDataReceived(StationData data)
    {
        UpdateDisplay(data);
    }

    void OnError(string errorMessage)
    {
        if (connectionStatusText != null)
        {
            connectionStatusText.text = $"⚠ {errorMessage}";
            connectionStatusText.color = Color.yellow;
        }
    }

    void Update()
    {
        if (apiDataManager != null && apiDataManager.CurrentData != null)
        {
            UpdateDisplay(apiDataManager.CurrentData);
        }
    }

    void UpdateDisplay(StationData data)
    {
        if (data == null) return;

        // Update station status
        if (stationStatusText != null && data.station != null)
        {
            stationStatusText.text = $"Status: {data.station.status}";
            stationStatusText.color = data.station.status == "Running" ? Color.green : Color.yellow;
        }

        // Update recipe
        if (recipeText != null && data.station != null)
        {
            recipeText.text = $"Recipe: {data.station.currentRecipe ?? "None"}";
        }

        // Update connection status
        if (connectionStatusText != null)
        {
            if (apiDataManager.IsConnected)
            {
                connectionStatusText.text = "● Connected";
                connectionStatusText.color = Color.green;
            }
            else
            {
                connectionStatusText.text = "● Disconnected";
                connectionStatusText.color = Color.red;
            }
        }

        // Update hopper displays
        if (data.hoppers != null)
        {
            foreach (var hopper in data.hoppers)
            {
                switch (hopper.color.ToLower())
                {
                    case "blue":
                        UpdateHopperText(blueHopperText, hopper);
                        break;
                    case "yellow":
                        UpdateHopperText(yellowHopperText, hopper);
                        break;
                    case "red":
                        UpdateHopperText(redHopperText, hopper);
                        break;
                }
            }
        }

        // Update alarms
        if (alarmText != null && data.alarms != null)
        {
            if (data.alarms.Length > 0)
            {
                alarmText.text = $"⚠ {data.alarms.Length} Active Alarm(s)";
                alarmText.color = Color.red;
            }
            else
            {
                alarmText.text = "No Active Alarms";
                alarmText.color = Color.green;
            }
        }
    }

    void UpdateHopperText(TextMeshProUGUI textField, HopperData hopper)
    {
        if (textField == null || hopper == null) return;

        textField.text = $"{hopper.color.ToUpper()} Hopper\n" +
                        $"Level: {hopper.level}%\n" +
                        $"Refill: {(hopper.needsRefill ? "Yes" : "No")}\n" +
                        $"Status: {hopper.status}";

        if (hopper.status == "Offline")
        {
            textField.color = Color.gray;
        }
        else if (hopper.status == "Low" || hopper.status == "Empty")
        {
            textField.color = new Color(1f, 0.5f, 0.5f);
        }
        else
        {
            textField.color = Color.white;
        }
    }
}