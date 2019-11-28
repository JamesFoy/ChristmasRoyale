using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseLimit : MonoBehaviour
{
    public GameObject player;

    StateManager states;

    private void Start()
    {
        states = player.GetComponent<StateManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            states.walk = true;
        }
    }
}
