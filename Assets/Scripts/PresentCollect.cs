using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PresentCollect : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 0.2f, 0);
    private static PresentCollect instance;

    [SerializeField] UnityEvent onCollectedPresent;
    [SerializeField] public UnityEvent onDroppedPresent;
    [SerializeField] UnityEvent onPlantedPresent;

    public static PresentCollect Instance {get { return instance; } }

    public float timerCountdown;

    public enum PresentState { notCollected, isCollected, hasDropped, planted };
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
        }
    }

    IEnumerator PresentCountDown(float timer)
    {
        //When Countdown ends play explosion effect
        yield return new WaitForSeconds(timer);
    }

    public void SetPresentPosition(Transform dropPoint)
    {
        this.gameObject.transform.position = dropPoint.position + offset;
    }
}

