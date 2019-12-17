using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] float shotCooldown = .3f;
    [SerializeField] Transform firePosition;
    [SerializeField] ShotEffectsManager shotEffects;
    [SerializeField] int killsToWin = 25;

    [SyncVar (hook = "OnScoreChanged")] int score;

    [SerializeField] Animator gunAnim;

    Player player;
    float ellaspedTime;
    bool canShoot;

    private void Start()
    {
        player = GetComponent<Player>();
        shotEffects.Initialize();

        if (isLocalPlayer)
        {
            canShoot = true;
        }
    }

    private void Update()
    {
        if (!canShoot)
            return;

        ellaspedTime += Time.deltaTime;

        if (Input.GetButton("Fire1") && ellaspedTime > shotCooldown)
        {
            ellaspedTime = 0f;
            gunAnim.SetTrigger("Shot");
            CmdFireShot(firePosition.position, firePosition.forward);
        }
    }

    [Command]
    void CmdFireShot(Vector3 origin, Vector3 direction)
    {
        RaycastHit hit;

        Ray ray = new Ray(origin, direction);
        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.red, 1f);

        bool result = Physics.Raycast(ray, out hit, 50f);

        if (result)
        {
            PlayerHealth enemy = hit.transform.GetComponent<PlayerHealth>();

            if (enemy != null)
            {
                bool wasKillShot = enemy.TakeDamage();

                if (wasKillShot && ++score >= killsToWin)
                {
                    player.Won();
                }
            }
        }

        RpcProcessShotEffects(result, hit.point);
    }

    [ClientRpc]
    void RpcProcessShotEffects(bool result, Vector3 point)
    {
        //Play shot effects muzzle flash etc
        shotEffects.PlayShotEffects();

        if (result)
        {
            //play impact effect at point
            shotEffects.PlayImpactEffect(point);
        }
    }

    void OnScoreChanged(int value)
    {
        score = value;
        if (isLocalPlayer)
        {
            PlayerCanvas.Instance.SetKills(value);
        }
    }
}
