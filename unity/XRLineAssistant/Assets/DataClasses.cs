using System;

[Serializable]
public class StationData
{
    public Station station;
    public AlarmData[] alarms;
}

[Serializable]
public class Station
{
    public string id;
    public string status;
    public string recipe;
    public string timestamp;

    public int runningTime;    // NEW
    public float current;      // NEW
}

[Serializable]
public class AlarmData
{
    public string id;
    public string severity;
    public string message;
    public string timestamp;
}
