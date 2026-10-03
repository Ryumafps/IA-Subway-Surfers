using UnityEngine;

[System.Serializable]
public class Player
{
    public bool up, down, jump, roll;
    public bool jumping;
    public bool grounded = true;
    public int position;
    public bool rolling;
    public int rollCount;
    public int jumpTime;
    public int jumpCount;
    public bool hurt;
    public int hurtCounter;
    public bool dead;
    public Game game;

    public Player(Game g)
    {
        position = 2;
        game = g;
    }

    // Boucle du jeu
    public void Loop()
    {
        GetBools();
        //GetInputs();
        Move();
        if (GetRoulerDessus())
        {
            Die();
        }
    }

    public void Reset()
    {
        up = false;
        down = false;
        jump = false;
        roll = false;
        position = 2;
        jumpCount = 0;
        hurt = false;
        hurtCounter = 0;
        dead = false;
    }

    private bool GetRoulerDessus()
    {
        bool blocked = game.WayIsBlocked(position);
        Enemy enemyBlocking = game.GetEnemyWhoIsBlockingWay(position);
        return blocked && !(jumping && enemyBlocking.GetType() == EnemyType.Barrier);
    }

    // Fix la merde
    private void GetBools()
    {
        hurt = hurtCounter > 0;
        if (hurtCounter > 0)
        {
            hurtCounter--;
        }
        if (jumpCount > 0)
        {
            jumpCount--;
            jumping = true;
        }
        else
        {
            jumping = false;
        }
        grounded = !jumping;
    }

    // Le nom dit tout
    private void GetInputs()
    {
        up = UnityEngine.Input.GetKeyDown(KeyCode.W);
        down = UnityEngine.Input.GetKeyDown(KeyCode.S);
        jump = UnityEngine.Input.GetKeyDown(KeyCode.Space);
        roll = UnityEngine.Input.GetKeyDown(KeyCode.RightArrow);
    }

    public int GetPos()
    {
        return position;
    }

    // Permet de faire les mouvements du joueur si les bonnes conditions sont reunis
    private void Move()
    {
        if (up && !down)
        {
            MoveUp();
        }
        else if (down && !up)
        {
            MoveDown();
        }
        if (jump && !jumping && grounded)
        {
            Jump();
        }
    }

    // Se deplace en haut si la voie n'est pas bloque
    private void MoveUp()
    {
        if (position == 3)
        {
            Hurt();
        }
        else
        {
            if (!game.WayIsBlocked(position + 1))
            {
                position += 1;
            }
        }
    }

    // Se deplace en bas si la voie n'est pas bloque
    private void MoveDown()
    {
        if (position == 1)
        {
            Hurt();
        }
        else
        {
            if (!game.WayIsBlocked(position - 1))
            {
                position -= 1;
            }
        }
    }
    // Saute pour un temps defini
    private void Jump()
    {
        jumpCount = jumpTime;
        jumping = true;
    }

    // Tue le joueur
    private void Die()
    {
        dead = true;
    }

    public bool IsDead()
    {
        return dead;
    }

    // Blesse le joueur ( peut encore survivre s'il se debrouille bien )
    private void Hurt()
    {
        if (!hurt)
        {
            hurtCounter = 30;
            hurt = true;
        }
        else
        {
            Die();
        }
    }
}
