using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class StationDataEntry
{
    public string station_status;
    public string recipe_code;
    public bool hopper_1_present;
    public bool hopper_1_pellets;
    public bool hopper_2_present;
    public bool hopper_2_pellets;
    public bool hopper_3_present;
    public bool hopper_3_pellets;
    public string container_type;
    public float energy_consumption;
    public string timestamp;
}

[System.Serializable]
public class StationDataList
{
    public List<StationDataEntry> data;
}

public class MockDataLoader : MonoBehaviour
{
    [Header("Data File")]
    public TextAsset jsonFile;

    private List<StationDataEntry> allData;
    private int currentIndex = 0;

    void Start()
    {
        LoadData();
    }

    void LoadData()
    {
        if (jsonFile == null)
        {
            UnityEngine.Debug.LogError("JSON file not assigned! Please assign it in Inspector.");
            return;
        }

        UnityEngine.Debug.Log("Loading data... please wait");
        float startTime = Time.realtimeSinceStartup;

        try
        {
            string json = jsonFile.text;
            string wrappedJson = "{\"data\":" + json + "}";

            StationDataList dataList = JsonUtility.FromJson<StationDataList>(wrappedJson);
            allData = dataList.data;

            float loadTime = Time.realtimeSinceStartup - startTime;
            UnityEngine.Debug.Log("✅ Loaded " + allData.Count + " entries from Mockaroo in " + loadTime.ToString("F2") + " seconds");
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("Failed to parse JSON: " + e.Message);
        }
    }

    public StationDataEntry GetNextEntry()
    {
        if (allData == null || allData.Count == 0)
        {
            UnityEngine.Debug.LogWarning("No data loaded!");
            return null;
        }

        StationDataEntry entry = allData[currentIndex];
        currentIndex = (currentIndex + 1) % allData.Count;

        return entry;
    }

    public StationDataEntry GetRandomEntry()
    {
        if (allData == null || allData.Count == 0)
        {
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, allData.Count);
        return allData[randomIndex];
    }

    public int GetDataCount()
    {
        return (allData != null) ? allData.Count : 0;
    }
}