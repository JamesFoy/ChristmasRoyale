using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;

    private Animator anim;

    private PlayerMotor motor;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        //Calculate movement velocity as a 3D vector
        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        //Final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

        //Animate movement
        anim.SetFloat("ForwardVelocity", _zMov, 1f, Time.deltaTime * 10f);
        anim.SetFloat("Sideways", _xMov, 1f, Time.deltaTime * 10f);

        //Apply movement
        motor.Move(_velocity);

        //Calculate rotation as a 3D vector (turning around)
        float _yRot = Input.GetAxis("Mouse X");
        Debug.Log("look y rotates " + _yRot);


        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        lookSensitivity = 5;
        Debug.Log("look sensitivity rotates " + lookSensitivity);
        Debug.Log("controller rotates " + _rotation);

        //Apply rotation
        motor.Rotate(_rotation);

        //Calculate camera rotation as a 3D vector (looking Up/Down)
        float _xRot = Input.GetAxis("Mouse Y");

        float _cameraRotationX = _xRot * lookSensitivity;



        //Apply camera rotation
        motor.RotateCamera(_cameraRotationX);
    }
}
