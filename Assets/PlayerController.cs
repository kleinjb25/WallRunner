using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 6f;
    private Vector3 moveDirection = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        if (CameraController.is2D)
        {
            moveDirection = Vector3.zero;
        }
        if (!CameraController.is2D)
        {
            CharacterController controller = GetComponent<CharacterController>();
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            controller.Move(moveDirection * Time.deltaTime);
        }
        
    }
}
