using UnityEngine;

public class StationAnchor : MonoBehaviour
{
    [Header("Anchor Settings")]
    public string anchorName = "SIF402_StationInfo_Anchor";
    public Vector3 savedPosition = new Vector3(0, 0.3f, 0);
    public Quaternion savedRotation = Quaternion.identity;

    [Header("Status")]
    public bool usesSavedPosition = false;

    void Start()
    {
        LoadPosition();
    }

    public void SaveCurrentPosition()
    {
        savedPosition = transform.position;
        savedRotation = transform.rotation;
        usesSavedPosition = true;

        PlayerPrefs.SetFloat(anchorName + "_PosX", savedPosition.x);
        PlayerPrefs.SetFloat(anchorName + "_PosY", savedPosition.y);
        PlayerPrefs.SetFloat(anchorName + "_PosZ", savedPosition.z);

        PlayerPrefs.SetFloat(anchorName + "_RotX", savedRotation.x);
        PlayerPrefs.SetFloat(anchorName + "_RotY", savedRotation.y);
        PlayerPrefs.SetFloat(anchorName + "_RotZ", savedRotation.z);
        PlayerPrefs.SetFloat(anchorName + "_RotW", savedRotation.w);

        PlayerPrefs.SetInt(anchorName + "_Saved", 1);
        PlayerPrefs.Save();

        UnityEngine.Debug.Log("✅ Position saved: " + savedPosition);
    }

    void LoadPosition()
    {
        if (PlayerPrefs.GetInt(anchorName + "_Saved", 0) == 1)
        {
            savedPosition = new Vector3(
                PlayerPrefs.GetFloat(anchorName + "_PosX"),
                PlayerPrefs.GetFloat(anchorName + "_PosY"),
                PlayerPrefs.GetFloat(anchorName + "_PosZ")
            );

            savedRotation = new Quaternion(
                PlayerPrefs.GetFloat(anchorName + "_RotX"),
                PlayerPrefs.GetFloat(anchorName + "_RotY"),
                PlayerPrefs.GetFloat(anchorName + "_RotZ"),
                PlayerPrefs.GetFloat(anchorName + "_RotW")
            );

            transform.position = savedPosition;
            transform.rotation = savedRotation;
            usesSavedPosition = true;

            UnityEngine.Debug.Log("✅ Loaded saved position");
        }
        else
        {
            transform.position = savedPosition;
            transform.rotation = savedRotation;
        }
    }

    public void ResetToDefault()
    {
        savedPosition = new Vector3(0, 0.3f, 0);
        savedRotation = Quaternion.identity;
        transform.position = savedPosition;
        transform.rotation = savedRotation;
        usesSavedPosition = false;

        PlayerPrefs.DeleteKey(anchorName + "_PosX");
        PlayerPrefs.DeleteKey(anchorName + "_PosY");
        PlayerPrefs.DeleteKey(anchorName + "_PosZ");
        PlayerPrefs.DeleteKey(anchorName + "_RotX");
        PlayerPrefs.DeleteKey(anchorName + "_RotY");
        PlayerPrefs.DeleteKey(anchorName + "_RotZ");
        PlayerPrefs.DeleteKey(anchorName + "_RotW");
        PlayerPrefs.DeleteKey(anchorName + "_Saved");
        PlayerPrefs.Save();

        UnityEngine.Debug.Log("🔄 Position reset");
    }
}