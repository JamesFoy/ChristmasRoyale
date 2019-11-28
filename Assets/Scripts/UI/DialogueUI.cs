using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField]
    GameObject dialogueCanvas;

    InputHandler inputHandler;

    FreeCameraLook camMovement;

    // Start is called before the first frame update
    void Start()
    {
        inputHandler = InputHandler.GetInstance();
        camMovement = FreeCameraLook.GetInstance();
    }

    public void ExitButton()
    {
        Debug.Log("ExitUI");
        camMovement.enabled = true;
        dialogueCanvas.SetActive(false);
        inputHandler.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
