//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class InteractRaycast : MonoBehaviour
//{
//    Camera cam;
//    IOutLineShaderActivator obj;

//    InputHandler inputHandler;

//    StateManager stateManager;

//    FreeCameraLook camMovement;

//    DialogueTrigger triggerDialogue;

//    void Start()
//    {
//        inputHandler = InputHandler.GetInstance();
//        stateManager = StateManager.GetInstance();
//        camMovement = GetComponentInParent<FreeCameraLook>();
//        cam = GetComponent<Camera>();
//    }

//    void Update()
//    {
//        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
//        RaycastHit hit;
//        Debug.DrawRay(ray.origin, Camera.main.transform.forward * 3, Color.red);
//        if (Physics.Raycast(ray, out hit, 3) && hit.collider.gameObject.CompareTag("NPC"))
//        {
//            obj = hit.collider.gameObject.GetComponent<IOutLineShaderActivator>();
//            obj.OutlineToggle();
//            if (Input.GetKeyDown(KeyCode.F))
//            {
//                if (hit.collider.CompareTag("NPC"))
//                {
//                    triggerDialogue = hit.collider.gameObject.GetComponentInChildren<DialogueTrigger>();
//                    triggerDialogue.TriggerDialogue();
//                    stateManager.vertical = 0;
//                    stateManager.horizontal = 0;
//                    camMovement.enabled = false;
//                    inputHandler.enabled = false;
//                    Cursor.lockState = CursorLockMode.None;
//                    Cursor.lockState = CursorLockMode.Locked;
//                    Cursor.lockState = CursorLockMode.None;
//                }
//            }
//        }
//        else if (obj != null)
//        {
//            obj.OutlineUnToggle();
//        }
//    }
//}