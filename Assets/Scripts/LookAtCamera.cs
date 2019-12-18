using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (mainCamera == null)
            return;

        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.position);
    }
}
