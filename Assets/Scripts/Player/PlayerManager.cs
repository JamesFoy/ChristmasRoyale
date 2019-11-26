using UnityEngine;
using Mirror;
using System.Collections;

public class PlayerManager : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }

        SetDefaults();
    }

    //public void Update()
    //{
    //    if (!isLocalPlayer)
    //        return;

    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //        RpcTakeDamage(999990);
    //    }
    //}

    [ClientRpc]
    public void RpcTakeDamage(int _amount)
    {
        if (isDead)
            return;

        currHealth -= _amount;

        Debug.Log(transform.name + " now has " + currHealth + " health.");

        if (currHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        //DISABLE COMPONENTS
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = false;
        }

        Debug.Log(transform.name + " is DEAD!!");

        //CALL RESPAWN METHOD
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

        SetDefaults();
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        Debug.Log(transform.name + " respawned");
    }

    public void SetDefaults()
    {
        isDead = false;

        currHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = true;
        }
    }
}
