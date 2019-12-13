using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInteraction : NetworkBehaviour
{
    PresentCollect presentCollect;
    Player player;
    PlayerCanvas playerCanvas;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        presentCollect = PresentCollect.Instance;
        playerCanvas = PlayerCanvas.Instance;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Present"))
        {
            if (presentCollect.presentState != PresentCollect.PresentState.planted)
            {
                presentCollect.presentState = PresentCollect.PresentState.isCollected;

                player.OnPresentCollected(true);
            }
        }
        
        if (other.gameObject.CompareTag("Tree") && player.hasPresent)
        {
            //Display UI Info
            playerCanvas.IsBombInZone(true);

            //Press Interation key to plant present
            if (Input.GetKeyDown(KeyCode.F))
            {
                //Plant bomb
                Debug.Log(presentCollect.presentState);
                presentCollect.SetPresentPosition(player.transform);
                presentCollect.presentState = PresentCollect.PresentState.planted;

                player.OnPresentCollected(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Tree") && player.hasPresent)
        {
            playerCanvas.IsBombInZone(false);
        }
    }
}
