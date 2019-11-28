using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleShooting : MonoBehaviour
{

    StateManager states;
    public Animator weaponAnim;
    public Animator ScarHAnim;
    public float fireRate;
    float timer;
    public Transform bulletSpawnPoint;
    public GameObject smokeParticle;
    public GameObject bloodParticle;
    public ParticleSystem[] muzzle;
    public GameObject casingPrefab;
    public Transform caseSpawn;

    public int curBullets = 30;

    bool shoot;
    bool dontShoot;

    bool emptyGun;

    //add the singleton
    public static HandleShooting instance;

    private void Awake()
    {
        instance = this;
    }

    public static HandleShooting GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        states = GetComponent<StateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        shoot = states.shoot;

        if (curBullets <= 29 && Input.GetKey(KeyCode.R))
        {
            states.handleAnim.StartReload();
            curBullets = 30;
        }

        if (shoot)
        {
            if (timer <= 0)
            {
                //ScarHAnim.SetBool("Shoot", false);
                weaponAnim.SetBool("Shoot", false);

                if (curBullets > 0)
                {
                    emptyGun = false;
                    states.audioManager.PlayGunSound();

                    GameObject go = Instantiate(casingPrefab, caseSpawn.position, caseSpawn.rotation) as GameObject;
                    Rigidbody rig = go.GetComponent<Rigidbody>();
                    rig.AddForce(transform.right.normalized * 2 + Vector3.up * 1.3f, ForceMode.Impulse);
                    rig.AddRelativeForce(go.transform.right * 1.5f, ForceMode.Impulse);

                    for (int i = 0; i < muzzle.Length; i++)
                    {
                        muzzle[i].Emit(1);
                    }

                    //RaycastShoot();

                    curBullets -= 1;
                }
                else
                {
                    if (emptyGun)
                    {
                        states.handleAnim.StartReload();
                        curBullets = 30;
                    }
                    else
                    {
                        states.audioManager.PlayEffect("empty_gun");
                        emptyGun = true;
                    }
                }

                timer = fireRate;
            }
            else
            {
                //ScarHAnim.SetBool("Shoot", true);
                weaponAnim.SetBool("Shoot", true);

                timer -= Time.deltaTime;
            }
        }
        else
        {
            timer = -1;
            //ScarHAnim.SetBool("Shoot", false);
            weaponAnim.SetBool("Shoot", false);
        }
    }

    //void RaycastShoot()
    //{
    //    Vector3 direction = states.lookHitPoistion - bulletSpawnPoint.position;
    //    RaycastHit hit;

    //    if (Physics.Raycast(bulletSpawnPoint.position, direction, out hit, 100, states.layerMask))
    //    {
    //    //Do something to the target hit (enemy)

    //        if (hit.transform.CompareTag("Enemy"))
    //        {
    //            //Damage enemy
    //            hit.collider.gameObject.GetComponentInParent<ZombieBase>().Damage(1);
    //            //spawn blood effect on enemy
    //            GameObject go = Instantiate(bloodParticle, hit.point, Quaternion.identity) as GameObject;
    //            go.transform.LookAt(bulletSpawnPoint.position);
    //        }
    //        else
    //        {
    //            //if bullet hits anything else spawn a smoke effect where the bullet hits
    //            GameObject go = Instantiate(smokeParticle, hit.point, Quaternion.identity) as GameObject;
    //            go.transform.LookAt(bulletSpawnPoint.position);
    //        }
    //    }
    //}
}
