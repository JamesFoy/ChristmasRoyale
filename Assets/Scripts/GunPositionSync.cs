using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GunPositionSync : NetworkBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform handMount;
    [SerializeField] Transform gunPivot;
    [SerializeField] float threshold = 10f;
    [SerializeField] float smoothing = 5f;

    [SyncVar] float pitch;
    Vector3 lastOffset;
    float lastSyncedPitch;

    private void Start()
    {
        if (isLocalPlayer)
        {
            gunPivot.parent = cameraTransform;
        }
        else
        {
            lastOffset = handMount.position - transform.position;
        }
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            pitch = cameraTransform.localRotation.eulerAngles.x;

            if (Mathf.Abs(lastSyncedPitch - pitch) >= threshold)
            {
                CmdUpdatePitch(pitch);
                lastSyncedPitch = pitch;
            }
        }
        else
        {
            Quaternion newRotation = Quaternion.Euler(pitch, 0f, 0f);

            Vector3 currentOffset = handMount.position - transform.position;
            gunPivot.localPosition += currentOffset - lastOffset;
            lastOffset = currentOffset;

            gunPivot.localRotation = Quaternion.Lerp(gunPivot.localRotation, newRotation, Time.deltaTime * smoothing);
        }
    }

    [Command]
    void CmdUpdatePitch(float newPitch)
    {
        pitch = newPitch;
    }
}
