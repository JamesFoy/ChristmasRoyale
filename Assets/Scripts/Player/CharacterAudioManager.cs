using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudioManager : MonoBehaviour
{
    public AudioSource gunSounds;
    public AudioSource runFoley;

    public AudioSource Reload1;
    public AudioSource Reload2;
    public AudioSource Reload3;

    public float footStepTimer;
    public float walkThreshold;
    public float runThreshold;
    public AudioSource footStep1;
    public AudioSource footStep2;
    public AudioClip[] footStepClips;
    public AudioSource effectsSource;
    public AudioClipsList[] effectsLists;
    StateManager states;

    float startingVolumeRun;
    float characterMovement;

    // Start is called before the first frame update
    void Start()
    {
        states = GetComponent<StateManager>();
        startingVolumeRun = runFoley.volume;

        runFoley.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //check if the character is moving by adding the horizontal and vertical movements.
        characterMovement = Mathf.Abs(states.horizontal) + Mathf.Abs(states.vertical);
        //Also clamp it between 0 and 1 to reduce the value
        characterMovement = Mathf.Clamp01(characterMovement);

        //how fast the timer is betweeen footsteps
        float targetThreshold = 0;

        //Find if we are running. Done by checking if we are doing any action that causes walking
        if (!states.walk && !states.aiming && !states.reloading)
        {
            //whatever value is set in editor for volume, it will be used as the maximum volume
            runFoley.volume = startingVolumeRun * characterMovement;
            targetThreshold = runThreshold;
        }
        else
        {
            targetThreshold = walkThreshold;

            runFoley.volume = Mathf.Lerp(runFoley.volume, 0, Time.deltaTime * 2);
        }

        if (characterMovement == 0)
        {

            footStep1.Stop();
            footStep2.Stop();

        //    footStepTimer += Time.deltaTime;

        //    if (footStepTimer > targetThreshold)
        //    {
        //        PlayFootStep();
        //        footStepTimer = 0;
        //    }
        //}
        //else
        //{
        //    footStep1.Stop();
        //    footStep2.Stop();
        }
    }

    public void PlayGunSound()
    {
        gunSounds.Play();
    }

    public void PlayFootStep()
    {
        //if footstep 1 is not playing
        if (!footStep1.isPlaying)
        {
            //find a random clip within the list
            int ran = Random.Range(0, footStepClips.Length);
            //assign it to the audiosource
            footStep1.clip = footStepClips[ran];
            //and play the clip
            footStep1.Play();
        }
        else
        {
            //if footstep 2 is not playing
            if (!footStep2.isPlaying)
            {
                //find a random clip within the list
                int ran2 = Random.Range(0, footStepClips.Length);
                //assign it to the audiosource
                footStep2.clip = footStepClips[ran2];
                //and play the clip
                footStep2.Play();
            }
        }
    }

    public void PlayEffect(string name)
    {
        //prevents errors
        AudioClip clip = null;

        //searches through the effects list
        for (int i = 0; i < effectsLists.Length; i++)
        {
            //compares the name of each item within the list to the string passed
            if (string.Equals(effectsLists[i].name, name))
            {
                //sets the clip to the clip from the list
                clip = effectsLists[i].clip;
                break;
            }
        }

        //Play clip
        effectsSource.clip = clip;
        effectsSource.Play();
    }
}

[System.Serializable]
public class AudioClipsList
{
    public string name;
    public AudioClip clip;
}
