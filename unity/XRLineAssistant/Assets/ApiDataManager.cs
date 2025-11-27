using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using Debug = UnityEngine.Debug;

public class ApiDataManager : MonoBehaviour
{
    [Header("API Settings")]
    [Tooltip("Node-RED server base URL (e.g., http://192.168.1.100:1880)")]
    public string baseUrl = "http://localhost:1880";

    [Tooltip("How often to fetch data (seconds)")]
    public float refreshInterval = 3.0f;

    [Header("Endpoints")]
    public string blueEndpoint = "/mes/blue_latest";
    public string yellowEndpoint = "/mes/yellow_latest";
    public string redEndpoint = "/mes/red_latest";

    [Header("Debug")]
    public bool enableDebugLogs = true;

    // Combined station data
    private StationData currentData;
    public StationData CurrentData => currentData;

    // Connection status
    private bool isConnected = false;
    public bool IsConnected => isConnected;

    // Events for UI updates
    public event Action<StationData> OnDataUpdated;
    public event Action<string> OnConnectionError;

    void Start()
    {
        currentData = new StationData
        {
            station = new Station
            {
                id = "SIF-402",
                status = "Connecting...",
                currentRecipe = "Loading...",
                productionCount = 0,
                efficiency = 0
            },
            hoppers = new HopperData[]
            {
                new HopperData { id = "hopper_blue", color = "blue", level = 0, status = "Connecting" },
                new HopperData { id = "hopper_yellow", color = "yellow", level = 0, status = "Connecting" },
                new HopperData { id = "hopper_red", color = "red", level = 0, status = "Connecting" }
            },
            alarms = new AlarmData[0]
        };

        StartCoroutine(FetchDataLoop());
    }

    IEnumerator FetchDataLoop()
    {
        while (true)
        {
            yield return StartCoroutine(FetchAllData());
            yield return new WaitForSeconds(refreshInterval);
        }
    }

    IEnumerator FetchAllData()
    {
        int successCount = 0;

        yield return StartCoroutine(FetchEndpoint(blueEndpoint, 0, (success) => { if (success) successCount++; }));
        yield return StartCoroutine(FetchEndpoint(yellowEndpoint, 1, (success) => { if (success) successCount++; }));
        yield return StartCoroutine(FetchEndpoint(redEndpoint, 2, (success) => { if (success) successCount++; }));

        isConnected = successCount > 0;

        if (isConnected)
        {
            currentData.station.status = "Running";
            OnDataUpdated?.Invoke(currentData);

            if (enableDebugLogs)
            {
                Debug.Log($"[API] Data updated - Status: {currentData.station.status}, Alarms: {currentData.alarms.Length}");
            }
        }
        else
        {
            currentData.station.status = "Disconnected";
            OnConnectionError?.Invoke("Unable to connect to Node-RED server");
        }
    }

    IEnumerator FetchEndpoint(string endpoint, int hopperIndex, Action<bool> callback)
    {
        string url = baseUrl + endpoint;

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.timeout = 5;
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    string json = request.downloadHandler.text;
                    HopperResponse response = JsonUtility.FromJson<HopperResponse>(json);

                    if (response != null && response.hoppers != null && response.hoppers.Length > 0)
                    {
                        currentData.hoppers[hopperIndex] = response.hoppers[0];

                        if (response.station != null)
                        {
                            currentData.station.currentRecipe = response.station.currentRecipe;
                            currentData.station.productionCount = response.station.productionCount;
                            currentData.station.efficiency = response.station.efficiency;
                        }

                        if (response.alarms != null && response.alarms.Length > 0)
                        {
                            currentData.alarms = response.alarms;
                        }

                        if (enableDebugLogs)
                        {
                            Debug.Log($"[API] {endpoint} - Level: {response.hoppers[0].level}%, Status: {response.hoppers[0].status}");
                        }

                        callback?.Invoke(true);
                    }
                    else
                    {
                        Debug.LogWarning($"[API] Invalid response from {endpoint}");
                        callback?.Invoke(false);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"[API] Parse error for {endpoint}: {e.Message}");
                    callback?.Invoke(false);
                }
            }
            else
            {
                if (enableDebugLogs)
                {
                    Debug.LogWarning($"[API] Failed to fetch {endpoint}: {request.error}");
                }

                currentData.hoppers[hopperIndex].status = "Offline";
                OnConnectionError?.Invoke($"Failed to connect to {endpoint}");
                callback?.Invoke(false);
            }
        }
    }

    public void RefreshNow()
    {
        StartCoroutine(FetchAllData());
    }

    public void SetServerUrl(string newUrl)
    {
        baseUrl = newUrl;
        Debug.Log($"[API] Server URL changed to: {newUrl}");
        RefreshNow();
    }
}

[Serializable]
public class HopperResponse
{
    public Station station;
    public HopperData[] hoppers;
    public AlarmData[] alarms;
}