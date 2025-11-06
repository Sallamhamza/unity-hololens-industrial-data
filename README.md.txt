SIF-402 HoloLens Control System

Mixed Reality application for monitoring and controlling the SIF-402 container filling station using Microsoft HoloLens 2.

##  Project Overview

**Course:** [Industrial Project With Extend Reality]  
**Team:** Group 2  
**Date:** November 2025  
**Unity Version:** 2022.3.62f2  
**Target Platform:** Microsoft HoloLens 2  

##  Features

### Current Implementation (Testing Stage)
-  **Startup Menu** - Main interface with START, SETTINGS, EXIT buttons
-  **Main Control Panel** - Real-time station monitoring
-  **Data Integration** - Mock data loading from Mockaroo JSON
-  **Station Status Display** - Live status updates every 3 seconds
-  **Navigation System** - Seamless transition between menus
-  **MRTK Integration** - Hand tracking and interaction support
-  **Spatial Anchoring** - Panel positioning system (PlayerPrefs-based)

### Planned Features
-  **Hopper Status Indicators** - Visual display of 3 hoppers (Blue, Yellow, Red)
-  **Alarm Panel** - Real-time alert notifications
-  **QR Code Anchoring** - Automatic positioning via QR codes
-  **Animation Guidance** - Step-by-step operator instructions

##  Architecture
```
SIF402_MainScene
├─ MixedRealityToolkit (MRTK)
├─ MixedRealityPlayspace (Camera & Hand Tracking)
├─ DataManager (Mock Data System)
├─ StartupMenu (Main UI)
└─ MainControlPanel (Station Monitoring)
```

##  Project Structure
```
Assets/
├─ Scenes/
│  └─ SIF402_MainScene.unity
├─ Scripts/
│  ├─ MockDataLoader.cs
│  ├─ MockDataManager.cs
│  ├─ StartupManager.cs
│  ├─ MainControlUI.cs
│  ├─ StationAnchor.cs
│  └─ StationInfoDisplay.cs
├─ Data/
│  └─ station_data_small.json (5-row test data)
└─ QRCodeTracking/ (QR code detection scripts)
```

##  Getting Started

### Prerequisites
- Unity 2022.3.62f2 (recommended)
- Mixed Reality Toolkit (MRTK) 2.8.x
- Microsoft HoloLens 2 device
- Visual Studio 2022 (for deployment)


### Building for HoloLens

1. **File → Build Settings**
2. **Switch Platform:** Universal Windows Platform
3. **Target Device:** HoloLens
4. **Architecture:** ARM64
5. **Click Build**
6. **Open in Visual Studio**
7. **Deploy to Device**

##  How to Use

### Testing in Unity Editor
1. Open `SIF402_MainScene`
2. Press Play
3. Use mouse to click buttons
4. Check Console for data updates

### On HoloLens Device
1. Launch "SIF402 Control" app
2. Use hand rays to interact with UI
3. Click START to view station monitoring
4. Data updates automatically every 3 seconds

##  Data System

### Mock Data Source
- **Provider:** Mockaroo.com
- **Rows:** 5 (testing) / 100 (production)
- **Update Interval:** 3 seconds
- **Fields:** Station status, recipe, hopper states, energy consumption

### Data Flow
```
JSON File → MockDataLoader → MockDataManager → UI Components
```

##  Configuration

### Key Settings
- **Canvas Scale:** 0.001 (World Space)
- **Panel Position:** (0, 1.5, 2) - 1.5m high, 2m away
- **Update Interval:** 3 seconds
- **Anchor System:** PlayerPrefs (local storage)

##  Known Issues

- Holographic Remoting may disconnect after long sessions (solution: restart Remoting Player)
- Stats overlay appears in editor (doesn't affect HoloLens build)
- QR code detection requires NuGet packages (optional feature)

##  Sprint Status

### Sprint 2 - Completed 
- [x] MRTK setup and configuration
- [x] Basic UI panels (Startup, Main Control)
- [x] Mock data integration
- [x] Navigation system
- [x] Spatial anchoring (PlayerPrefs)

### Sprint 3 - In Progress 
- [ ] Hopper status visualization
- [ ] Alarm panel implementation
- [ ] QR code anchoring (optional)
- [ ] Real-time PLC data integration

### Sprint 4 - Planned 
- [ ] Animation guidance system
- [ ] Voice commands
- [ ] Final testing and optimization



##  Acknowledgments

- Microsoft Mixed Reality Toolkit (MRTK)
- Unity Technologies
- Mockaroo for test data generation

##  Contact

sallamhamza77@gmail.com