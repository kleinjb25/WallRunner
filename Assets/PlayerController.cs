using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 6f;
    private Vector3 moveDirection = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            PlayerPrefs.SetInt("once", 0);
            PlayerPrefs.Save();
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                GameObject[] musicObjs = GameObject.FindGameObjectsWithTag("Music");
                Destroy(musicObjs[0]);
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
            
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
            CharacterController controller = GetComponent<CharacterController>();
            moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            controller.Move(moveDirection * Time.deltaTime);
    }
}
