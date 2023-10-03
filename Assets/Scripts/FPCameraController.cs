using UnityEngine;

public class FPCameraController : MonoBehaviour
{
    public float senseX;
    public float senseY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Application.targetFrameRate = 60;
        // QualitySettings.vSyncCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Should not rotate because world is paused
        if (Time.timeScale == 0)
            return;

        float mouseX = Input.GetAxisRaw("Mouse X") * senseX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * senseY;

        xRotation = Mathf.Clamp(xRotation - mouseY, -90f, 90f);
        yRotation += mouseX;

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
