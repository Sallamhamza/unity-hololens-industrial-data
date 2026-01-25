using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Ambiguity Fix
using Debug = UnityEngine.Debug;
using Application = UnityEngine.Application;

public class HoloLensCameraCapture : MonoBehaviour
{
    [Header("AI Integration")]
    public AI aiClient;

    [Header("Manual Upload")]
    public Texture2D manualUploadImage;

    [Header("UI References")]
    public RawImage previewImage;

    // This is the ONLY text field we will update with the result
    public TextMeshProUGUI containerTypeText;

    private Texture2D currentImage;
    private bool isBusy = false;

    void Start()
    {
        if (aiClient == null) aiClient = FindObjectOfType<AI>();

        // Clear text on start
        if (containerTypeText != null) containerTypeText.text = "";
    }

    // --- BUTTONS ---

    public void OnUploadButton()
    {
        if (isBusy) return;
        if (manualUploadImage == null) return;

        currentImage = manualUploadImage;
        UpdatePreview(currentImage);
        SendToAI();
    }

    public void OnCaptureButton()
    {
        if (isBusy) return;
        StartCoroutine(CaptureRoutine());
    }

    // --- CAPTURE LOGIC ---

    private IEnumerator CaptureRoutine()
    {
        isBusy = true;

        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            isBusy = false;
            yield break;
        }

        WebCamTexture webcam = new WebCamTexture(1280, 720);
        webcam.Play();

        float timeout = 5f;
        while (!webcam.didUpdateThisFrame && timeout > 0)
        {
            timeout -= Time.deltaTime;
            yield return null;
        }

        if (!webcam.didUpdateThisFrame)
        {
            webcam.Stop();
            isBusy = false;
            yield break;
        }

        yield return new WaitForEndOfFrame();

        currentImage = new Texture2D(webcam.width, webcam.height, TextureFormat.RGB24, false);
        currentImage.SetPixels(webcam.GetPixels());
        currentImage.Apply();

        webcam.Stop();
        UpdatePreview(currentImage);
        SendToAI();
    }

    // --- AI PROCESSING ---

    private void SendToAI()
    {
        if (aiClient == null)
        {
            Debug.LogError("AI Client missing");
            isBusy = false;
            return;
        }

        isBusy = true;
        // NOTE: We do NOT update text here to avoid "Analysis..." messages
        aiClient.RunAnalysis(currentImage, OnAnalysisSuccess, OnAnalysisError);
    }

    private void OnAnalysisSuccess(List<AI.PredictionResult> results)
    {
        isBusy = false;

        if (containerTypeText == null) return;

        if (results == null || results.Count == 0)
        {
            containerTypeText.text = ""; // Clear text if nothing found
            return;
        }

        // Get the first result
        string className = results[0].className.ToLower();

        // STRICT DISPLAY LOGIC
        if (className.Contains("round") || className.Contains("circle"))
        {
            containerTypeText.text = "ROUND";
            containerTypeText.color = new Color(0.2f, 0.6f, 1f); // Blue
        }
        else if (className.Contains("square") || className.Contains("rect"))
        {
            containerTypeText.text = "SQUARED";
            containerTypeText.color = new Color(1f, 0.6f, 0.2f); // Orange
        }
        else
        {
            // If it detects something else, just show the name in uppercase
            containerTypeText.text = className.ToUpper();
            containerTypeText.color = Color.white;
        }
    }

    private void OnAnalysisError(string error)
    {
        isBusy = false;
        Debug.LogError("AI Error: " + error);
        // Optional: Clear text on error so it doesn't show old results
        // if (containerTypeText != null) containerTypeText.text = ""; 
    }

    // --- UI HELPERS ---

    private void UpdatePreview(Texture2D tex)
    {
        if (previewImage != null)
        {
            previewImage.texture = tex;
            previewImage.gameObject.SetActive(true);
        }
    }
}