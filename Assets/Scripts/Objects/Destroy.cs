using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("WaitSeconds");
    }

    public IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    } 
}
