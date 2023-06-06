using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 12f;
    private Vector3 moveDirection = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Return))
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
        if (Input.GetKey(KeyCode.M))
        {
            PlayerPrefs.SetInt("once", 0);
            PlayerPrefs.Save();
            GameObject[] musicObjs = GameObject.FindGameObjectsWithTag("Music");
            Destroy(musicObjs[0]);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("MainMenu");
        }
            CharacterController controller = GetComponent<CharacterController>();
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            controller.Move(moveDirection * Time.deltaTime);
    }
}
