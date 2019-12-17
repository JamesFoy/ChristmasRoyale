using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PresentCollect : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 0.2f, 0);
    private static PresentCollect instance;
    private float elapsedTime;

    [SerializeField] UnityEvent onCollectedPresent;
    [SerializeField] public UnityEvent onDroppedPresent;
    [SerializeField] UnityEvent onPresentExplode;
    [SerializeField] UnityEvent onPresentPlanted;
    [SerializeField] AudioSource bombClip;

    public static PresentCollect Instance {get { return instance; } }

    public float timerCountdown;
    public float soundTimer;

    public enum PresentState { notCollected, isCollected, hasDropped, planted, exploded };
    public PresentState presentState = PresentState.notCollected;

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
        if (presentState == PresentState.isCollected)
        {
            onCollectedPresent.Invoke();
        }

        if (presentState == PresentState.hasDropped || presentState == PresentState.planted)
        {
            onDroppedPresent.Invoke();
        }

        if (presentState == PresentState.planted)
        {
            //Start Countdown
            StartCoroutine(PresentCountDown(timerCountdown));
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= soundTimer)
            {
                elapsedTime = 0f;
                bombClip.Play();
            }
            else
            {
                if (bombClip.isPlaying)
                    return;

                bombClip.Stop();
            }

            //Allow Defuse Mechanic
            onPresentPlanted.Invoke();
        }

        if (presentState != PresentState.planted)
        {
            StopAllCoroutines();
        }
    }

    IEnumerator PresentCountDown(float timer)
    {
        //When Countdown ends play explosion effect
        yield return new WaitForSeconds(timer);
        onPresentExplode.Invoke();
        presentState = PresentState.exploded;
    }

    public void SetPresentPosition(Transform dropPoint)
    {
        this.gameObject.transform.position = dropPoint.position + offset;
    }
}

