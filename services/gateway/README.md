\# Gateway Service (P1 - Backend/Data)



\*\*Owner:\*\* \[Eric]



\## Responsibilities



\- OPC UA client for PLC communication

\- Node-RED flows for data routing

\- FastAPI REST endpoints

\- InfluxDB time-series storage

\- Safety checks and rules engine



\## Structure

```

gateway/

├─ flows/           # Node-RED flow definitions

├─ fastapi/         # Python REST API

│  ├─ app.py

│  ├─ opcua\_client.py

│  ├─ influx\_client.py

│  └─ requirements.txt

└─ README.md

```



\## Setup

```bash

\# Install dependencies

pip install -r fastapi/requirements.txt



\# Run server

python fastapi/app.py

```



\## API Endpoints



\- `GET /api/station/status` - Get station status

\- `GET /api/hoppers` - Get hopper data

\- `POST /api/alarms/reset` - Reset alarm

\- `GET /api/history?hours=24` - Historical data



\## Development



\*\*Branch:\*\* `feature/p1-data`



\*\*Merge to:\*\* `develop` (weekly or when stable)



\## TODO



\- \[ ] Set up OPC UA connection to SIF-402 PLC

\- \[ ] Create Node-RED flows

\- \[ ] Implement InfluxDB storage

\- \[ ] Add authentication middleware

\- \[ ] Write API tests

