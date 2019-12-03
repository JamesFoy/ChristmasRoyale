using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;

    [SerializeField]
    private float cameraRotationLimit = 85f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //Gets a momement vector
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    //Gets a rotational vector
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    //Gets a rotational vector for the camera
    public void RotateCamera(float _cameraRotationX)
    {
        cameraRotationX = _cameraRotationX;
    }

    //Run every physics iteration
    private void FixedUpdate()
    {
        PerfromMovement();
        PerformRotation();
    }

    //Perform movement based on velocity variable
    private void PerfromMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
        }
    }

    //Perfrom rotation
    void PerformRotation()
    {
        Debug.Log("Motor rotates " + rotation);
        rb.MoveRotation(transform.rotation * Quaternion.Euler(rotation));
        if (cam != null)
        {
            //Set rotation
            currentCameraRotationX -= cameraRotationX;
            //Clamp rotation
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -80, 45);

            //Apply rotation to the transform of the camera
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);
        }
    }
}
