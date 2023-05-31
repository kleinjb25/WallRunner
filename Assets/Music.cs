using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private AudioSource _audioSource;
    private void Awake()
    {
        GameObject[] musicObjs = GameObject.FindGameObjectsWithTag("Music");
        if (musicObjs.Length > 1)
            Destroy(this.gameObject);
        else
        DontDestroyOnLoad(this.gameObject);
    }

}
