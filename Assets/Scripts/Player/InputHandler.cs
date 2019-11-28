using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float mouse1;
    public float mouse2;
    public float fire3;
    public float middleMouse;
    public float mouseX;
    public float mouseY;

    [HideInInspector]
    public FreeCameraLook camProperties;
    [HideInInspector]
    public Transform camPivot;
    [HideInInspector]
    public Transform camTrans;

    CrosshairManager crosshairManager;
    ShakeCamera shakeCam;
    StateManager states;

    public float normalFov = 60;
    public float aimingFov = 40;
    float targetFov;
    float curFov;

    //Used for camera collisions
    public float cameraNormalZ = -2;
    public float cameraAimingZ = -0.86f;
    float targetZ;
    float actualZ;
    float curZ;
    LayerMask layerMask;

    public float shakeRecoil = 0.5f;
    public float shakeMovement = 0.3f;
    public float shakeMin = 0.1f;
    float targetShake;
    float curShake;

    //add the singleton
    public static InputHandler instance;

    private void Awake()
    {
        instance = this;
    }

    public static InputHandler GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        crosshairManager = CrosshairManager.GetInstance();
        camProperties = FreeCameraLook.GetInstance();
        camPivot = camProperties.transform.GetChild(0);
        camTrans = camPivot.GetChild(0);
        shakeCam = camPivot.GetComponentInChildren<ShakeCamera>();

        states = GetComponent<StateManager>();

        layerMask = ~(1 << gameObject.layer);
        states.layerMask = layerMask;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleInput();
        UpdateStates();
        HandleShake();

        //Find where the camera is looking
        Ray ray = new Ray(camTrans.position, camTrans.forward);
        states.lookPosition = ray.GetPoint(20);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100, layerMask))
        {
            states.lookHitPoistion = hit.point;
        }
        else
        {
            states.lookHitPoistion = states.lookPosition;
        }

        //Check for obstacles in front of the camera
        CameraCollision(layerMask);

        //Update the camera's position
        curZ = Mathf.Lerp(curZ, actualZ, Time.deltaTime * 15);
        camTrans.localPosition = new Vector3(0, 0, curZ);
    }

    void HandleInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        mouse1 = Input.GetAxis("Fire1");
        mouse2 = Input.GetAxis("Fire2");
        middleMouse = Input.GetAxis("Mouse ScrollWheel");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        fire3 = Input.GetAxis("Fire3");
    }

    void UpdateStates()
    {
        states.aiming = states.onGround && (mouse2 > 0);
        states.canRun = !states.aiming;
        states.walk = (fire3 > 0);

        states.horizontal = horizontal;
        states.vertical = vertical;

        if (states.aiming)
        {
            targetZ = cameraAimingZ; //Update target Z position of the camera
            targetFov = aimingFov;

            if (mouse1 > 0.5f && !states.reloading) //We want to shoot
            {
                states.shoot = true;
            }
            else
            {
                states.shoot = false;
            }
        }
        else
        {
            states.shoot = false;
            targetZ = cameraNormalZ; //Update target Z position of the camera
            targetFov = normalFov;
        }
    }

    void HandleShake()
    {
        //Shakes cam if you shoot and if you have bullets to shoot
        if (states.shoot && states.handleShooting.curBullets > 0)
        {
            targetShake = shakeRecoil;
            camProperties.WiggleCrosshairAndCamera(0.06f);
            targetFov += 5;
        }
        else
        {
            //If moving shake the camera slightly
            if (states.vertical != 0)
            {
                targetShake = shakeMovement;
            }
            else
            {
                if (states.horizontal != 0)
                {
                    targetShake = shakeMovement;
                }
                else
                {
                    targetShake = shakeMin; //No movement, no shake
                }
            }
        }

        curShake = Mathf.Lerp(curShake, targetShake, Time.deltaTime * 10);
        //shakeCam.positionShakeSpeed = curShake;

        curFov = Mathf.Lerp(curFov, targetFov, Time.deltaTime * 5);
        //Camera.main.fieldOfView = curFov;
    }

    void CameraCollision(LayerMask layerMask)
    {
        //Do a raycast from the pivot of the camera to the camera
        Vector3 origin = camPivot.TransformPoint(Vector3.zero);
        Vector3 direction = camTrans.TransformPoint(Vector3.zero) - camPivot.TransformPoint(Vector3.zero);
        RaycastHit hit;

        //the distance of the raycast is controlled by if we are aimaing or not
        actualZ = targetZ;

        //if an obstacle is found
        if (Physics.Raycast(origin, direction, out hit, Mathf.Abs(targetZ), layerMask))
        {
            //if we hit something, then find that distance
            float dis = Vector3.Distance(camPivot.position, hit.point);
            actualZ = -dis; //and the oppositie of that is where we want to place our camera
        }
    }
}
