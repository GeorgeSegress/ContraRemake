using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private bool activated;
    private GameObject myPlayer;
    private Collider2D hardCollider;

    private void Start()
    {
        hardCollider= GetComponent<Collider2D>();
    }

    private void Update()
    {
        if(activated && myPlayer.GetComponent<PlayerMove>().moveInp.y <= -0.8f && Input.GetButtonDown("Jump"))//Input.GetButtonDown("Jump") && Input.GetButton("Down"))
        {
            StartCoroutine(DropDelay());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            activated = true;
            myPlayer = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            activated = false;
    }

    IEnumerator DropDelay()
    {
        Physics2D.IgnoreCollision(myPlayer.GetComponent<Collider2D>(), hardCollider, true);
        yield return new WaitForSeconds(.5f);
        Physics2D.IgnoreCollision(myPlayer.GetComponent<Collider2D>(), hardCollider, false);
    }
}
