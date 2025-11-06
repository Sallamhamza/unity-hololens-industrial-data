\# Unity XR Application (P3/P4)



\*\*Owner:\*\* \[Hamza]



\## Responsibilities



\### P3 (Core Unity)

\- Networking and API client

\- Spatial anchors

\- Scene logic

\- Data polling

\- Device builds



\### P4 (UI/UX)

\- Prefabs and visuals

\- Animations

\- Voice commands

\- UI polish

\- Accessibility



\## Structure

```

XRLineAssistant/

├─ Assets/

│  ├─ Scripts/

│  │  ├─ Net/          # API client, networking

│  │  ├─ UI/           # UI components

│  │  └─ XR/           # Spatial anchors, voice

│  ├─ Prefabs/         # Reusable components

│  ├─ Scenes/

│  │  └─ SIF402\_MainScene.unity

│  └─ Resources/

│     └─ Config.json   # API endpoints config

├─ Packages/

└─ ProjectSettings/

```



\## Current Features



\- Startup menu with navigation

\- Main control panel

\- Mock data integration

\- Station status display

\- Basic spatial anchoring

\- Hopper animations (in progress)

\- Container guidance (planned)



\## Development



\*\*Branch:\*\* `feature/p3-ui` (core logic)

\*\*Branch:\*\* `feature/p4-ui` (visuals)



\*\*Merge to:\*\* `develop` (every 1-2 weeks)



\## Building for HoloLens



1\. File → Build Settings

2\. Platform: Universal Windows Platform

3\. Target Device: HoloLens

4\. Architecture: ARM64

5\. Build and deploy via Visual Studio



\## TODO



\- \[ ] Connect to real backend API (when P1 ready)

\- \[ ] Add AR animations for hoppers

\- \[ ] Container placement guidance

\- \[ ] Error/warning visual effects

\- \[ ] Voice command integration

\- \[ ] QR code anchoring (optional)

