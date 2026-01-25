using UnityEngine;
using TMPro;
using Debug = UnityEngine.Debug;

public class HopperVisual3D : MonoBehaviour
{
    [Header("Hopper Settings")]
    public string hopperColor = "blue";  // "blue", "yellow", or "red"

    [Header("3D Objects")]
    public GameObject hopperContainer;
    public GameObject hopperFill;
    public TextMeshPro statusLabel;

    [Header("Materials")]
    public Material fullMaterial;
    public Material emptyMaterial;
    public Material missingMaterial;
    public Material hopperBaseMaterial;

    [Header("Colors (if no materials assigned)")]
    public Color fullColor = new Color(0.2f, 0.8f, 0.2f);
    public Color emptyColor = new Color(0.8f, 0.8f, 0.2f);
    public Color missingColor = new Color(0.8f, 0.2f, 0.2f);

    [Header("Animation")]
    public float animationSpeed = 3f;
    public bool enablePulseWhenEmpty = true;
    public float pulseSpeed = 2f;

    [Header("Billboard")]
    public bool labelFacesCamera = true;

    private bool isPresent = true;
    private bool isFull = true;
    private Renderer fillRenderer;
    private Vector3 fullScale;
    private Vector3 emptyScale;
    private Transform cameraTransform;
    private float pulseTimer;

    void Start()
    {
        cameraTransform = Camera.main?.transform;

        if (hopperFill != null)
        {
            fillRenderer = hopperFill.GetComponent<Renderer>();
            fullScale = hopperFill.transform.localScale;
            emptyScale = new Vector3(fullScale.x, 0.05f, fullScale.z);
        }

        if (fullMaterial == null)
        {
            fullMaterial = new Material(Shader.Find("Standard"));
            fullMaterial.color = fullColor;
        }

        if (emptyMaterial == null)
        {
            emptyMaterial = new Material(Shader.Find("Standard"));
            emptyMaterial.color = emptyColor;
        }

        if (missingMaterial == null)
        {
            missingMaterial = new Material(Shader.Find("Standard"));
            missingMaterial.color = missingColor;
        }

        UpdateVisual();
    }

    void Update()
    {
        if (labelFacesCamera && statusLabel != null && cameraTransform != null)
        {
            statusLabel.transform.LookAt(cameraTransform);
            statusLabel.transform.Rotate(0, 180, 0);
        }

        if (enablePulseWhenEmpty && !isFull && fillRenderer != null)
        {
            pulseTimer += Time.deltaTime * pulseSpeed;
            float pulse = 0.7f + Mathf.Sin(pulseTimer) * 0.3f;
            Color baseColor = isPresent ? emptyColor : missingColor;
            Color pulseColor = baseColor * pulse;
            pulseColor.a = 1f;
            fillRenderer.material.color = pulseColor;
        }
    }

    // New method to set status using present and full flags
    public void SetStatus(bool present, bool full)
    {
        if (isPresent != present || isFull != full)
        {
            isPresent = present;
            isFull = full;
            StartCoroutine(AnimateStateChange());
        }
    }

    // Legacy method - still works
    public void SetState(bool full)
    {
        SetStatus(true, full);
    }

    System.Collections.IEnumerator AnimateStateChange()
    {
        if (hopperFill == null) yield break;

        Vector3 startScale = hopperFill.transform.localScale;
        Vector3 targetScale = isFull ? fullScale : emptyScale;

        Material targetMaterial;
        if (!isPresent)
            targetMaterial = missingMaterial;
        else if (isFull)
            targetMaterial = fullMaterial;
        else
            targetMaterial = emptyMaterial;

        if (fillRenderer != null)
            fillRenderer.material = targetMaterial;

        float elapsed = 0f;
        float duration = 1f / animationSpeed;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);
            hopperFill.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        hopperFill.transform.localScale = targetScale;
        UpdateLabel();

        string status = !isPresent ? "MISSING" : (isFull ? "FULL" : "LOW");
        Debug.Log($"[HOPPER] {hopperColor} hopper is now {status}");
    }

    void UpdateVisual()
    {
        if (hopperFill != null)
        {
            hopperFill.transform.localScale = isFull ? fullScale : emptyScale;

            if (fillRenderer != null)
            {
                if (!isPresent)
                    fillRenderer.material = missingMaterial;
                else if (isFull)
                    fillRenderer.material = fullMaterial;
                else
                    fillRenderer.material = emptyMaterial;
            }
        }

        UpdateLabel();
    }

    void UpdateLabel()
    {
        if (statusLabel != null)
        {
            string status = !isPresent ? "MISSING" : (isFull ? "FULL" : "LOW");
            statusLabel.text = $"{hopperColor.ToUpper()}\n{status}";

            if (!isPresent)
                statusLabel.color = missingColor;
            else if (isFull)
                statusLabel.color = fullColor;
            else
                statusLabel.color = emptyColor;
        }
    }

    // Updated method to work with new HopperInfo class
    public void UpdateFromHopperInfo(bool present, bool minPellets)
    {
        SetStatus(present, minPellets);
    }

    [ContextMenu("Test - Set Full")]
    public void TestSetFull()
    {
        SetStatus(true, true);
    }

    [ContextMenu("Test - Set Low")]
    public void TestSetLow()
    {
        SetStatus(true, false);
    }

    [ContextMenu("Test - Set Missing")]
    public void TestSetMissing()
    {
        SetStatus(false, false);
    }
}