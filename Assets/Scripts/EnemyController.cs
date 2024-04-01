using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletSpot;

    private Animator myAnim;

    public float shootRate;
    public float bulletSpeed;
    private bool shooting;
    private float shootinTime;

    private void Start()
    {
        myAnim= GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(shooting)
        {
            shootinTime += Time.deltaTime;

            if(shootinTime > shootRate)
            {
                shootinTime = 0;
                Instantiate(bullet, bulletSpot.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed, 0);
            }
        }
    }

    public void Shoot()
    {
        shootinTime = 0;
        shooting = true;
        myAnim.SetBool("Shooting", true);
    }

    public void StopShoot()
    {
        shooting = false;
        myAnim.SetBool("Shooting", false);
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
