using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";

    public bool hitEnemy = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BulletLifeTime());
    }

    IEnumerator BulletLifeTime()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }

    void OnCollision(Collider other)
    {
        if (other.gameObject.tag == PLAYER_TAG)
        {
            hitEnemy = true;

            if (hitEnemy == true)
            {
                CmdPlayerShot(other.name, 15);
            }
        }
    }

    [Command]
    void CmdPlayerShot(string _playerID, int _damage)
    {
        Debug.Log(_playerID + " has been shot");

        PlayerManager _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);
    }
}
