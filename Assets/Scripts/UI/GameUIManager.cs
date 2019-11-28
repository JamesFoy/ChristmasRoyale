using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    HandleShooting handleShooting;
    public TMP_Text ammoText;
    private int ammoCounter = 0;

    //add the singleton
    public static GameUIManager instance;

    private void Awake()
    {
        instance = this;
    }

    public static GameUIManager GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        handleShooting = HandleShooting.GetInstance();
        OffText();
    }

    // Update is called once per frame
    void Update()
    {
        ammoCounter = handleShooting.curBullets;
        ammoText.text = "Bullets Remaining: " + ammoCounter;
    }

    public void OnText()
    {
        ammoText.enabled = true;
    }

    public void OffText()
    {
        ammoText.enabled = false;
    }
}
