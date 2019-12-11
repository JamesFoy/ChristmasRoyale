using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class PlayerInteraction : NetworkBehaviour
{
    PresentCollect presentCollect;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Present"))
        {
            presentCollect = other.gameObject.GetComponent<PresentCollect>();
            presentCollect.presentState = PresentCollect.PresentState.isCollected;

            player.OnPresentCollected(true);
        }
    }
}
