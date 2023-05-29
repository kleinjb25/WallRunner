using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{
    public void LoadControls()
    {
        SceneManager.LoadScene("Controls");
    }
}
