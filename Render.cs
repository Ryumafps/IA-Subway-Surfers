using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Render : MonoBehaviour
{
    public Game game;
    public Player player;
    public Algo algo;
    public GameObject enemyPrefab;
    public Sprite[] sprites;
    private Dictionary<Enemy, GameObject> enemysRenders = new Dictionary<Enemy, GameObject>();
    Stack<GameObject> freeGameObjects = new Stack<GameObject>();
    private HashSet<GameObject> usedGameObject = new HashSet<GameObject>();
    public Text generation;
    public Text iaGeneration;
    public Text score;
    public Text iaTotal;
    public Text scoreIA;
    public GameObject playerRender;
    public int EnemyCount;
    public bool way1Blocked;
    public bool way2Blocked;
    public bool way3Blocked;

    void Update()
    {
        if (algo.show)
        {
            EnemyCount = game.enemys.Count;
            ShowPlayer();
            ShowEnemys();
            ShowStats();
            Debug();
        }
    }

    private void Debug()
    {
        way1Blocked = game.WayIsBlocked(1);
        way2Blocked = game.WayIsBlocked(2);
        way3Blocked = game.WayIsBlocked(3);
    }

    private void ShowPlayer()
    {
        Vector3 playerPos = new Vector3(-5.5f, 0, 0);
        if (!player.IsDead())
        {
            playerPos.y = Constants.posYForPosition[player.GetPos()];
        }
        else
        {
            playerPos.x = -5f;
            playerPos.y = Constants.posYForPosition[player.GetPos()];
            //playerPos = new Vector3(-20f, 0, 0);
        }
        playerRender.transform.position = playerPos;
    }

    public void ClearScren()
    {
        foreach (GameObject obj in enemysRenders.Values)
        {
            freeGameObjects.Push(obj);
            obj.transform.position = new Vector3(50, 0, 0);
        }
        enemysRenders.Clear();
        usedGameObject.Clear();
    }

    private void ShowEnemys()
    {
        if (game.enemys.Count == 0)
        {
            ClearScren();
        }
        foreach (Enemy enemy in game.enemys)
        {
            if (enemysRenders.ContainsKey(enemy))
            {
                Vector3 pos = new Vector3(enemy.GetX(), 0, 0);
                pos.y = Constants.posYForPosition[enemy.GetWay()];
                enemysRenders[enemy].transform.position = pos;
            }
            else
            {
                if (freeGameObjects.Count > 0)
                {
                    GameObject render = freeGameObjects.Pop();
                    render.GetComponentInChildren<SpriteRenderer>().sprite = sprites[(int)enemy.GetType()];
                    usedGameObject.Add(render);
                    enemysRenders[enemy] = render;
                }
                else
                {
                    if (usedGameObject.Count < 20)
                    {
                        GameObject render = Instantiate(enemyPrefab);
                        render.GetComponentInChildren<SpriteRenderer>().sprite = sprites[(int)enemy.GetType()];
                        usedGameObject.Add(render);
                        enemysRenders[enemy] = render;
                    }
                }
            }
        }
        HashSet<Enemy> enemyToRemove = new HashSet<Enemy>();
        foreach (Enemy enemy in enemysRenders.Keys)
        {
            if (enemy.IsGone())
            {
                enemyToRemove.Add(enemy);
            }
        }
        foreach (Enemy enemy in enemyToRemove)
        {
            freeGameObjects.Push(enemysRenders[enemy]);
            enemysRenders.Remove(enemy);
        }
    }

    private void ShowStats()
    {
        generation.text = "Generation : " + algo.generation.ToString();
        iaGeneration.text = "IA : " + algo.ias.Count.ToString();
        iaTotal.text = "Total IA : " + algo.iaCount.ToString();
        score.text = "Meilleur Score : " + algo.bestIA.GetScore().ToString();
        try
        {
            scoreIA.text = "Score : " + algo.controllers[0].ia.GetScore().ToString();
        }
        catch { }
    }
}
