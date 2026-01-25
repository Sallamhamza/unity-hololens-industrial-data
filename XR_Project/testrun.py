# 1. Import the library
from inference_sdk import InferenceHTTPClient

# 2. Connect to your local server
client = InferenceHTTPClient(
    api_url="http://172.20.10.11:9001", # Local server address
    api_key="M9oW1Va2Kqrj5Ybt2DJa"
)

#image_path = r"D:/Group2_Project2/Image1.jpg"
#with open(image_path, "rb") as image_file:
#    image_data = image_file.read()

# 3. Run your workflow on an image
result = client.run_workflow(
    workspace_name="xrproject",
    workflow_id="custom-workflow",
    images={
        "image": "D:/Group2_Project2/Image1.png" # Path to your image file
    },

  #  images={
  #      "image": image_data
 #   },
    use_cache=True # Speeds up repeated requests
)

# 4. Get your results
print(result)
