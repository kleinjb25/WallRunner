using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class avoid : MonoBehaviour
{
    // objects that need to be destroyed
    public GameObject destroy1;
    public GameObject destroy2;
    void Start()
    {
        StartCoroutine(dont());
    }

    IEnumerator dont()
    {
        yield return new WaitForSeconds(2.25f);
        Destroy(destroy1);
        Destroy(destroy2);
    }
}
