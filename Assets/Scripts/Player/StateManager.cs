using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public bool aiming;
    public bool canRun;
    public bool walk;
    public bool shoot;
    public bool reloading;
    public bool onGround;

    public float horizontal;
    public float vertical;
    public Vector3 lookPosition;
    public Vector3 lookHitPoistion;
    public LayerMask layerMask;

    public CharacterAudioManager audioManager;

    [HideInInspector]
    public HandleShooting handleShooting;
    [HideInInspector]
    public HandleAnimations handleAnim;

    //add the singleton
    public static StateManager instance;

    [HideInInspector]
    public GameObject camHolder;

    private void Awake()
    {
        instance = this;
    }

    public static StateManager GetInstance()
    {
        return instance;
    }

    //Start is called before the first frame update
    void Start()
    {  
        audioManager = GetComponent<CharacterAudioManager>();
        handleShooting = GetComponent<HandleShooting>();
        handleAnim = GetComponent<HandleAnimations>();

        //Instantiate(camHolder, this.gameObject.transform);
    }

    private void FixedUpdate()
    {
        onGround = IsOnGround();
    }

    //Check if you are on the ground
    bool IsOnGround()
    {
        bool retVal = false;

        Vector3 origin = transform.position + new Vector3(0, 0.05f, 0);
        RaycastHit hit;

        //Shoots small raycast down if it hits something that isnt a layermask it returns true
        if (Physics.Raycast(origin, -Vector3.up, out hit, 0.5f, layerMask))
        {
            retVal = true;
        }

        return retVal;
    }
}
