using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class Algo : MonoBehaviour
{
    public PlayerController[] controllers;
    public bool learn;
    public bool show;
    public float renderSpeed;
    public float roadSpeedModifier;
    public int maxIteration;
    public List<IA> ias = new List<IA>();
    public IA bestIA = new IA();
    public int iaPerGeneration;
    public float REFRESH_TIME_SECONDS = 0.01f;
    public int controllerCount;
    public Render render;
    public Road road;
    public int generation = 0;
    public int iaCount;

    void Start()
    {
        Application.targetFrameRate = 60;
        controllers = new PlayerController[controllerCount];
        for (int i = 0; i < controllerCount; i++)
        {
            Player player = new Player(new Game());
            player.game.player = player;
            controllers[i] = new PlayerController(player, MasterIA.GetRandomIA());
        }
        render.game = controllers[0].player.game;
        render.player = controllers[0].player;
        bestIA.SetScore(5);
        FastLearn();
    }

    IEnumerator AlgoLoop()
    {
        yield return new WaitForSeconds(1);
        int iteration = 0;
        int frames = 0;
        while (learn)
        {
            foreach (PlayerController controller in controllers)
            {
                iteration += 1;

                controller.GetIAInputs();
                controller.ia.Calcule();
                controller.ControlPlayer();
                controller.player.game.Loop();

                if (!controller.player.IsDead())
                {
                    if (!controller.player.hurt)
                    {
                        controller.ia.AddToScore(0.1f);
                    }

                    if (controller.ia.GetScore() > bestIA.GetScore() && !show)
                    {
                        yield return new WaitForSeconds(0.5f);
                        render.ClearScren();
                        show = true;
                        yield return new WaitForSeconds(1f);
                    }

                    if (frames % 3000 == 0 && renderSpeed < 20)
                    {
                        renderSpeed += 1;
                    }
                    frames++;
                }
                else
                {
                    ias.Add(controller.ia.Copy());
                    iaCount++;

                    if (ias.Count >= iaPerGeneration)
                    {
                        float score = bestIA.GetScore();
                        GetClassement();
                        float score2 = bestIA.GetScore();
                        print($"generation : {generation}");
                        generation += 1;
                        iteration = 0;
                        show = score2 > score;
                        yield return new WaitForSecondsRealtime(0.0000000000000000001f);
                    }

                    frames = 0;
                    renderSpeed = 3;

                    if (show)
                    {
                        render.ClearScren();
                    }

                    controller.player.game.ResetGame();
                    controller.ResetPlayer();
                    controller.ChangeToIA(GetBestIA());
                }
                if ((show && iteration % renderSpeed == 0) || iteration > maxIteration)
                {
                    yield return new WaitForSeconds(REFRESH_TIME_SECONDS);
                    iteration = 0;
                    road.Move(renderSpeed / roadSpeedModifier);

                }
            }
        }
    }

    /*void FixedUpdate()
    {
        AlgoLoop();
    }*/
    private void FastLearn()
    {
        StartCoroutine(AlgoLoop());
    }

    void GetClassement()
    {
        IA localBestIA = ias[0];
        foreach (IA ia in ias)
        {
            if (ia.GetScore() >= localBestIA.GetScore())
            {
                localBestIA = ia.Copy();
            }
        }
        Debug.Log(localBestIA.GetScore());
        if (localBestIA.GetScore() >= bestIA.GetScore())
        {
            if (localBestIA.GetScore() > bestIA.GetScore())
            {
                print($"New best score of {localBestIA.GetScore()}");
            }
            bestIA = localBestIA.Copy();

        }
        ias.Clear();
    }


    IA GetBestIA()
    {
        if (bestIA.GetScore() < 10)
        {
            return MasterIA.GetRandomIA();
        }
        else
        {
            return MasterIA.GetMutationIA(bestIA, 50);
        }
    }
}
