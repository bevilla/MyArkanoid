using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private float _speed = 20.0f;
    private Rigidbody2D rb2d = null;
    private int damage = 1;
    private Vector3 initialPosition;
    private BallSpawner spawner;
    private int multiplier = 1;
    private AudioSource audioSource;

    public void SetSpawner(BallSpawner spawner)
    {
        this.spawner = spawner;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        initialPosition = transform.position;
        gameObject.SetActive(false);
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction)
    {
        multiplier = 1;
        transform.position = new Vector3(spawner.transform.position.x, initialPosition.y, initialPosition.z);
        gameObject.SetActive(true);
        if (rb2d)
            rb2d.velocity = direction * _speed * multiplier;
    }

    private void FixedUpdate()
    {
        if (rb2d)
            rb2d.velocity = rb2d.velocity.normalized * _speed * multiplier;
    }

    public int GetDamage()
    {
        return damage;
    }

    public void Clear()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            spawner.IncreaseInactiveBalls();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            if (audioSource)
            {
                audioSource.PlayOneShot(audioSource.clip);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "BottomBorder")
        {
            Player player = transform.parent.parent.gameObject.GetComponent<Player>();

            if (player)
            {
                player.SetNewPosition(transform.position.x);
            }
            Clear();
        }
    }

    public void SetMultiplier(int multiplier)
    {
        this.multiplier = multiplier;
    }
}
