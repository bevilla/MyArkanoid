using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private Vector3 initialPos;
    private float paddingX = 1.549f;
    private float paddingY = -1.5492857f;
    private List<List<Enemy>> enemies;
    private int width = 6;
    private int height = 9;

    public Enemy _enemy;
    public Player player;

    void ShuffleList<T>(ref List<T> list)
    {
        list.Sort((x, y) => Random.value < 0.5 ? -1 : 1);
    }

    void Start()
	{
        enemies = new List<List<Enemy>>();
        for (int i = 0; i < height; i++)
        {
            enemies.Add(new List<Enemy>());
            for (int j = 0; j < width; j++)
            {
                Enemy enemy = Instantiate(_enemy, transform, false);
                enemy.gameObject.SetActive(false);
                enemies[i].Add(enemy);
            }
        }
        SetEnemiesPosition();
	}

    void SetEnemiesPosition()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Enemy enemy = enemies[i][j];
                Vector3 pos = transform.position;

                pos.x += j * paddingX;
                pos.y += i * paddingY;
                enemy.transform.position = pos;
            }
        }
    }

    public void SpawnLine()
    {
        int n = Random.Range(1, 6);
        int i = 0;
        foreach (Enemy enemy in enemies[0])
        {
            if (i < n)
            {
                enemy.gameObject.SetActive(true);
            }
            else
            {
                enemy.gameObject.SetActive(false);
            }
            i += 1;
        }
        List<Enemy> l = enemies[0];
        ShuffleList(ref l);

        List<Enemy> first = enemies[height - 1];
        for (i = (height - 1); i > 0; i--)
        {
            enemies[i] = enemies[i - 1];
        }
        enemies[0] = first;
        foreach (Enemy enemy in first)
        {
            if (enemy.gameObject.activeSelf)
            {
                enemy.Damage(player);
                enemy.gameObject.SetActive(false);
            }
        }
        SetEnemiesPosition();
    }

    public bool IsEmpty()
    {
        foreach (List<Enemy> list in enemies)
        {
            foreach (Enemy enemy in list)
            {
                if (enemy.gameObject.activeSelf)
                    return false;
            }
        }
        return true;
    }
}
