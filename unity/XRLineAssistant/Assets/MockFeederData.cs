using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class MockFeederData : MonoBehaviour
{
    [Header("UI References (Optional)")]
    public FeederDisplay feederDisplay;
    public StationInfoDisplay stationDisplay;

    [Header("Mock Feeder Data")]
    [Range(0, 50)]
    public int feeder1Caps = 12;
    public int feeder1Type = 1;  // 1 = round, 2 = square
    [Range(0, 50)]
    public int feeder2Caps = 8;
    public int feeder2Type = 2;  // 1 = round, 2 = square

    [Header("Mock Hopper Data (True = Full, False = Empty)")]
    public bool blueHopperFull = true;
    public bool yellowHopperFull = true;
    public bool redHopperFull = false;

    [Header("3D Hopper Visuals")]
    public HopperVisual3D blueHopperVisual;
    public HopperVisual3D yellowHopperVisual;
    public HopperVisual3D redHopperVisual;

    [Header("3D Caps Visuals")]
    public CapsVisual3D feeder1Visual;
    public CapsVisual3D feeder2Visual;

    [Header("Settings")]
    public bool autoUpdate = false;
    public float updateInterval = 2f;

    private float timer;

    void Start()
    {
        Debug.Log("=== MOCK DATA INITIALIZED ===");
        UpdateMockData();
    }

    void Update()
    {
        if (autoUpdate)
        {
            timer += Time.deltaTime;
            if (timer >= updateInterval)
            {
                timer = 0f;
                RandomizeData();
                UpdateMockData();
            }
        }
    }

    void RandomizeData()
    {
        feeder1Caps = Mathf.Clamp(feeder1Caps + Random.Range(-2, 3), 0, 50);
        feeder2Caps = Mathf.Clamp(feeder2Caps + Random.Range(-2, 3), 0, 50);

        if (Random.value < 0.1f) blueHopperFull = !blueHopperFull;
        if (Random.value < 0.1f) yellowHopperFull = !yellowHopperFull;
        if (Random.value < 0.1f) redHopperFull = !redHopperFull;
    }

    [ContextMenu("Update Mock Data")]
    public void UpdateMockData()
    {
        // Update 3D Hopper Visuals
        if (blueHopperVisual != null)
            blueHopperVisual.SetState(blueHopperFull);

        if (yellowHopperVisual != null)
            yellowHopperVisual.SetState(yellowHopperFull);

        if (redHopperVisual != null)
            redHopperVisual.SetState(redHopperFull);

        // Update 3D Caps Visuals
        if (feeder1Visual != null)
            feeder1Visual.SetCaps(feeder1Caps, feeder1Type);

        if (feeder2Visual != null)
            feeder2Visual.SetCaps(feeder2Caps, feeder2Type);

        Debug.Log("[MOCK] Hoppers: B=" + blueHopperFull + " Y=" + yellowHopperFull + " R=" + redHopperFull);
        Debug.Log("[MOCK] Feeders: F1=" + feeder1Caps + " caps, F2=" + feeder2Caps + " caps");
    }

    [ContextMenu("Test - All Hoppers Full")]
    public void TestHoppersFull()
    {
        blueHopperFull = true;
        yellowHopperFull = true;
        redHopperFull = true;
        UpdateMockData();
    }

    [ContextMenu("Test - All Hoppers Empty")]
    public void TestHoppersEmpty()
    {
        blueHopperFull = false;
        yellowHopperFull = false;
        redHopperFull = false;
        UpdateMockData();
    }

    [ContextMenu("Test - 20 Round Caps")]
    public void TestRoundCaps()
    {
        feeder1Caps = 20;
        feeder1Type = 1;
        UpdateMockData();
    }

    [ContextMenu("Test - 15 Square Caps")]
    public void TestSquareCaps()
    {
        feeder2Caps = 15;
        feeder2Type = 2;
        UpdateMockData();
    }
}