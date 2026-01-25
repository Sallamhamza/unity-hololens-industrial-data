using UnityEngine;
using TMPro;
using Debug = UnityEngine.Debug;

public class StationInfoDisplay : MonoBehaviour
{
    [Header("Data Source")]
    public ApiDataManager apiDataManager;

    [Header("Station 402 Display")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI currentText;  // Changed back to currentText

    [Header("3D Hopper Texts (Floating Labels)")]
    public TMP_Text hopper1Text;
    public TMP_Text hopper2Text;
    public TMP_Text hopper3Text;

    [Header("Colors")]
    public Color okColor = Color.green;
    public Color warningColor = Color.yellow;
    public Color errorColor = Color.red;
    public Color alarmColor = Color.red;

    void Start()
    {
        if (apiDataManager == null)
            apiDataManager = FindObjectOfType<ApiDataManager>();

        if (apiDataManager != null)
        {
            apiDataManager.OnDataUpdated += UpdateDisplay;

            if (apiDataManager.CurrentData != null && !string.IsNullOrEmpty(apiDataManager.CurrentData.stationId))
            {
                UpdateDisplay(apiDataManager.CurrentData);
            }

            Debug.Log("[StationInfo 402] Connected to ApiDataManager");
        }
        else
        {
            Debug.LogError("[StationInfo 402] ApiDataManager not found!");
        }
    }

    void OnDestroy()
    {
        if (apiDataManager != null)
        {
            apiDataManager.OnDataUpdated -= UpdateDisplay;
        }
    }

    public void UpdateDisplay(StationData data)
    {
        if (data == null)
        {
            Debug.LogWarning("[StationInfo 402] Data is null!");
            return;
        }

        Debug.Log($"[StationInfo 402] Updating - Status: {data.status}, Current: {data.current}A");

        // Update Status
        if (statusText != null)
        {
            statusText.text = "Status: " + data.status;

            // Color code based on status
            if (data.status.ToLower().Contains("alarm") || data.status.ToLower().Contains("error"))
            {
                statusText.color = alarmColor;
                Debug.Log("[StationInfo 402] Status is ALARM - setting RED");
            }
            else if (data.status.ToLower().Contains("warning"))
            {
                statusText.color = warningColor;
                Debug.Log("[StationInfo 402] Status is WARNING - setting YELLOW");
            }
            else
            {
                statusText.color = okColor;
                Debug.Log("[StationInfo 402] Status is OK - setting GREEN");
            }
        }

        // Update Current (showing the float value)
        if (currentText != null)
        {
            currentText.text = $"Current: {data.current:F2} A";

            // Color based on current level
            if (data.current > 10f)
            {
                currentText.color = warningColor;
                Debug.Log("[StationInfo 402] Current HIGH - setting YELLOW");
            }
            else if (data.current < 0.1f)
            {
                currentText.color = errorColor;
                Debug.Log("[StationInfo 402] Current LOW - setting RED");
            }
            else
            {
                currentText.color = okColor;
                Debug.Log("[StationInfo 402] Current NORMAL - setting GREEN");
            }

            Debug.Log($"[StationInfo 402] Current set to: {data.current:F2} A");
        }

        // Update Hopper Texts (Floating 3D Labels)
        if (hopper1Text != null)
        {
            hopper1Text.text = GetHopperStatusText(data.hopper1Present, data.hopper1Full);
            hopper1Text.color = GetHopperColor(data.hopper1Present, data.hopper1Full);
        }

        if (hopper2Text != null)
        {
            hopper2Text.text = GetHopperStatusText(data.hopper2Present, data.hopper2Full);
            hopper2Text.color = GetHopperColor(data.hopper2Present, data.hopper2Full);
        }

        if (hopper3Text != null)
        {
            hopper3Text.text = GetHopperStatusText(data.hopper3Present, data.hopper3Full);
            hopper3Text.color = GetHopperColor(data.hopper3Present, data.hopper3Full);
        }

        Debug.Log("[StationInfo 402] Display updated successfully!");
    }

    string GetHopperStatusText(bool present, bool full)
    {
        if (!present) return "MISSING";
        else if (!full) return "LOW";
        else return "FULL";
    }

    Color GetHopperColor(bool present, bool full)
    {
        if (!present) return errorColor;
        else if (!full) return warningColor;
        else return okColor;
    }
}