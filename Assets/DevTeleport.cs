using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTeleport : MonoBehaviour
{
    public GameObject TeleporterCanvas;
    public Transform[] Teleporters;

    // Start is called before the first frame update
    void Start()
    {
        if (!TeleporterCanvas) 
        {
            Debug.LogError("No canvas selected...");
            Destroy(this);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) 
        {
            bool teleporterIsActive = TeleporterCanvas.activeSelf;

            // Teleporter Screen is already active
            if (TeleporterCanvas.activeSelf)
            {
                TeleporterCanvas.SetActive(false);
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            // Teleporter Screen is inactive
            else
            {
                TeleporterCanvas.SetActive(true);
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    // Button Functions
    public void Teleport(int num) 
    {
        if (num < Teleporters.Length) 
        {
            this.transform.position = Teleporters[num].position;
        }
    }
}
