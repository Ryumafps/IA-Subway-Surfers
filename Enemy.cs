using UnityEngine;
using System;

[Serializable]
public class Enemy
{
    // position gauche de la voiture
    private float x1;
    // position droite de la voiture
    private float x2;
    private int way;
    private float speed;
    private EnemyType type;
    private float length;
    private bool isGone;

    // Cree un enemy avec la vitesse,taille,voie sur laquelle il est
    public Enemy(float s, EnemyType t, float l, int w)
    {
        speed = s;
        type = t;
        length = l;
        way = w;
        x1 = Constants.enemyStartX;
        x2 = x1 + length;
    }

    // Deplace l'enemy en fonction de sa vitesse
    public void Move()
    {
        x1 -= speed;
        x2 = x1 + length;
        if (x2 < -10)
        {
            isGone = true;
        }
    }

    // Dit la voie sur laquelle l'enemy est
    public int GetWay()
    {
        return way;
    }

    public float GetX()
    {
        return x1;
    }

    // Dit l'enemy est en dehors de l'ecran
    public bool IsGone()
    {
        return isGone;
    }

    public EnemyType GetType()
    {
        return type;
    }

    // Renvoie si l'enemy est au niveau du joueur sur la voie
    public bool CanHurtPlayer()
    {
        return x1 <= Constants.playerPosX && x2 >= Constants.playerPosX;
    }

    public bool isInFrontOf(float x)
    {
        return x1 <= x && x2 >= Constants.playerPosX;
    }
}