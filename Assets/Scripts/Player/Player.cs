using UnityEngine.Events;
using Mirror;
using UnityEngine;
using Unity.Collections;
using System.Collections;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool> { }

public class Player : NetworkBehaviour
{
    [SyncVar (hook = "OnPresentCollected")] public bool hasPresent;
    [SyncVar (hook = "OnNameChanged")] public string playerName;

    [SerializeField] ToggleEvent onToggleShared;
    [SerializeField] ToggleEvent onToggleLocal;
    [SerializeField] ToggleEvent onToggleRemote;
    [SerializeField] UnityEvent onDeath;
    [SerializeField] float respawnTime = 5f;

    static List<Player> players = new List<Player>();

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

    [ServerCallback]
    private void OnEnable()
    {
        if (!players.Contains(this))
        {
            players.Add(this);
        }
    }

    [ServerCallback]
    private void OnDisable()
    {
        if (players.Contains(this))
        {
            players.Remove(this);
        }
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
            PlayerCanvas.Instance.HideReticule();
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
            PlayerCanvas.Instance.Initialize();
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
            onDeath.Invoke();

            PlayerCanvas.Instance.WriteGameStatusText ("You Died!");
            PlayerCanvas.Instance.PlayDeathAudio();

            anim.SetTrigger("Die");

        }

        //Drop Present if the player hasPresent was true;
        if (hasPresent)
        {
            presentCollect.SetPresentPosition(transform);
            presentCollect.presentState = PresentCollect.PresentState.hasDropped;
            OnPresentCollected(false);
        }

        DisablePlayer();

        StartCoroutine(Respawn(respawnTime));
    }

    IEnumerator Respawn(float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);

        if (isLocalPlayer)
        {
            Transform spawn = NetworkManager.singleton.GetStartPosition();
            transform.position = spawn.position;
            transform.rotation = spawn.rotation;

            anim.SetTrigger("Restart");
        }

        yield return null;

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

    void OnNameChanged(string value)
    {
        playerName = value;
        gameObject.name = playerName;
        //set text
        GetComponentInChildren<TMP_Text>(true).text = playerName;
    }

    [Server]
    public void Won()
    {
        //tell other players
        for (int i = 0; i < players.Count; i++)
        {
            players[i].RpcGameOver(netIdentity, name);
        }

        Invoke("BackToLobby", 5f);
    }

    [ClientRpc]
    void RpcGameOver(NetworkIdentity networkID, string name)
    {
        DisablePlayer();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (isLocalPlayer)
        {
            if (netIdentity == networkID)
            {
                PlayerCanvas.Instance.WriteGameStatusText("You Won!");
            }
            else
            {
                PlayerCanvas.Instance.WriteGameStatusText("Game Over!\n" + name + " Won");
            }
        }
    }

    void BackToLobby()
    {
        FindObjectOfType<NetworkRoomManager>().ServerChangeScene("Lobby");
    }
}
