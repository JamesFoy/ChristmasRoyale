using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCanvas : MonoBehaviour
{
    private static PlayerCanvas instance;

    [SerializeField] GameObject plantInfo;

    bool bombInZone;

    public static PlayerCanvas Instance { get { return instance; } }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bombInZone)
        {
            plantInfo.SetActive(true);
        }
        else
        {
            plantInfo.SetActive(false);
        }
    }

    public void IsBombInZone(bool value)
    {
        bombInZone = value;
    }
}
