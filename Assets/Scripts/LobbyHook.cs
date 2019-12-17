using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class LobbyHook : MonoBehaviour
{
    public virtual void OnLobbyServerSceneLoadedForPlayer(NetworkRoomManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {

    }
}
