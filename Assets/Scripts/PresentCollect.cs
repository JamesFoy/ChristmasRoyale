using UnityEngine;
using UnityEngine.Events;

public class PresentCollect : MonoBehaviour
{
    [SerializeField] UnityEvent onCollectedPresent;
    [SerializeField] UnityEvent onDroppedPresent;

    public enum PresentState { notCollected, isCollected, hasDropped };
    public PresentState presentState = PresentState.notCollected;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (presentState == PresentState.isCollected)
        {
            Debug.Log("Present Collected");
            onCollectedPresent.Invoke();
        }
    }

    public void SetPresentPosition(Vector3 dropPoint)
    {
        transform.position = dropPoint;
    }
}

