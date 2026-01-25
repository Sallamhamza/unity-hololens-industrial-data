# XR Line Assistant - Industrial AR Monitoring System (SIF-402/405)

**A Mixed Reality interface for real-time industrial automation monitoring and AI-assisted quality control on Microsoft HoloLens 2.**

---

## Project Overview

This project bridges the gap between **Operational Technology (OT)** and **Augmented Reality (AR)**. It connects to Siemens B&R PLCs controlling SMC SIF-402 and SIF-405 stations via OPC UA, processes data through Node-RED, and visualizes it in 3D using Unity and MRTK. Additionally, it integrates a Computer Vision loop for real-time inventory classification.

Demo of the final submission: https://www.youtube.com/watch?v=KruqfdqWRAk

---

## System Architecture

The system operates on a 4-layer architecture:

1.  **Industrial Layer:**
    * **Machines:** SMC SIF-402 (Hopper Station) & SIF-405 (Feeder Station).
    * **PLC:** Siemens B&R Automation Runtime.
    * **Protocol:** OPC UA (Binary stream).

2.  **Middleware Layer (The Bridge):**
    * **Node-RED:** Subscribes to OPC UA tags and exposes them as REST API endpoints.
    * **Endpoints:*
        * `GET /mes/402_latest`: Status, Current, Alarms.
        * `GET /mes/405_latest`: Feeder Station Status.
        * `GET /feeders`: Cap counts for Round/Square feeders.

3.  **Intelligence Layer (AI):**
    * **Docker:** Runs Roboflow Inference Server (Port 9001).
    * **Python API:** FastAPI middleware (Port 8000) handling image upload and formatting.
    * **Model:** Object detection for "Round Cap" vs "Square Cap".
    * Model can be tested over https://app.roboflow.com/workflows/mobile/eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ3b3JrZmxvd0lkIjoiTjVXNFlCS2RJMXgwVDB3MW9YaG4iLCJ3b3Jrc3BhY2VJZCI6IlFQcDgzd2k3UE5jQXJXYndjNml4MlpnMGdUMjMiLCJ1c2VySWQiOiJRUHA4M3dpN1BOY0FyV2J3YzZpeDJaZzBnVDIzIiwiaWF0IjoxNzY4Mzk4OTcwfQ.TDClNMEHPAKDJJ6VsRP_sn4R7FRno1TVtKq65mPo-hU

4.  **Presentation Layer (HoloLens 2):**
    * **Engine:** Unity 2022.3 LTS.
    * **Toolkit:** MRTK 2.8.3 + OpenXR.
    * **Features:** Hand tracking, Spatial UI, Async Data Polling.

---

##  Key Features

###  Real-Time Monitoring
* **Station Status:** Displays "Running", "Idle", or "Alarm" with color-coded panels.
* **Live Metrics:** Shows electrical current (Amps) and hopper levels.
* **3D Digital Twins:** Virtual hoppers and feeders animate based on real PLC data.

###  AI Computer Vision
* **Camera Capture:** Uses HoloLens PV camera to capture inventory.
* **Manual Override:** Drag-and-drop image support in Editor for testing without a headset.
* **Classification:** Detects container types (Round vs Square) and displays confidence scores.

### Spatial Interaction
* **Hand Tracking:** Full support for Air-Tap, Pinch, and Poke gestures.
* **Movable UI:** Information panels (`Visualizer_Round/Square`) are equipped with `ObjectManipulator` to allow users to organize their workspace.
* **Static Anchors:** Heavy machinery holograms (`SIF402`) remain fixed for precise alignment with the physical world.

---

##  Installation & Setup

### Prerequisites
* **Unity Hub** + Unity **2022.3.x LTS** (with UWP Build Support).
* **Visual Studio 2022** (Workloads: Desktop C++, UWP, Game Development with Unity).
* **Mixed Reality Feature Tool** (for MRTK and OpenXR Plugin).
* **Python 3.10** (Anaconda recommended for AI layer).
* **Docker Desktop** (for Inference Server).

### 1. Unity Setup
1.  Clone this repository.
2.  Open the project in Unity 2022.3.
3.  Go to `Window > TextMeshPro > Import TMP Essentials`.
4.  **Project Settings Check:**
    * *Player > Publishing Settings:* Enable **InternetClient**, **InternetClientServer**, **PrivateNetworkClientServer**, **WebCam**.
    * *XR Plug-in Management:* Ensure **Microsoft HoloLens** feature group is checked.

### 2. Backend Setup (Simulation)
If you do not have access to the physical PLC, ensure the Node-RED mock server is running:
1.  Import `flows.json` into Node-RED.
2.  Verify endpoints are accessible at `http://localhost:1880/mes/402_latest`.

### 3. AI Setup
1.  **Start Docker:**
    ```powershell
    docker run -p 9001:9001 roboflow/roboflow-inference-server-cpu
    ```
2.  **Start Python API:**
    ```powershell
    conda activate xr_env
    python -m uvicorn api_server:app --host 0.0.0.0 --port 8000
    ```

---

##  Configuration

### Changing Data Sources
To switch between Localhost (Simulator) and Physical PLC (Lab Network), edit **ApiDataManager** in the Inspector:

* **GameObject:** `DataManager`
* **Script:** `ApiDataManager.cs`
* **Variables:**
    * `Machine 402 Url`: `http://[PLC_IP]:1880/mes/402_latest`
    * `Machine 405 Url`: `http://[PLC_IP]:1880/mes/405_latest`

### AI Configuration
To point to a different inference server, edit **HoloLensCameraCapture**:

* **GameObject:** `MainCamera` or `UI_Manager`
* **Script:** `HoloLensCameraCapture.cs`
* **Variables:**
    * `Server Url`: `http://[PC_IP]:8000/predict`

---

##  Troubleshooting / FAQ



**Q: AI returns "Analysis Failed" or "Network Error".**
* **Fix:**
    1.  Ensure PC firewall allows connections on Port 8000.
    2.  Ensure Unity Player settings have **"Allow downloads over HTTP"** set to "Always Allowed".
    3.  Verify HoloLens and PC are on the same Wi-Fi.

**Q: 3D Objects appear gray or invisible.**
* **Fix:** Check the imported FBX model. Remove any nested "Camera" objects inside the prefab. Ensure `Scale Factor` is set to 1.0 (or 100 if exported from Blender).

---

##  Team Structure

* **P1 (Backend/Data):** Eric - Gateway Services
* **P2 (ML Classifier):** Leon - Model Training
* **P3/P4 (Unity XR, HoloLens 2):** Hamza - Frontend & Integration

---

*Last Updated: Jan 2026*
