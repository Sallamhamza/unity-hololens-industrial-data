using UnityEngine;

[System.Serializable]
public class HopperData
{
    public int position;
    public bool isPresent;
    public bool hasPellets;
    public string color;
}

[System.Serializable]
public class StationData
{
    public string status;
    public string recipe;
    public string containerType;
    public HopperData[] hoppers;
    public float energyConsumption;
    public string timestamp;
}

[System.Serializable]
public class MessageData
{
    public string timestamp;
    public string messageType;
    public string text;
}

public class MockDataManager : MonoBehaviour
{
    [Header("Mockaroo Data")]
    public MockDataLoader dataLoader;

    [Header("Settings")]
    public float updateInterval = 3f;

    private StationData currentData;

    void Start()
    {
        InitializeData();
        InvokeRepeating("UpdateFromMockaroo", 2f, updateInterval);
    }

    void InitializeData()
    {
        currentData = new StationData();
        currentData.status = "Station is ready to run";
        currentData.recipe = "NULL";
        currentData.containerType = "square";
        currentData.hoppers = new HopperData[3];
        currentData.energyConsumption = 0f;
        currentData.timestamp = System.DateTime.Now.ToString("HH:mm:ss");

        for (int i = 0; i < 3; i++)
        {
            currentData.hoppers[i] = new HopperData();
            currentData.hoppers[i].position = i + 1;
            currentData.hoppers[i].isPresent = true;
            currentData.hoppers[i].hasPellets = true;

            if (i == 0) currentData.hoppers[i].color = "blue";
            else if (i == 1) currentData.hoppers[i].color = "yellow";
            else currentData.hoppers[i].color = "red";
        }

        UnityEngine.Debug.Log("MockDataManager initialized with default data");
    }

    void UpdateFromMockaroo()
    {
        if (dataLoader == null)
        {
            UnityEngine.Debug.LogWarning("DataLoader not assigned to MockDataManager!");
            return;
        }

        StationDataEntry entry = dataLoader.GetNextEntry();

        if (entry != null)
        {
            currentData.status = entry.station_status;
            currentData.recipe = entry.recipe_code;
            currentData.containerType = entry.container_type;
            currentData.energyConsumption = entry.energy_consumption;
            currentData.timestamp = entry.timestamp;

            currentData.hoppers[0].position = 1;
            currentData.hoppers[0].isPresent = entry.hopper_1_present;
            currentData.hoppers[0].hasPellets = entry.hopper_1_pellets;
            currentData.hoppers[0].color = "blue";

            currentData.hoppers[1].position = 2;
            currentData.hoppers[1].isPresent = entry.hopper_2_present;
            currentData.hoppers[1].hasPellets = entry.hopper_2_pellets;
            currentData.hoppers[1].color = "yellow";

            currentData.hoppers[2].position = 3;
            currentData.hoppers[2].isPresent = entry.hopper_3_present;
            currentData.hoppers[2].hasPellets = entry.hopper_3_pellets;
            currentData.hoppers[2].color = "red";

            UnityEngine.Debug.Log("Updated to: " + currentData.status + " | Recipe: " + currentData.recipe);
        }
        else
        {
            UnityEngine.Debug.LogWarning("No data entry available from DataLoader");
        }
    }

    public StationData GetStationData()
    {
        return currentData;
    }

    public void ProcessCommand(string command)
    {
        UnityEngine.Debug.Log("Processing command: " + command);

        switch (command.ToUpper())
        {
            case "START":
                currentData.status = "Station is running";
                UnityEngine.Debug.Log("Station started");
                break;

            case "STOP":
                currentData.status = "Station is ready to run";
                currentData.recipe = "NULL";
                UnityEngine.Debug.Log("Station stopped");
                break;

            case "RESET":
                currentData.status = "Station is resetting Step 0";
                UnityEngine.Debug.Log("Station resetting");
                break;

            case "ALARM":
                currentData.status = "Alarm acknowledged";
                UnityEngine.Debug.Log("Alarm acknowledged");
                break;

            default:
                UnityEngine.Debug.LogWarning("Unknown command: " + command);
                break;
        }
    }
}