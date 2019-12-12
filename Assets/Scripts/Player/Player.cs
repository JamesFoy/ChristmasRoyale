using UnityEngine.Events;
using Mirror;
using UnityEngine;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool> { }

public class Player : NetworkBehaviour
{
    [SyncVar (hook = "OnPresentCollected")] public bool hasPresent;

    [SerializeField] ToggleEvent onToggleShared;
    [SerializeField] ToggleEvent onToggleLocal;
    [SerializeField] ToggleEvent onToggleRemote;
    [SerializeField] float respawnTime = 5f;

    PresentCollect presentCollect;
    GameObject mainCamera;
    NetworkAnimator anim;

    private void Start()
    {
        anim = GetComponent<NetworkAnimator>();
        mainCamera = Camera.main.gameObject;
        presentCollect = PresentCollect.Instance;

        EnablePlayer();
    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        anim.animator.SetFloat("Speed", Input.GetAxis("Vertical"));
        anim.animator.SetFloat("Strafe", Input.GetAxis("Horizontal"));
    }

    void DisablePlayer()
    {
        onToggleShared.Invoke(false);

        if (isLocalPlayer)
        {
            mainCamera.SetActive(true);
            //PlayerCanvas.canvas.HideReticule();
            onToggleLocal.Invoke(false);
        }
        else
        {
            onToggleRemote.Invoke(false);
        }
    }

    void EnablePlayer()
    {
        onToggleShared.Invoke(true);

        if (isLocalPlayer)
        {
            mainCamera.SetActive(false);
            //PlayerCanvas.canvas.Initialize();
            onToggleLocal.Invoke(true);
        }
        else
        {
            onToggleRemote.Invoke(true);
        }
    }

    public void Die()
    {
        if (isLocalPlayer)
        {
            //PlayerCanvas.canvas.WriteGameStatusText ("You Died!");
            //PlayerCanvas.canvas.PlayDeathAudio();

            anim.SetTrigger("Died");
        }

        //Drop Present if the player hasPresent was true;
        if (hasPresent)
        {
            presentCollect.SetPresentPosition(transform);
            presentCollect.presentState = PresentCollect.PresentState.hasDropped;
            OnPresentCollected(false);
        }

        DisablePlayer();

        Invoke("Respawn", respawnTime);
    }

    void Respawn()
    {
        if (isLocalPlayer)
        {
            Transform spawn = NetworkManager.singleton.GetStartPosition();
            transform.position = spawn.position;
            transform.rotation = spawn.rotation;

            anim.SetTrigger("Restart");
        }

        EnablePlayer();
    }

    public void OnPresentCollected(bool value)
    {
        hasPresent = value;
        if (isLocalPlayer)
        {
            //Display UI icon
        }
    }
}
