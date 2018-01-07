using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private int hp = 5;
    private float timeToMove = 0.5f;
    private float timeElapsed = 0.0f;
    private bool isMoving = false;
    private Vector3 newPosition;
    private Vector3 prevPosition;
    private bool canSetPos = true;

    public Text hpText;

    void Start()
	{
        newPosition = transform.position;
	}
	
	void Update()
	{
        if (isMoving)
        {
            if (timeElapsed < timeToMove)
            {
                timeElapsed += Time.deltaTime;
                float t = Mathf.Clamp(timeElapsed / timeToMove, 0, 1);
                transform.position = Vector3.Lerp(prevPosition, newPosition, t);
            }
            else
            {
                canSetPos = true;
                isMoving = false;
            }
        }

        hpText.text = hp.ToString();
	}

    public void Move()
    {
        if (prevPosition != newPosition)
        {
            timeElapsed = 0.0f;
            prevPosition = transform.position;
            isMoving = true;
        }
    }

    public void Damage(int damage)
    {
        hp -= damage;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public int Hp()
    {
        return hp;
    }

    public void SetNewPosition(float x)
    {
        if (canSetPos)
        {
            newPosition = transform.position;
            newPosition.x = x;
            canSetPos = false;
        }
    }
}
