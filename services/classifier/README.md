\# Classifier Service (P2 - ML Model)



\*\*Owner:\*\* \[Leon]



\## Responsibilities



\- YOLO model training for defect detection

\- ONNX model export

\- Inference REST server

\- Performance optimization

\- Model evaluation and metrics



\## Structure

```

classifier/

├─ training/

│  ├─ dataset/      # Training images

│  ├─ labels/       # YOLO annotations

│  ├─ train.py      # Training script

│  └─ export\_onnx.py

├─ inference/

│  ├─ server.py     # REST inference server

│  ├─ onnx/model.onnx

│  └─ requirements.txt

└─ evaluation/

&nbsp;  ├─ latency\_benchmark.ipynb

&nbsp;  └─ confusion\_matrix.png

```



\## Setup

```bash

\# Install dependencies

pip install -r inference/requirements.txt



\# Run inference server

python inference/server.py

```



\## API Endpoints



\- `POST /classify` - Upload image, get classification

\- `GET /health` - Server health check



\## Development



\*\*Branch:\*\* `feature/p2-classifier`



\*\*Merge to:\*\* `develop` (weekly or after retrain)



\## TODO



\- \[x] Collect training dataset (100+ images)

\- \[x] Annotate with YOLO labels

\- \[x] Train initial model

\- \[ ] Export to ONNX format

\- \[ ] Create inference server

\- \[ ] Benchmark latency (<100ms target)

