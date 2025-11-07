# XR Line Assistant - SIF-402 HoloLens Control System

Mixed Reality application for industrial station monitoring and control.

## Team Structure

- **P1 (Backend/Data)**: [Eric] - `services/gateway/`
- **P2 (ML Classifier)**: [Leon] - `services/classifier/`
- **P3/P4 (Unity XR)**: [Hamza] - `unity/XRLineAssistant/`

## Quick Start

### Unity XR App (HoloLens)
```bash
cd unity/XRLineAssistant
# Open in Unity 2022.3.62f2
```

### Backend Gateway (P1)
```bash
cd services/gateway
# See services/gateway/README.md
```

### Classifier Service (P2)
```bash
cd services/classifier
# See services/classifier/README.md
```

## Project Structure
```
xr-line-assistant/
├─ docs/              # Documentation
├─ deploy/            # Deployment configs
├─ services/
│  ├─ gateway/        # P1: OPC UA, Node-RED, FastAPI
│  └─ classifier/     # P2: YOLO model, inference
├─ unity/             # P3/P4: HoloLens app
├─ tools/             # Utilities
└─ tests/             # E2E tests
```

## Branching Strategy

- `main` - Stable releases only
- `develop` - Integration branch
- `feature/p1-data` - Backend work
- `feature/p2-classifier` - ML model work
- `feature/p3-ui` - Unity core
- `feature/p4-ui` - Unity visuals 

## Sprint Workflow

1. Work on your feature branch
2. Merge to `develop` weekly
3. Test integration
4. Merge `develop` → `main` at sprint end

## Current Status

**Sprint [2] - [09/11/2026]**

- [x] Unity basic UI - DONE
- [x] Mock data integration - DONE
- [x] Backend OPC UA - IN PROGRESS (P1)
- [x] Classifier training - IN PROGRESS (P2)
- [x] AR animations - IN PROGRESS (P3/P4)

## Contact

- Project Lead: [Eric]
- Repository: https://github.com/Sallamhamza/unity-hololens-industrial-data
