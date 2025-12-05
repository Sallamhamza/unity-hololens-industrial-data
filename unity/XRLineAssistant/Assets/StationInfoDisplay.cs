using UnityEngine;
using TMPro;

public class StationInfoDisplay : MonoBehaviour
{
    [Header("Data Source")]
    public ApiDataManager api;

    [Header("UI Elements")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI runningTimeText;
    public TextMeshProUGUI currentText;
    public TextMeshProUGUI alarmMessageText;
    public TextMeshProUGUI connectionStatusText;

    void Start()
    {
        if (!api)
            api = FindObjectOfType<ApiDataManager>();

        api.OnDataUpdated += UpdateUI;
        api.OnConnectionError += ShowError;
    }

    void OnDestroy()
    {
        if (api)
        {
            api.OnDataUpdated -= UpdateUI;
            api.OnConnectionError -= ShowError;
        }
    }

    void Update()
    {
        if (api && api.CurrentData != null)
            UpdateUI(api.CurrentData);
    }

    void ShowError(string msg)
    {
        if (connectionStatusText)
        {
            connectionStatusText.text = "Disconnected";
            connectionStatusText.color = Color.red;
        }
    }

    void UpdateUI(StationData data)
    {
        if (data.station != null)
        {
            if (statusText)
                statusText.text = "Status: " + data.station.status;

            if (runningTimeText)
                runningTimeText.text =
                    "Running Time: " + data.station.runningTime + " sec";

            if (currentText)
                currentText.text =
                    "Current: " + data.station.current.ToString("F2") + " A";
        }

        if (connectionStatusText)
        {
            connectionStatusText.text = api.IsConnected ? "Connected" : "Disconnected";
            connectionStatusText.color = api.IsConnected ? Color.green : Color.red;
        }

        if (alarmMessageText)
        {
            if (data.alarms != null && data.alarms.Length > 0)
            {
                alarmMessageText.text =
                    "Alarm: " + data.alarms[0].message;
                alarmMessageText.color = Color.red;
            }
            else
            {
                alarmMessageText.text = "No Alarms";
                alarmMessageText.color = Color.green;
            }
        }
    }
}
