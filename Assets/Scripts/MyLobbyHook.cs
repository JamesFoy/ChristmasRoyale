using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkRoomManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        NetworkRoomPlayer lPlayer = lobbyPlayer.GetComponent<NetworkRoomPlayer>();
        Player gPlayer = gameObject.GetComponent<Player>();

        gPlayer.playerName = lPlayer.name;
    }
}
