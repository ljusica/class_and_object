using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assignment5 : ProcessingLite.GP21
{
    public GameObject text;

    BallManager ballManager;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = new Player(0, 0);
        ballManager = new BallManager(text);
        player.playerPosition.x = Width / 2;
        player.playerPosition.y = Height / 2;
        ballManager.Initialize();
        ballManager.AddBalls();
        InvokeRepeating("ballManager.AddBalls", 1.0f, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Background(0);

        player.Draw();
        player.Move();
        ballManager.CheckCollision(player);
    }

}

public class Ball : ProcessingLite.GP21
{
    public Vector2 position; //Ball position
    Vector2 velocity; //Ball direction
    public float ballDiameter = 0.5f;

    public Ball(float x, float y)
    {
        position = new Vector2(x, y);

        velocity = new Vector2();
        velocity.x = Random.Range(0, 11) - 5;
        velocity.y = Random.Range(0, 11) - 5;
    }
    public void DrawBall()
    {
        Fill(Random.Range(0, 255), 44, 12);
        Circle(position.x, position.y, ballDiameter);
    }
    public void UpdatePos()
    {
        position += velocity * Time.deltaTime;

        if (position.x > Width || position.x < 0)
        {
            velocity.x *= -1;
        }

        if (position.y > Height || position.y < 0)
        {
            velocity.y *= -1;
        }
    }
}

public class Player : ProcessingLite.GP21
{
    public Vector2 playerPosition;
    public float diameter = 1;
    float speed = 3.5f;
    float acceleration = 2;
    float maxSpeed = 10;

    public Player(float x, float y)
    {
        playerPosition = new Vector2(x, y);
    }

    public void Draw()
    {
        Fill(36, 168, 42);
        Circle(playerPosition.x, playerPosition.y, diameter);
    }

    public void Move()
    {
        Vector2 moveCorrect = new Vector2(Input.GetAxis("Horizontal"), (Input.GetAxis("Vertical")));
        moveCorrect.Normalize();

        if (moveCorrect.magnitude != 0)

            playerPosition.x = (playerPosition.x + Width) % Width;
        playerPosition.y = (playerPosition.y + Height) % Height;

        {
            if (speed <= maxSpeed)
            {
                speed += speed + acceleration;
                playerPosition = playerPosition + speed * moveCorrect * Time.deltaTime;
            }

            else
            {
                playerPosition = playerPosition + speed * moveCorrect * Time.deltaTime;

                if (moveCorrect.magnitude == 0 && speed != 0)
                {
                    speed--;
                }
            }
        }
    }
}


public class BallManager : ProcessingLite.GP21
{
    GameObject text;

    int numberOfStartingBalls = 10;
    Ball[] startingBalls;

    int numberOfAddedBalls = 3;
    Ball[] addedBalls;

    public BallManager(GameObject text)
    {
        this.text = text;
    }

    public void Initialize()
    {
        startingBalls = new Ball[numberOfStartingBalls];

        for (int i = 0; i < startingBalls.Length; i++)
        {
            startingBalls[i] = new Ball(1, 1);
        }
    }

    public void AddBalls()
    {

        if (addedBalls.Length < 100)
        {
            addedBalls = new Ball[numberOfAddedBalls];

            for (int i = 0; i < addedBalls.Length; i++)
            {
                addedBalls[i] = new Ball(1, 1);
            }
        }
    }

    bool CircleCollision(float x1, float y1, float size1, float x2, float y2, float size2)
    {
        float maxDistance = size1 / 2 + size2 / 2;

        if (Mathf.Abs(x1 - x2) > maxDistance || Mathf.Abs(y1 - y2) > maxDistance)
        {
            return false;
        }

        else if (Vector2.Distance(new Vector2(x1, y1), new Vector2(x2, y2)) > maxDistance)
        {
            return false;
        }

        else
        {
            return true;
        }
        
    }

    public void CheckCollision(Player player)
    {
        for (int i = 0; i < startingBalls.Length; i++)
        {
            startingBalls[i].UpdatePos();
            startingBalls[i].DrawBall();

            bool collision = CircleCollision(player.playerPosition.x, player.playerPosition.y, player.diameter,
                startingBalls[i].position.x, startingBalls[i].position.y, startingBalls[i].ballDiameter);

            if (collision)
            {
                text.SetActive(true);
                Time.timeScale = 0 * Time.fixedDeltaTime;
            }
        }

        for (int i = 0; i < addedBalls.Length; i++)
        {
            addedBalls[i].UpdatePos();
            addedBalls[i].DrawBall();

            bool collision = CircleCollision(player.playerPosition.x, player.playerPosition.y, player.diameter,
                addedBalls[i].position.x, addedBalls[i].position.y, addedBalls[i].ballDiameter);

            if (collision)
            {
                text.SetActive(true);
                Time.timeScale = 0 * Time.fixedDeltaTime;
            }
        }

    }

}
