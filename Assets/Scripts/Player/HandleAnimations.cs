using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleAnimations : MonoBehaviour
{
    Animator anim;

    [SerializeField]
    Transform magSpawnPoint;

    [SerializeField]
    GameObject magSpawnObject;

    [SerializeField]
    MeshRenderer currentMag;

    CharacterAudioManager audio;

    StateManager states;
    Vector3 lookDirection;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<CharacterAudioManager>();
        states = GetComponent<StateManager>();
        SetupAnimator();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        states.reloading = anim.GetBool("Reloading");
        anim.SetBool("Aim", states.aiming);

        if (!states.canRun)
        {
            anim.SetFloat("Forward", states.vertical, 0.1f, Time.deltaTime);
            anim.SetFloat("Sideways", states.horizontal, 0.1f, Time.deltaTime);
        }
        else
        {
            float movement = Mathf.Abs(states.vertical) + Mathf.Abs(states.horizontal);

            bool walk = states.walk;

            movement = Mathf.Clamp(movement, 0, (walk || states.reloading) ? 0.5f : 1); //clamps movement by 0 or if walking/reloading by 0.5 or 1

            anim.SetFloat("Forward", movement, 0.1f, Time.deltaTime);
        }
    }

    void SetupAnimator()
    {
        anim = GetComponent<Animator>();

        Animator[] anims = GetComponentsInChildren<Animator>();

        //for (int i = 0; i < anims.Length; i++)
        //{
        //    if (anims[i] != anim)
        //    {
        //        anim.avatar = anims[i].avatar;
        //        Destroy(anims[i]);
        //        break;
        //    }
        //}
    }

    public void StartReload()
    {
        if (!states.reloading)
        {
            anim.SetTrigger("Reload");
        }
    }

    public void Reload1()
    {
        currentMag.enabled = false;
        GameObject spawnedMag = Instantiate(magSpawnObject, magSpawnPoint.transform.position, magSpawnPoint.transform.rotation);
        spawnedMag.transform.SetParent(null);
        audio.Reload1.Play();
    }

    public void Reload2()
    {
        audio.Reload2.Play();
    }

    public void Reload3()
    {
        audio.Reload3.Play();
        currentMag.enabled = true;
    }
}
