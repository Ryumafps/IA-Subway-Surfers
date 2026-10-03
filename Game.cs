using UnityEngine;
using System.Collections.Generic;
using System.Threading;

public class Game
{
    public Player player;
    public HashSet<Enemy> enemys = new HashSet<Enemy>();
    private HashSet<Enemy> enemysToRemove = new HashSet<Enemy>();
    public Dictionary<int, bool> ways = new Dictionary<int, bool>();
    public Dictionary<int, Enemy> waysBlocked = new Dictionary<int, Enemy>();
    public int waitEnemy;

    // Les voies sont vide a l'initialisation
    public Game()
    {
        ways.Add(1, false);
        ways.Add(2, false);
        ways.Add(3, false);
        waysBlocked.Add(1, null);
        waysBlocked.Add(2, null);
        waysBlocked.Add(3, null);
        ResetGame();
        //Thread t1 = new Thread(GameLoop);
    }

    /*private void FixedUpdate()
    {
        Loop();
    }*/

    /*private void Update()
    {
        Loop();
    }*/

    private void SpawnEnemys()
    {
        if (waitEnemy == 0)
        {
            SpawnRandomEnemy();
        }
        else
        {
            waitEnemy--;
        }
    }

    private void SpawnRandomEnemy()
    {
        if (!GetPattern())
        {
            Pattern1();
        }
        else
        {
            switch (Random.Range(1, 4))
            {
                case 1:
                    Pattern2(); break;
                case 2:
                    Pattern3(); break;
                case 3:
                    Pattern4(); break;
            }
        }
    }

    private bool GetPattern()
    {
        return Random.Range(1, 10) < 3;
    }

    private void Pattern1()
    {
        waitEnemy = 150;
        EnemyType type = RandomEnemyType();
        float speed = Random.Range(0.05f, 0.06f);
        float length = 2.8f;
        int way = Random.Range(1, 4);
        Enemy enemy = new Enemy(speed, type, length, way);
        enemys.Add(enemy);
    }

    private void Pattern2()
    {
        waitEnemy = 150;
        EnemyType type = RandomEnemyType();
        float speed = Random.Range(0.05f, 0.06f);
        float length = 2.8f;
        Enemy enemy1 = new Enemy(speed, type, length, 1);
        Enemy enemy2 = new Enemy(speed, type, length, 2);
        enemys.Add(enemy1);
        enemys.Add(enemy2);
    }

    private void Pattern3()
    {
        waitEnemy = 150;
        EnemyType type = RandomEnemyType();
        float speed = Random.Range(0.05f, 0.06f);
        float length = 2.8f;
        Enemy enemy1 = new Enemy(speed, type, length, 2);
        Enemy enemy2 = new Enemy(speed, type, length, 3);
        enemys.Add(enemy1);
        enemys.Add(enemy2);
    }

    private void Pattern4()
    {
        waitEnemy = 150;
        EnemyType type = RandomEnemyType();
        float speed = Random.Range(0.05f, 0.06f);
        float length = 2.8f;
        Enemy enemy1 = new Enemy(speed, type, length, 1);
        Enemy enemy2 = new Enemy(speed, type, length, 3);
        enemys.Add(enemy1);
        enemys.Add(enemy2);
    }

    public static EnemyType RandomEnemyType()
    {
        return (EnemyType)Random.Range(0, 3);
    }

    public void Loop()
    {
        UnblockWaysWhenEnemyIsGone();
        MoveCars();
        player.Loop();
        SpawnEnemys();
    }

    public void ResetGame()
    {
        Random.seed = 0;
        enemys.Clear();
        waysBlocked[1] = null;
        waysBlocked[2] = null;
        waysBlocked[3] = null;
        ways[1] = false;
        ways[2] = false;
        ways[3] = false;
        waitEnemy = 0;
    }


    private void UnblockAllWays()
    {
        foreach (var way in ways)
        {
            UnblockWay(way.Key);
        }
    }

    // Deplace toutes les vehicules sur la route
    public void MoveCars()
    {
        foreach (Enemy enemy in enemys)
        {
            enemy.Move();
            if (enemy.CanHurtPlayer())
            {
                int way = enemy.GetWay();
                BlockWay(way);
                waysBlocked[way] = enemy;
            }
            else if (enemy.IsGone())
            {
                enemysToRemove.Add(enemy);
            }
        }
        RemoveEnemys();
    }

    public bool IsPosBlocked(int way, float x)
    {
        foreach (Enemy enemy in enemys)
        {
            if (enemy.GetWay() == way && enemy.isInFrontOf(x))
            {
                return true;
            }
        }
        return false;
    }

    private void UnblockWaysWhenEnemyIsGone()
    {
        for (int way = 1; way < 4; way++)
        {
            Enemy enemyBlocking = waysBlocked[way];
            if (enemyBlocking != null && !enemyBlocking.CanHurtPlayer())
            {
                waysBlocked[way] = null;
                UnblockWay(way);
            }
        }
    }

    public Enemy GetEnemyWhoIsBlockingWay(int way)
    {
        return waysBlocked[way];
    }

    private void RemoveEnemys()
    {
        foreach (Enemy enemy in enemysToRemove)
        {
            enemys.Remove(enemy);
        }
        enemysToRemove.Clear();
    }

    // Bloque une voie pour le joueur
    private void BlockWay(int way)
    {
        ways[way] = true;
    }

    // Debloque une voie pour le joueur
    private void UnblockWay(int way)
    {
        ways[way] = false;
    }

    // Dit si une voie est bloque
    public bool WayIsBlocked(int way)
    {
        return ways[way];
    }
}
