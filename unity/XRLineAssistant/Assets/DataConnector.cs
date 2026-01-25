using UnityEngine;
using Debug = UnityEngine.Debug;

public class DataConnector : MonoBehaviour
{
    [Header("API Manager")]
    public ApiDataManager apiManager;

    [Header("Hopper Visualizers (3D)")]
    public HopperVisual3D hopper1Visual;  // Blue
    public HopperVisual3D hopper2Visual;  // Yellow
    public HopperVisual3D hopper3Visual;  // Red

    [Header("Feeder Visualizers (3D)")]
    public CapsVisual3D feeder1Visual;    // Round caps
    public CapsVisual3D feeder2Visual;    // Square caps

    [Header("Feeder Display (UI)")]
    public FeederDisplay feederDisplay;

    void Start()
    {
        // Find ApiDataManager if not assigned
        if (apiManager == null)
            apiManager = FindObjectOfType<ApiDataManager>();

        if (apiManager != null)
        {
            // Subscribe to events
            apiManager.OnDataUpdated += HandleStationUpdate;
            apiManager.OnFeedersUpdated += HandleFeedersUpdate;
            apiManager.OnConnectionError += HandleError;

            Debug.Log("[Connector] Connected to ApiDataManager");
        }
        else
        {
            Debug.LogError("[Connector] ApiDataManager not found!");
        }
    }

    void OnDestroy()
    {
        if (apiManager != null)
        {
            apiManager.OnDataUpdated -= HandleStationUpdate;
            apiManager.OnFeedersUpdated -= HandleFeedersUpdate;
            apiManager.OnConnectionError -= HandleError;
        }
    }

    void HandleStationUpdate(StationData data)
    {
        Debug.Log("[Connector] Station 402 data received");

        // Update Hopper 1 (Blue)
        if (hopper1Visual != null)
        {
            hopper1Visual.SetStatus(data.hopper1Present, data.hopper1Full);
            Debug.Log($"[Connector] Hopper1: Present={data.hopper1Present}, Full={data.hopper1Full}");
        }

        // Update Hopper 2 (Yellow)
        if (hopper2Visual != null)
        {
            hopper2Visual.SetStatus(data.hopper2Present, data.hopper2Full);
            Debug.Log($"[Connector] Hopper2: Present={data.hopper2Present}, Full={data.hopper2Full}");
        }

        // Update Hopper 3 (Red)
        if (hopper3Visual != null)
        {
            hopper3Visual.SetStatus(data.hopper3Present, data.hopper3Full);
            Debug.Log($"[Connector] Hopper3: Present={data.hopper3Present}, Full={data.hopper3Full}");
        }
    }

    void HandleFeedersUpdate(FeederInfo[] feeders)
    {
        Debug.Log("[Connector] Feeder data received: " + feeders.Length + " feeders");

        foreach (var feeder in feeders)
        {
            // Update 3D Visualizers
            if (feeder.feeder == 1 && feeder1Visual != null)
            {
                feeder1Visual.SetCaps(feeder.caps, feeder.type);
                Debug.Log($"[Connector] Feeder 1 (Round): {feeder.caps} caps");
            }
            else if (feeder.feeder == 2 && feeder2Visual != null)
            {
                feeder2Visual.SetCaps(feeder.caps, feeder.type);
                Debug.Log($"[Connector] Feeder 2 (Square): {feeder.caps} caps");
            }
        }

        // Update UI Display
        if (feederDisplay != null)
        {
            feederDisplay.UpdateFromFeeders(feeders);
        }
    }

    void HandleError(string error)
    {
        Debug.LogError("[Connector] Connection error: " + error);
    }
}