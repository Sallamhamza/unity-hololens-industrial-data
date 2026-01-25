using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class StartupManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject startupMenu;
    public GameObject controlPanel402;
    public GameObject controlPanel405;

    [Header("3D Hopper Objects (Show after START)")]
    public GameObject blueHopper;
    public GameObject yellowHopper;
    public GameObject redHopper;

    [Header("3D Feeder Visualizers (Show after START)")]
    public GameObject visualizerRound;
    public GameObject visualizerSquare;

    [Header("Buttons (Unity UI)")]
    public Button startButton;
    public Button settingsButton;
    public Button exitButton;

    void Start()
    {
        Debug.Log("=== STARTUP MANAGER INITIALIZED ===");

        // Show startup menu
        if (startupMenu != null) startupMenu.SetActive(true);

        // Hide control panels
        if (controlPanel402 != null) controlPanel402.SetActive(false);
        if (controlPanel405 != null) controlPanel405.SetActive(false);

        // Hide 3D hopper objects
        if (blueHopper != null) blueHopper.SetActive(false);
        if (yellowHopper != null) yellowHopper.SetActive(false);
        if (redHopper != null) redHopper.SetActive(false);

        // Hide BOTH feeder visualizers
        if (visualizerRound != null) visualizerRound.SetActive(false);
        if (visualizerSquare != null) visualizerSquare.SetActive(false);

        Debug.Log("[OK] 3D objects hidden");

        // Connect buttons
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
            Debug.Log("[OK] Start button connected");
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
            Debug.Log("[OK] Exit button connected");
        }
    }

    void OnStartButtonClicked()
    {
        Debug.Log("[START] Button clicked - Showing panels and 3D objects");

        // Hide startup menu
        if (startupMenu != null) startupMenu.SetActive(false);

        // Show control panels
        if (controlPanel402 != null) controlPanel402.SetActive(true);
        if (controlPanel405 != null) controlPanel405.SetActive(true);

        // Show hopper 3D objects
        if (blueHopper != null) blueHopper.SetActive(true);
        if (yellowHopper != null) yellowHopper.SetActive(true);
        if (redHopper != null) redHopper.SetActive(true);

        // Show BOTH feeder visualizers
        if (visualizerRound != null) visualizerRound.SetActive(true);
        if (visualizerSquare != null) visualizerSquare.SetActive(true);

        Debug.Log("[SUCCESS] All panels and 3D objects shown!");
    }

    void OnExitButtonClicked()
    {
        Debug.Log("[EXIT] Button clicked - Quitting application");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        UnityEngine.Application.Quit();
#endif
    }
}