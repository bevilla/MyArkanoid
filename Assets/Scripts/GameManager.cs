using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player;
    public BallSpawner ballSpawner;
    public EnemySpawner enemySpawner;

    private State state;

    public abstract class State
    {
        private State next = null;

        public void SetNext(State next)
        {
            this.next = next;
        }
        public abstract bool CanGoNext();
        public abstract void Update();
        public abstract void Start();
        public State Next()
        {
            return next;
        }
    }

    public class Idle : State
    {
        private BallSpawner ballSpawner;
        private EnemySpawner enemySpawner;
        private LineRenderer line;
        private bool canGoNext = false;

        public Idle(BallSpawner ballSpawner, EnemySpawner enemySpawner)
        {
            this.ballSpawner = ballSpawner;
            this.enemySpawner = enemySpawner;
            this.line = this.ballSpawner.GetComponent<LineRenderer>();
        }

        public override bool CanGoNext()
        {
            return canGoNext;
        }

        public override void Start()
        {
            canGoNext = false;
            ballSpawner.Reset();
            enemySpawner.SpawnLine();
        }

        public override void Update()
        {
            Vector2 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = click - (new Vector2(ballSpawner.transform.position.x, ballSpawner.transform.position.y));

            line.numPositions = 0;
            if (direction.y > 0)
            {
                if (Input.GetButton("Fire1"))
                {
                    RaycastHit2D info = Physics2D.Raycast(ballSpawner.transform.position, direction);

                    line.numPositions = 2;
                    line.SetPosition(0, ballSpawner.transform.position);
                    line.SetPosition(1, info.point);
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    ballSpawner.Shoot(direction);
                    canGoNext = true;
                }
            }
        }
    }

    public class Shoot : State
    {
        private BallSpawner ballSpawner;
        private EnemySpawner enemySpawner;

        public Shoot(BallSpawner ballSpawner, EnemySpawner enemySpawner)
        {
            this.ballSpawner = ballSpawner;
            this.enemySpawner = enemySpawner;
        }

        public override void Start()
        {
        }

        public override bool CanGoNext()
        {
            return ballSpawner.IsFinish() || enemySpawner.IsEmpty();
        }

        public override void Update()
        {
        }
    }

    public class Move : State
    {
        private Player player;
        private BallSpawner ballSpawner;

        public Move(Player player, BallSpawner ballSpawner)
        {
            this.player = player;
            this.ballSpawner = ballSpawner;
        }
        public override bool CanGoNext()
        {
            return !player.IsMoving();
        }

        public override void Start()
        {
            ballSpawner.Clear();
            player.Move();
        }

        public override void Update()
        {
        }
    }

    void Start()
	{
        Idle idleState = new Idle(ballSpawner, enemySpawner);
        Shoot shootState = new Shoot(ballSpawner, enemySpawner);
        Move moveState = new Move(player, ballSpawner);

        idleState.SetNext(shootState);
        shootState.SetNext(moveState);
        moveState.SetNext(idleState);

        state = idleState;
        state.Start();
	}
	
	void Update()
	{
        if (state != null)
        {
            if (state.CanGoNext())
            {
                state = state.Next();
                state.Start();
            }
            state.Update();
        }

        if (player.Hp() <= 0)
        {
            state = null;
            SceneManager.LoadScene("title");
        }
    }
}
