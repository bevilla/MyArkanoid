using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int maxhp = 10;
    private int hp = 10;
    private int damage = 1;
    private TextMesh hpText;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if (child.name == "HP")
                hpText = child.GetComponent<TextMesh>();
        }
    }

    public void Update()
    {
        if (hpText)
            hpText.text = hp.ToString();
    }

    private void OnEnable()
    {
        hp = maxhp;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();

        if (collision.gameObject.GetComponent<Ball>())
        {
            hp -= ball.GetDamage();
            if (hp <= 0)
                gameObject.SetActive(false);
        }
    }

    public void Damage(Player player)
    {
        player.Damage(damage);
    }
}
