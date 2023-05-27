using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private bool isGrounded = false;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Horizontal movement with Q and E keys
        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.Q))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            horizontalInput = 1f;
        }
        Vector3 movement = new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0f, 0f);
        transform.Translate(movement);

        // Jumping with Space key
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the image is touching the ground or platform
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if the image is no longer touching the ground or platform
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

}
