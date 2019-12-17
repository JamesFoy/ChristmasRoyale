using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInteraction : NetworkBehaviour
{
    private float startTime;

    private float defuseTime = 7f;

    PresentCollect presentCollect;
    Player player;
    PlayerCanvas playerCanvas;
    NetworkAnimator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<NetworkAnimator>();
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
            if (Input.GetKey(KeyCode.F))
            {
                //Start planting timer


                //Plant bomb once button is held down long enough
                Debug.Log(presentCollect.presentState);
                presentCollect.SetPresentPosition(player.transform);
                presentCollect.presentState = PresentCollect.PresentState.planted;

                playerCanvas.IsBombInZone(false);

                player.OnPresentCollected(false);
            }
        }

        if (other.gameObject.CompareTag("DefuseCollider") && presentCollect.presentState == PresentCollect.PresentState.planted)
        {
            //Start defuse timer
            if (Input.GetKeyDown(KeyCode.F))
            {
                startTime = Time.time;
            }

            if (Input.GetKey(KeyCode.F))
            {
                anim.animator.SetTrigger("Defusing");

                //When timer finished defuse bomb
                if (startTime + defuseTime <= Time.time)
                {
                    Debug.Log("Bomb Defused");
                    presentCollect.presentState = PresentCollect.PresentState.isCollected;
                    player.OnPresentCollected(true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Tree"))
        {
            playerCanvas.IsBombInZone(false);
        }
    }
}
