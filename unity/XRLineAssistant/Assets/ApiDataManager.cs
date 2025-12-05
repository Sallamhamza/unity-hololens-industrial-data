using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;

public class ApiDataManager : MonoBehaviour
{
    [Header("API Settings")]
    public string baseUrl = "http://130.130.130.201:1880";
    public string endpoint = "/mes/402_latest";
    public float refreshInterval = 2f;

    public StationData CurrentData { get; private set; }
    public bool IsConnected { get; private set; }

    public event Action<StationData> OnDataUpdated;
    public event Action<string> OnConnectionError;

    void Start()
    {
        CurrentData = new StationData();
        StartCoroutine(FetchLoop());
    }

    IEnumerator FetchLoop()
    {
        while (true)
        {
            yield return FetchData();
            yield return new WaitForSeconds(refreshInterval);
        }
    }

    IEnumerator FetchData()
    {
        string url = baseUrl + endpoint;

        using (var req = UnityWebRequest.Get(url))
        {
            req.timeout = 5;
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                IsConnected = false;
                OnConnectionError?.Invoke("Connection error: " + req.error);
                yield break;
            }

            try
            {
                string json = req.downloadHandler.text;

                StationData parsed = JsonUtility.FromJson<StationData>(json);
                CurrentData = parsed;

                IsConnected = true;
                OnDataUpdated?.Invoke(CurrentData);
            }
            catch (Exception e)
            {
                IsConnected = false;
                OnConnectionError?.Invoke("JSON parse error: " + e.Message);
            }
        }
    }
}
