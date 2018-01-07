using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    private int _nball = 10;
    private float _wait = 0.1f;
    private Vector2 _direction;
    private int _remainingBalls;
    private List<Ball> _balls = new List<Ball>();
    private int _inactiveBalls;
    private int _multiplier = 1;

    public Ball _ball;

    private void Start()
    {
        for (int i = 0; i < _nball; i++)
        {
            Ball ball = Instantiate(_ball, transform, false);

            ball.SetSpawner(this);
            _balls.Add(ball);
        }
    }

    public void Shoot(Vector2 direction)
    {
        _multiplier = 1;
        _direction = direction.normalized;
        _remainingBalls = _nball;
        _inactiveBalls = 0;
        RecSpawn();
    }

    void RecSpawn()
    {
        if (_remainingBalls > 0)
        {
            int idx = _nball - _remainingBalls;
            Ball ball = _balls[idx];

            ball.Launch(_direction);
            ball.SetMultiplier(_multiplier);
            _remainingBalls--;
            Invoke("RecSpawn", _wait);
        }
    }

    public void IncreaseInactiveBalls()
    {
        _inactiveBalls++;
    }

    public bool IsFinish()
    {
        return _inactiveBalls == _nball;
    }

    public void Reset()
    {
        _inactiveBalls = 0;
    }

    public void Clear()
    {
        _remainingBalls = 0;
        foreach (Ball ball in _balls)
            ball.Clear();
        _inactiveBalls = _nball;
    }

    public void SetMultiplier(int multiplier)
    {
        _multiplier = multiplier;
        foreach (Ball ball in _balls)
            ball.SetMultiplier(multiplier);
    }
}
