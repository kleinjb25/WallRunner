using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    float camerVerticalRotation = 0f;
    private float error = 0.001f;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false; // hide the cursor
        Cursor.lockState = CursorLockMode.Locked; // lock the cursor to the center of the screen

    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Mouse X"); // get the mouse X axis
        float inputY = Input.GetAxis("Mouse Y"); // get the mouse Y axis
        camerVerticalRotation -= inputY; // subtract the Y axis input from the camera rotation
        camerVerticalRotation = Mathf.Clamp(camerVerticalRotation, 0f, 0f); // clamp the camera rotation to prevent the player from looking too far up or down
        transform.localEulerAngles = Vector3.right * camerVerticalRotation; // rotate the camera on the X axis
        player.Rotate(Vector3.up * inputX); // rotate the player on the Y axis

        //draw a raycast from the camera and get the color of the object it hits
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Color c = hit.collider.gameObject.GetComponent<Renderer>().material.color;
            if (!(c.r - .292 <= error && c.g - .292 <= error && c.b - .292 <= error))
            {
                Debug.Log(i + " " + hit.collider.gameObject.GetComponent<Renderer>().material.color);
                i++;
            }
                
        }
    }
}
