using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class avoid : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject avoidText;
    void Start()
    {
        StartCoroutine(dont());
    }

    IEnumerator dont()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(avoidText);
    }
}
