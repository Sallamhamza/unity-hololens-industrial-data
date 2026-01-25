from fastapi import FastAPI, UploadFile, File, HTTPException
from fastapi.responses import JSONResponse
import httpx
import base64

app = FastAPI(title="XR Inference API")

API_KEY = "M9oW1Va2Kqrj5Ybt2DJa"
INFERENCE_URL = "http://130.130.130.203:9001"

@app.get("/health")
def health():
    return {"status": "ok"}

@app.post("/predict")
async def predict(image: UploadFile = File(...)):
    if not image.content_type.startswith("image/"):
        raise HTTPException(status_code=400, detail="File must be an image.")
    
    try:
        image_bytes = await image.read()
        image_base64 = base64.b64encode(image_bytes).decode("utf-8")
        
        async with httpx.AsyncClient(timeout=60.0) as client:
            response = await client.post(
                f"{INFERENCE_URL}/infer/workflows/xrproject/custom-workflow",
                headers={"Content-Type": "application/json"},
                json={
                    "api_key": API_KEY,
                    "inputs": {
                        "image": {"type": "base64", "value": image_base64}
                    }
                }
            )
            response.raise_for_status()
            result = response.json()
            
    except httpx.HTTPStatusError as e:
        raise HTTPException(status_code=e.response.status_code, detail=str(e))
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))
    
    return JSONResponse(content=result)