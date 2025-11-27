using System;

// Data structure classes used by both MockDataLoader and ApiDataManager

[Serializable]
public class StationData
{
    public Station station;
    public HopperData[] hoppers;
    public AlarmData[] alarms;
}

[Serializable]
public class Station
{
    public string id;
    public string status;
    public string currentRecipe;
    public int productionCount;
    public float efficiency;
}

[Serializable]
public class HopperData
{
    public string id;
    public string color;
    public int level;
    public string status;
    public bool needsRefill;
}

[Serializable]
public class AlarmData
{
    public string id;
    public string type;
    public string message;
    public string timestamp;
    public string severity;
}