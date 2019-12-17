using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LobbyCanvas : MonoBehaviour
{
    [SerializeField] NetworkRoomManager networkRoomManager;
    [SerializeField] GameObject networkSelectionUI;

    public void HostGame()
    {
        networkRoomManager.StartHost();
        networkSelectionUI.SetActive(false);
        networkRoomManager.showRoomGUI = true;
    }

    public void JoinGame()
    {
        networkRoomManager.StartClient();
        networkSelectionUI.SetActive(false);
        networkRoomManager.showRoomGUI = true;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
