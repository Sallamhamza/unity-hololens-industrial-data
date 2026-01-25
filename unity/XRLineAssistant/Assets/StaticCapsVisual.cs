using UnityEngine;
using TMPro;
using System.Collections.Generic; // Needed for Lists

public class StaticCapsVisual : MonoBehaviour
{
    [Header("Configuration")]
    [Range(1, 2)]
    public int fixedType = 1; // 1 = Round, 2 = Square (Set this in Inspector!)

    [Header("Cap Prefabs")]
    public GameObject roundCapPrefab;
    public GameObject squareCapPrefab;

    [Header("Container")]
    public Transform capsContainer;

    [Header("Appearance")]
    public float capScale = 100f;   // Big scale for imported models
    public float labelHeight = 2.0f; // Height of text above the cap
    public float labelScale = 0.05f; // Text size
    public bool labelFacesCamera = true;

    [Header("Colors")]
    public Color roundCapColor = new Color(0.2f, 0.6f, 1f);
    public Color squareCapColor = new Color(1f, 0.6f, 0.2f);
    public Color lowCountColor = new Color(1f, 0.3f, 0.3f);
    public int lowCountThreshold = 5;

    [Header("Floating Label")]
    public TextMeshPro countLabel;

    private GameObject staticCap;
    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main?.transform;
        if (capsContainer == null) capsContainer = this.transform;

        // --- CLEANUP: Destroy everything EXCEPT the label ---
        List<GameObject> childrenToDelete = new List<GameObject>();
        foreach (Transform child in capsContainer)
        {
            if (countLabel != null && child == countLabel.transform) continue; // Skip label
            childrenToDelete.Add(child.gameObject);
        }
        foreach (GameObject obj in childrenToDelete) Destroy(obj);
        // ---------------------------------------------------

        SpawnStaticCap();
        UpdateLabel(0);
    }

    void Update()
    {
        if (labelFacesCamera && countLabel != null && cameraTransform != null)
        {
            countLabel.transform.LookAt(cameraTransform);
            countLabel.transform.Rotate(0, 180, 0);
        }
    }

    public void SetCaps(int count, int type)
    {
        // Ignore 'type' from API, just update the number
        UpdateLabel(count);
    }

    void SpawnStaticCap()
    {
        GameObject prefab = fixedType == 1 ? roundCapPrefab : squareCapPrefab;
        if (prefab == null) return;

        staticCap = Instantiate(prefab, capsContainer);
        staticCap.transform.localPosition = Vector3.zero;
        staticCap.transform.localScale = Vector3.one * capScale;
        staticCap.transform.localRotation = Quaternion.identity;

        Renderer rend = staticCap.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            rend.material.color = fixedType == 1 ? roundCapColor : squareCapColor;
        }
    }

    void UpdateLabel(int count)
    {
        if (countLabel == null) return;

        countLabel.transform.localPosition = new Vector3(0, labelHeight, 0);
        countLabel.transform.localScale = Vector3.one * labelScale;

        string typeName = fixedType == 1 ? "Round Caps" : "Square Caps";
        countLabel.text = $"{typeName}\n{count}";
        countLabel.color = (count <= lowCountThreshold) ? lowCountColor : Color.white;
    }
}