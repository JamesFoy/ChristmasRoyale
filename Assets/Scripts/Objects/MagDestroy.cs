using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestoryMag());
    }

    IEnumerator DestoryMag()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
