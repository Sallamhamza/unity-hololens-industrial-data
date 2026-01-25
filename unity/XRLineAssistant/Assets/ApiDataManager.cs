using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using Debug = UnityEngine.Debug;

public class ApiDataManager : MonoBehaviour
{
    [Header("API Settings")]
    [Tooltip("Use your laptop IP when deploying to HoloLens")]
    public string baseUrl = "http://30.130.130.209:1880";
    public float refreshInterval = 3.0f;

    [Header("Endpoints")]
    [Tooltip("Station 402 - Hopper data")]
    public string station402Endpoint = "/mes/402_latest";

    [Tooltip("Station 405 - Current monitoring data")]
    public string station405Endpoint = "/mes/405_latest";

    [Tooltip("Station 405 - Feeder data")]
    public string feedersEndpoint = "/feeders";

    [Header("Debug")]
    public bool enableDebugLogs = true;

    // Data storage
    private StationData currentData;
    public StationData CurrentData => currentData;

    private Station405Data current405Data;
    public Station405Data Current405Data => current405Data;

    private FeederInfo[] currentFeeders;
    public FeederInfo[] CurrentFeeders => currentFeeders;

    private bool isConnected = false;
    public bool IsConnected => isConnected;

    // Events
    public event Action<StationData> OnDataUpdated;
    public event Action<Station405Data> On405DataUpdated;
    public event Action<FeederInfo[]> OnFeedersUpdated;
    public event Action<string> OnConnectionError;

    void Start()
    {
        currentData = new StationData();
        current405Data = new Station405Data();
        currentFeeders = new FeederInfo[0];

        if (enableDebugLogs)
        {
            Debug.Log("[API] Starting data fetch from: " + baseUrl);
            Debug.Log("[API] Station 402 endpoint: " + station402Endpoint);
            Debug.Log("[API] Station 405 endpoint: " + station405Endpoint);
            Debug.Log("[API] Feeders endpoint: " + feedersEndpoint);
        }

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
        bool station402Success = false;
        bool station405Success = false;
        bool feedersSuccess = false;

        // Fetch Station 402 (Hopper data)
        yield return StartCoroutine(FetchStation402Data((success) => station402Success = success));

        // Fetch Station 405 (Current monitoring)
        yield return StartCoroutine(FetchStation405Data((success) => station405Success = success));

        // Fetch Feeders (Station 405)
        yield return StartCoroutine(FetchFeederData((success) => feedersSuccess = success));

        isConnected = station402Success || station405Success || feedersSuccess;

        if (!isConnected)
        {
            OnConnectionError?.Invoke("Connection Lost");
        }
    }

    IEnumerator FetchStation402Data(Action<bool> callback)
    {
        string url = baseUrl + station402Endpoint;

        if (enableDebugLogs)
            Debug.Log("[API] Fetching Station 402: " + url);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.timeout = 10;
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    string json = request.downloadHandler.text;

                    if (enableDebugLogs)
                        Debug.Log("[API] Station 402 response: " + json);

                    StationResponse response = JsonUtility.FromJson<StationResponse>(json);

                    if (response != null && response.station != null)
                    {
                        currentData.stationId = response.station.id;
                        currentData.status = response.station.status;
                        currentData.recipe = response.station.recipe;
                        currentData.current = response.station.current;  // CAPTURE CURRENT

                        // Hopper 1 (Blue)
                        if (response.station.hopper1 != null)
                        {
                            currentData.hopper1Present = response.station.hopper1.present;
                            currentData.hopper1Full = response.station.hopper1.minPellets;
                        }

                        // Hopper 2 (Yellow)
                        if (response.station.hopper2 != null)
                        {
                            currentData.hopper2Present = response.station.hopper2.present;
                            currentData.hopper2Full = response.station.hopper2.minPellets;
                        }

                        // Hopper 3 (Red)
                        if (response.station.hopper3 != null)
                        {
                            currentData.hopper3Present = response.station.hopper3.present;
                            currentData.hopper3Full = response.station.hopper3.minPellets;
                        }

                        if (enableDebugLogs)
                        {
                            Debug.Log($"[API] 402 Status: {currentData.status}");
                            Debug.Log($"[API] 402 Recipe: {currentData.recipe}");
                            Debug.Log($"[API] 402 Current: {currentData.current}A");
                            Debug.Log($"[API] Hopper1: Present={currentData.hopper1Present}, Full={currentData.hopper1Full}");
                            Debug.Log($"[API] Hopper2: Present={currentData.hopper2Present}, Full={currentData.hopper2Full}");
                            Debug.Log($"[API] Hopper3: Present={currentData.hopper3Present}, Full={currentData.hopper3Full}");
                        }

                        OnDataUpdated?.Invoke(currentData);
                        callback?.Invoke(true);
                    }
                    else
                    {
                        if (enableDebugLogs)
                            Debug.LogWarning("[API] Station 402 response was null or invalid");
                        callback?.Invoke(false);
                    }
                }
                catch (Exception e)
                {
                    if (enableDebugLogs)
                        Debug.LogError("[API] Station 402 parse error: " + e.Message);
                    callback?.Invoke(false);
                }
            }
            else
            {
                if (enableDebugLogs)
                    Debug.LogError("[API] Station 402 fetch failed: " + request.error);
                callback?.Invoke(false);
            }
        }
    }

    IEnumerator FetchStation405Data(Action<bool> callback)
    {
        string url = baseUrl + station405Endpoint;

        if (enableDebugLogs)
            Debug.Log("[API] Fetching Station 405: " + url);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.timeout = 10;
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    string json = request.downloadHandler.text;

                    if (enableDebugLogs)
                        Debug.Log("[API] Station 405 RAW response: " + json);

                    StationResponse response = JsonUtility.FromJson<StationResponse>(json);

                    if (response != null && response.station != null)
                    {
                        if (current405Data == null)
                            current405Data = new Station405Data();

                        current405Data.stationId = response.station.id;
                        current405Data.status = response.station.status;
                        current405Data.recipe = response.station.recipe;
                        current405Data.current = response.station.current;
                        current405Data.timestamp = response.station.timestamp;

                        // Check for alarms
                        current405Data.hasAlarm = false;
                        current405Data.alarmMessage = "No Alarms";

                        if (response.alarms != null && response.alarms.Length > 0)
                        {
                            if (enableDebugLogs)
                                Debug.Log($"[API] 405 Found {response.alarms.Length} alarm entries");

                            foreach (var alarm in response.alarms)
                            {
                                if (enableDebugLogs)
                                    Debug.Log($"[API] 405 Alarm: severity={alarm.severity}, message={alarm.message}");

                                // Only treat warning/error/critical as real alarms, not "info"
                                if (alarm.severity != "info")
                                {
                                    current405Data.hasAlarm = true;
                                    current405Data.alarmMessage = alarm.message;
                                    break;
                                }
                            }
                        }

                        if (enableDebugLogs)
                        {
                            Debug.Log($"[API] 405 Parsed - ID: {current405Data.stationId}");
                            Debug.Log($"[API] 405 Parsed - Status: {current405Data.status}");
                            Debug.Log($"[API] 405 Parsed - Recipe: {current405Data.recipe}");
                            Debug.Log($"[API] 405 Parsed - Current: {current405Data.current}A");
                            Debug.Log($"[API] 405 Parsed - HasAlarm: {current405Data.hasAlarm}");
                        }

                        On405DataUpdated?.Invoke(current405Data);
                        callback?.Invoke(true);
                    }
                    else
                    {
                        if (enableDebugLogs)
                            Debug.LogWarning("[API] Station 405 response.station was null");
                        callback?.Invoke(false);
                    }
                }
                catch (Exception e)
                {
                    if (enableDebugLogs)
                        Debug.LogError("[API] Station 405 parse error: " + e.Message + "\nStack: " + e.StackTrace);
                    callback?.Invoke(false);
                }
            }
            else
            {
                if (enableDebugLogs)
                    Debug.LogError("[API] Station 405 fetch failed: " + request.error);
                callback?.Invoke(false);
            }
        }
    }

    IEnumerator FetchFeederData(Action<bool> callback)
    {
        string url = baseUrl + feedersEndpoint;

        if (enableDebugLogs)
            Debug.Log("[API] Fetching Feeders: " + url);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.timeout = 10;
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    string json = request.downloadHandler.text;

                    if (enableDebugLogs)
                        Debug.Log("[API] Feeders response: " + json);

                    FeederResponse response = JsonUtility.FromJson<FeederResponse>(json);

                    if (response != null && response.feeders != null)
                    {
                        currentFeeders = response.feeders;

                        if (enableDebugLogs)
                        {
                            foreach (var feeder in currentFeeders)
                            {
                                Debug.Log($"[API] Feeder {feeder.feeder}: {feeder.caps} caps, type={feeder.type}");
                            }
                        }

                        OnFeedersUpdated?.Invoke(currentFeeders);
                        callback?.Invoke(true);
                    }
                    else
                    {
                        if (enableDebugLogs)
                            Debug.LogWarning("[API] Feeders response was null or invalid");
                        callback?.Invoke(false);
                    }
                }
                catch (Exception e)
                {
                    if (enableDebugLogs)
                        Debug.LogError("[API] Feeders parse error: " + e.Message);
                    callback?.Invoke(false);
                }
            }
            else
            {
                if (enableDebugLogs)
                    Debug.LogError("[API] Feeders fetch failed: " + request.error);
                callback?.Invoke(false);
            }
        }
    }
}

// ============ DATA CLASSES ============

[Serializable]
public class StationData
{
    public string stationId;
    public string status;
    public string recipe;
    public float current;  // ADDED CURRENT FIELD
    public bool hopper1Present;
    public bool hopper1Full;
    public bool hopper2Present;
    public bool hopper2Full;
    public bool hopper3Present;
    public bool hopper3Full;
}

[Serializable]
public class Station405Data
{
    public string stationId;
    public string status;
    public string recipe;
    public float current;
    public bool hasAlarm;
    public string alarmMessage;
    public string timestamp;
}

[Serializable]
public class StationResponse
{
    public StationInfo station;
    public AlarmInfo[] alarms;
}

[Serializable]
public class StationInfo
{
    public string id;
    public string status;
    public string recipe;
    public string timestamp;
    public float runningTime;
    public float current;
    public HopperInfo hopper1;
    public HopperInfo hopper2;
    public HopperInfo hopper3;
}

[Serializable]
public class HopperInfo
{
    public bool present;
    public bool minPellets;
}

[Serializable]
public class AlarmInfo
{
    public string id;
    public string severity;
    public string message;
    public string timestamp;
}

[Serializable]
public class FeederResponse
{
    public FeederInfo[] feeders;
}

[Serializable]
public class FeederInfo
{
    public int feeder;
    public int caps;
    public int type;
    public bool simulated;
}