using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(enemy && collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerMove>().Death();
            Destroy(gameObject);
        }
        else if(!enemy && collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyController>().Death();
            Destroy(gameObject);
        }
    }
}
