using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Mirror;

public class PlayerCanvas : MonoBehaviour
{
    private static PlayerCanvas instance;

    [SerializeField] GameObject killsInfo;
    [SerializeField] GameObject plantInfo;
    [SerializeField] GameObject reticule;
    //[SerializeField] UIFader damageImage;
    [SerializeField] TMP_Text gameStatusText;
    [SerializeField] TMP_Text killsValue;
    [SerializeField] TMP_Text logText;
    [SerializeField] AudioSource deathAudio;

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

    private void Reset()
    {
        plantInfo = GameObject.Find("PlantInfo");
        reticule = GameObject.Find("Reticule");
        //damageImage = GameObject.Find("DamageFlash").GetComponent<UIFader>();
        gameStatusText = GameObject.Find("GameStatusText").GetComponent<TMP_Text>();
        killsValue = GameObject.Find("KillsValue").GetComponent<TMP_Text>();
        logText = GameObject.Find("LogText").GetComponent<TMP_Text>();
        deathAudio = GameObject.Find("DeathAudio").GetComponent<AudioSource>();
    }

    public void Initialize()
    {
        killsInfo.SetActive(true);
        reticule.SetActive(true);
        gameStatusText.text = "";
    }

    public void HideReticule()
    {
        reticule.SetActive(false);
    }

    //public void FlashDamageEffect()
    //{
    //    damageImage.Flash();
    //}

    public void PlayDeathAudio()
    {
        if (!deathAudio.isPlaying)
        {
            deathAudio.Play();
        }
    }

    public void SetKills(int amount)
    {
        killsValue.text = amount.ToString();
    }

    public void WriteGameStatusText(string text)
    {
        gameStatusText.text = text;
    }

    public void WriteLogText(string text, float duration)
    {
        CancelInvoke();
        logText.text = text;
        Invoke("ClearLogText", duration);
    }

    void ClearLogText()
    {
        logText.text = "";
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
