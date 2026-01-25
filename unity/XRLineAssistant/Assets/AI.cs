using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Ambiguity Fix
using Debug = UnityEngine.Debug;

public class AI : MonoBehaviour
{
    [Header("Roboflow Settings")]
    [SerializeField] private string workflowUrl = "http://172.20.10.11:9001/infer/workflows/xrproject/custom-workflow";
    [SerializeField] private string apiKey = "M9oW1Va2Kqrj5Ybt2DJa";
    [SerializeField] private int timeoutSeconds = 60;

    public void RunAnalysis(Texture2D image, Action<List<PredictionResult>> onSuccess, Action<string> onError)
    {
        if (image == null)
        {
            onError?.Invoke("No image");
            return;
        }
        StartCoroutine(SendRequestRoutine(image, onSuccess, onError));
    }

    private IEnumerator SendRequestRoutine(Texture2D image, Action<List<PredictionResult>> onSuccess, Action<string> onError)
    {
        byte[] imageBytes = image.EncodeToJPG(85);
        string base64Image = Convert.ToBase64String(imageBytes);

        WorkflowRequest req = new WorkflowRequest
        {
            api_key = this.apiKey,
            inputs = new Inputs { image = new ImageInput { type = "base64", value = base64Image } }
        };

        string jsonBody = JsonUtility.ToJson(req);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);

        using (UnityWebRequest www = new UnityWebRequest(workflowUrl, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.timeout = timeoutSeconds;

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke($"Network Error: {www.error}");
            }
            else
            {
                ParseResponse(www.downloadHandler.text, onSuccess, onError);
            }
        }
    }

    private void ParseResponse(string json, Action<List<PredictionResult>> onSuccess, Action<string> onError)
    {
        try
        {
            // Handle root array vs object
            json = json.Trim();
            if (json.StartsWith("[")) json = "{\"outputs\":" + json + "}";

            var root = JsonUtility.FromJson<RoboflowResponse>(json);

            if (root?.outputs == null || root.outputs.Length == 0)
            {
                onError?.Invoke("No data");
                return;
            }

            List<PredictionResult> results = new List<PredictionResult>();

            foreach (var output in root.outputs)
            {
                if (output.predictions?.predictions != null)
                {
                    foreach (var p in output.predictions.predictions)
                    {
                        results.Add(new PredictionResult
                        {
                            className = p.class_name,
                            confidence = p.confidence
                        });
                    }
                }
            }
            onSuccess?.Invoke(results);
        }
        catch (Exception e)
        {
            onError?.Invoke("Parse Error");
            Debug.LogError(e.Message);
        }
    }

    public struct PredictionResult { public string className; public float confidence; }

    [Serializable] private class WorkflowRequest { public string api_key; public Inputs inputs; }
    [Serializable] private class Inputs { public ImageInput image; }
    [Serializable] private class ImageInput { public string type; public string value; }
    [Serializable] private class RoboflowResponse { public OutputItem[] outputs; }
    [Serializable] private class OutputItem { public PredictionContainer predictions; }
    [Serializable] private class PredictionContainer { public PredictionItem[] predictions; }
    [Serializable] private class PredictionItem { [SerializeField] private string @class; public string class_name => @class; public float confidence; }
}