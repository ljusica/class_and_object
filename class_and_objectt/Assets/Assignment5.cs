using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assignment5 : ProcessingLite.GP21
{
    int numberOfBalls = 10;
    Ball[] balls;
    Player player = new Player(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        balls = new Ball[numberOfBalls];

        player.playerPosition.x = Width / 2;
        player.playerPosition.y = Height / 2;

        for (int i = 0; i < balls.Length; i++)
        {
            balls[i] = new Ball(0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Background(0);
  
        player.Draw();
        player.Move();

        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].UpdatePos();
            balls[i].DrawBall();
            
            CircleCollision(player.playerPosition.x, player.playerPosition.y, player.diameter, 
                balls[i].position.x, balls[i].position.y, balls[i].ballDiameter);

            if (true)
            {
                Square(5, 5, 5);
            }
        }
    }
    bool CircleCollision(float x1, float y1, float size1, float x2, float y2, float size2)
    {
        float maxDistance = size1 + size2;

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

}

public class Ball : ProcessingLite.GP21
{
    //Our class variables
    public Vector2 position; //Ball position
    Vector2 velocity; //Ball direction
    public float ballDiameter = 0.5f;

    //Ball Constructor, called when we type new Ball(x, y);
    public Ball(float x, float y)
    {
        //Set our position when we create the code.
        position = new Vector2(x, y);

        //Create the velocity vector and give it a random direction.
        velocity = new Vector2();
        velocity.x = Random.Range(0, 11) - 5;
        velocity.y = Random.Range(0, 11) - 5;
    }

    //Draw our ball
    public void DrawBall()
    {
        Fill(Random.Range(0, 255), 44, 12);
        Circle(position.x, position.y, ballDiameter);
    }

    //Update our ball
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
    float maxSpeed = 15;

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