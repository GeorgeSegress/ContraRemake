using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public enum ShootingDirection
{
    Down,
    ForwardDown,
    Forward,
    ForwardUp,
    Up
};

public class PlayerMove : MonoBehaviour
{
    public Transform groundCheck;
    public Transform bulletSpot;
    public GameObject bullet;
    private Rigidbody2D myBod;
    private Animator myAnim;
    private LevelManager myBoss;

    public LayerMask groundLayer;
    private ShootingDirection currentDirection = ShootingDirection.Forward;

    public Vector2[] colliderScales;
    private BoxCollider2D myCollider;
    public float speed;
    public float jumpHeight;
    public float bulletSpeed;
    private float intSpeed;
    private float shootingInt = 0;
    [HideInInspector] public Vector2 moveInp;
    private int rightDir = 1;
    private bool grounded;
    private bool swimming;
    private bool dead = false;
    private bool squatted = false;

    public GameObject jumpSFX;
    public GameObject deathSFX;

    private void Start()
    {
        myBoss = FindObjectOfType<LevelManager>();
        myAnim = GetComponentInChildren<Animator>();
        myBod = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();

        intSpeed = speed;
    }

    private void Update()
    {
        if (!dead)
        {
            if (shootingInt > 0)
                shootingInt -= Time.deltaTime;

            grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer) && myBod.velocity.y < 0.5f;

            moveInp.x = Input.GetAxis("Horizontal");
            moveInp.y = Input.GetAxis("Vertical");

            moveInp.Normalize();
            CheckDirection();
            if (moveInp.x < 0 && rightDir != -1)
            {
                rightDir = -1;
                transform.localScale = new Vector3(-1, 1);
            }
            else if (moveInp.x > 0 && rightDir != 1)
            {
                rightDir = 1;
                transform.localScale = new Vector3(1, 1);
            }
            if (!squatted && moveInp.y < -0.8f)
                Squat();
            else if (squatted && moveInp.y >= -0.8f)
                UnSquat();

            // if movement is fully forward
            if (Mathf.Abs(moveInp.x) > .25f)
                myBod.velocity = new Vector2(myBod.velocity.x * .650f + moveInp.x * intSpeed * 0.35f, myBod.velocity.y);
            // if movement is only partially forward (mostly for controllers)
            else if (Mathf.Abs(moveInp.x) <= .25f)
                myBod.velocity = new Vector2(myBod.velocity.x * 0.60f, myBod.velocity.y);

            if (Input.GetButtonDown("Jump") && moveInp.y > -0.8f && grounded)
                Jump();

            if (Input.GetButtonDown("Fire"))
                Shoot();

            if (Input.GetButtonDown("Pause"))
                Pause();

            AnimatorUpdate();
        }
    }

    public void Squat()
    {
        myCollider.offset = colliderScales[2]; 
        myCollider.size = colliderScales[3];
        squatted = true;
    }

    public void UnSquat()
    {
        myCollider.offset = colliderScales[0];
        myCollider.size = colliderScales[1];
        squatted = false;
    }

    public void Jump()
    {
        Instantiate(jumpSFX);
        myBod.velocity = new Vector2(myBod.velocity.x, jumpHeight);
    }

    public void Pause()
    {
        myBoss.PauseGame();
    }

    public void CheckDirection()
    {
        if (moveInp.y > 0.8f)
            currentDirection = ShootingDirection.Up;
        else if (moveInp.y >= 0.33f)
            currentDirection = ShootingDirection.ForwardUp;
        else if (Mathf.Abs(moveInp.y) < 0.33f)
            currentDirection = ShootingDirection.Forward;
        else if (moveInp.y < -0.8f)
            currentDirection = ShootingDirection.Down;
        else if (moveInp.y <= -0.33f)
            currentDirection = ShootingDirection.ForwardDown;
    }

    public void Shoot()
    {
        shootingInt = 0.5f;
        switch(currentDirection)
        {
            case ShootingDirection.Up:
                Instantiate(bullet, bulletSpot.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(0, bulletSpeed);
                break;
            case ShootingDirection.ForwardUp:
                Instantiate(bullet, bulletSpot.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(rightDir * bulletSpeed / 2, bulletSpeed / 2);
                break;
            case ShootingDirection.Forward:
                Instantiate(bullet, bulletSpot.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed * rightDir, 0);
                break;
            case ShootingDirection.Down:
                Instantiate(bullet, bulletSpot.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed * rightDir, 0);
                break;
            case ShootingDirection.ForwardDown:
                Instantiate(bullet, bulletSpot.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(rightDir * bulletSpeed / 2, -bulletSpeed / 2);
                break;
        }
    }

    public void AnimatorUpdate()
    {
        myAnim.SetBool("Grounded", false);
        myAnim.SetBool("Shooting", false);
        myAnim.SetBool("Running", false);
        myAnim.SetBool("Swimming", false); 

        if (!dead)
        {
            if (!grounded)
                myAnim.SetBool("Grounded", false);
            else
            {
                myAnim.SetBool("Grounded", true);
                myAnim.SetBool("Shooting", shootingInt > 0);
                myAnim.SetInteger("ShootingDirection", (int)currentDirection);
                myAnim.SetBool("Running", moveInp.x != 0);
                myAnim.SetBool("Swimming", swimming); 
            }
        }
        else
        {
            myAnim.SetBool("Dead", true);
        }
    }

    public void EnteredWater(bool entering)
    {
        swimming = entering;
    }

    public void Win()
    {
        myBoss.Victory();
    }

    public void Death()
    {
        Instantiate(deathSFX);
        myBod.velocity = Vector2.zero;
        myBoss.PlayerDeath();
        dead = true;
        AnimatorUpdate();
    }
}
