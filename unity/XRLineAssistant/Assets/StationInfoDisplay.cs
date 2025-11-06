using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StationInfoDisplay : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI stationHeader;
    public TextMeshProUGUI statusDisplay;
    public TextMeshProUGUI recipeDisplay;
    public TextMeshProUGUI alarmDisplay;

    [Header("Blue Hopper")]
    public UnityEngine.UI.Image blueIndicatorIcon;
    public TextMeshProUGUI blueHopperText;
    public UnityEngine.UI.Image blueLevelBarFill;
    public TextMeshProUGUI blueLevelText;

    [Header("Yellow Hopper")]
    public UnityEngine.UI.Image yellowIndicatorIcon;
    public TextMeshProUGUI yellowHopperText;
    public UnityEngine.UI.Image yellowLevelBarFill;
    public TextMeshProUGUI yellowLevelText;

    [Header("Red Hopper")]
    public UnityEngine.UI.Image redIndicatorIcon;
    public TextMeshProUGUI redHopperText;
    public UnityEngine.UI.Image redLevelBarFill;
    public TextMeshProUGUI redLevelText;

    [Header("Data Source")]
    public MockDataManager dataManager;

    [Header("Settings")]
    public float updateInterval = 2f;
    public bool alwaysFaceUser = true;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        if (stationHeader != null)
            stationHeader.text = "SIF-402 STATION";

        InvokeRepeating("UpdateDisplay", 0f, updateInterval);
    }

    void Update()
    {
        if (alwaysFaceUser && mainCamera != null)
        {
            Vector3 directionToCamera = mainCamera.transform.position - transform.position;
            directionToCamera.y = 0;

            if (directionToCamera != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(-directionToCamera);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
    }

    void UpdateDisplay()
    {
        if (dataManager == null)
        {
            UnityEngine.Debug.LogWarning("DataManager not assigned to StationInfoDisplay!");
            return;
        }

        StationData data = dataManager.GetStationData();

        if (data != null)
        {
            if (statusDisplay != null)
            {
                statusDisplay.text = "Status: " + data.status;

                string statusLower = data.status.ToLower();
                if (statusLower.Contains("ready"))
                    statusDisplay.color = Color.green;
                else if (statusLower.Contains("running"))
                    statusDisplay.color = Color.cyan;
                else if (statusLower.Contains("error") || statusLower.Contains("refill"))
                    statusDisplay.color = Color.yellow;
                else if (statusLower.Contains("emergency"))
                    statusDisplay.color = Color.red;
                else
                    statusDisplay.color = Color.white;
            }

            if (recipeDisplay != null)
            {
                recipeDisplay.text = "Recipe: " + data.recipe;
            }

            if (data.hoppers != null && data.hoppers.Length >= 3)
            {
                UpdateHopperDisplay(0, blueIndicatorIcon, blueLevelBarFill, blueHopperText, blueLevelText, data.hoppers[0]);
                UpdateHopperDisplay(1, yellowIndicatorIcon, yellowLevelBarFill, yellowHopperText, yellowLevelText, data.hoppers[1]);
                UpdateHopperDisplay(2, redIndicatorIcon, redLevelBarFill, redHopperText, redLevelText, data.hoppers[2]);
            }

            UpdateAlarmDisplay(data);
        }
    }

    void UpdateHopperDisplay(int index, UnityEngine.UI.Image icon, UnityEngine.UI.Image levelBar, TextMeshProUGUI text, TextMeshProUGUI levelText, HopperData hopper)
    {
        if (icon == null || levelBar == null || text == null || levelText == null) return;

        bool isPresent = hopper.isPresent;
        bool hasPellets = hopper.hasPellets;

        int level = 0;
        if (isPresent && hasPellets)
            level = UnityEngine.Random.Range(60, 100);
        else if (isPresent && !hasPellets)
            level = UnityEngine.Random.Range(0, 30);
        else
            level = 0;

        Color hopperColor = Color.white;
        if (hopper.color == "blue") hopperColor = new Color(0, 0.47f, 1f);
        else if (hopper.color == "yellow") hopperColor = new Color(1f, 0.86f, 0f);
        else if (hopper.color == "red") hopperColor = new Color(1f, 0.2f, 0.2f);

        icon.color = hopperColor;

        string statusEmoji = "";
        string statusText = "";
        Color textColor = Color.white;

        if (!isPresent)
        {
            statusEmoji = "❌";
            statusText = "MISSING";
            textColor = Color.red;
        }
        else if (!hasPellets)
        {
            statusEmoji = "⚠️";
            statusText = "LOW";
            textColor = Color.yellow;
        }
        else
        {
            statusEmoji = "✅";
            statusText = "OK";
            textColor = Color.green;
        }

        text.text = hopper.color.ToUpper() + "\n" + statusEmoji + " " + statusText;
        text.color = textColor;

        float fillAmount = level / 100f;
        RectTransform barRect = levelBar.GetComponent<RectTransform>();
        if (barRect != null)
        {
            barRect.sizeDelta = new Vector2(140 * fillAmount, barRect.sizeDelta.y);
        }

        if (level > 50)
            levelBar.color = Color.green;
        else if (level > 20)
            levelBar.color = Color.yellow;
        else
            levelBar.color = Color.red;

        levelText.text = level + "%";
    }

    void UpdateAlarmDisplay(StationData data)
    {
        if (alarmDisplay == null) return;

        string alarmText = "";
        int alarmCount = 0;

        if (data.hoppers != null)
        {
            for (int i = 0; i < data.hoppers.Length; i++)
            {
                if (!data.hoppers[i].isPresent)
                {
                    alarmText += "🔴 Missing hopper " + (i + 1) + "\n";
                    alarmCount++;
                }
                else if (!data.hoppers[i].hasPellets)
                {
                    alarmText += "🟡 Refill " + data.hoppers[i].color + " pellets\n";
                    alarmCount++;
                }
            }
        }

        string statusLower = data.status.ToLower();
        if (statusLower.Contains("error") || statusLower.Contains("alarm"))
        {
            alarmText += "⚠️ " + data.status + "\n";
            alarmCount++;
        }

        if (alarmCount == 0)
        {
            alarmDisplay.text = "✅ No Active Alarms";
            alarmDisplay.color = Color.green;
        }
        else
        {
            alarmDisplay.text = "⚠️ ALARMS (" + alarmCount + "):\n" + alarmText;
            alarmDisplay.color = Color.yellow;
        }
    }
}