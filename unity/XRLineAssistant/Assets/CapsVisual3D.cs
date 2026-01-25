using UnityEngine;
using TMPro;

public class CapsVisual3D : MonoBehaviour
{
    [Header("Configuration")]
    [Range(1, 2)]
    public int fixedType = 1; // 1 = Round, 2 = Square. SET THIS IN INSPECTOR!

    [Header("Cap Prefabs")]
    public GameObject roundCapPrefab;
    public GameObject squareCapPrefab;

    [Header("Container")]
    public Transform capsContainer;

    [Header("Appearance")]
    public float capScale = 100f;    // Scale for 0.01 imported models
    public float labelHeight = 2.0f; // Height of text above cap
    public float labelScale = 0.05f; // Text size
    public bool labelFacesCamera = true;

    [Header("Colors")]
    public Color roundCapColor = new Color(0.2f, 0.6f, 1f);
    public Color squareCapColor = new Color(1f, 0.6f, 0.2f);
    public Color lowCountColor = new Color(1f, 0.3f, 0.3f);
    public int lowCountThreshold = 5;

    [Header("Floating Label")]
    public TextMeshPro countLabel;

    // This variable holds the ONE single cap.
    private GameObject displayObject;
    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main?.transform;
        if (capsContainer == null) capsContainer = this.transform;

        // Create the single cap immediately
        RefreshStaticCap();

        // Initialize label
        UpdateLabel(0);
    }

    void Update()
    {
        // Make label face camera
        if (labelFacesCamera && countLabel != null && cameraTransform != null)
        {
            countLabel.transform.LookAt(cameraTransform);
            countLabel.transform.Rotate(0, 180, 0);
        }
    }

    // Called by ApiDataManager
    public void SetCaps(int count, int type)
    {
        // We ignore the 'type' from the API because we set it manually in the Inspector using 'fixedType'.
        // We only use the count.
        UpdateLabel(count);
    }

    void RefreshStaticCap()
    {
        // SAFETY CHECK: If we already have a cap, DO NOT spawn another one.
        if (displayObject != null) return;

        // Choose prefab based on the Fixed Type setting
        GameObject prefab = fixedType == 1 ? roundCapPrefab : squareCapPrefab;
        if (prefab == null) return;

        // Spawn exactly one
        displayObject = Instantiate(prefab, capsContainer);
        displayObject.transform.localPosition = Vector3.zero;
        displayObject.transform.localScale = Vector3.one * capScale;
        displayObject.transform.localRotation = Quaternion.identity;

        // Color it
        Renderer rend = displayObject.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            rend.material.color = fixedType == 1 ? roundCapColor : squareCapColor;
        }
    }

    void UpdateLabel(int count)
    {
        if (countLabel == null) return;

        // Enforce position and scale every time data changes
        countLabel.transform.localPosition = new Vector3(0, labelHeight, 0);
        countLabel.transform.localScale = Vector3.one * labelScale;

        string typeName = fixedType == 1 ? "Round" : "Square";
        countLabel.text = $"{typeName}\n{count}";

        // Turn red if low inventory
        countLabel.color = (count <= lowCountThreshold) ? lowCountColor : Color.white;
    }
}