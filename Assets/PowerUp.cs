using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private Ability ability;
    Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(Ability ability, Vector3 position)
    {
        this.ability = ability;
        GetComponent<SpriteRenderer>().sprite = ability.Sprite;
        transform.position = position;
    }

    public void Activate()
    {
        ability.Activate();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public abstract class Ability
{
    public Sprite Sprite { get; protected set; }

    public Ability(string spriteName)
    {
        Sprite = Resources.Load<Sprite>("Sprites/" + spriteName);
    }

    public abstract void Activate();
}

public class SpeedUp : Ability
{
    Ball ball;
    GameManager gameManager;
    public SpeedUp(Ball ball, GameManager gameManager) : base("speedPower")
    {
        this.ball = ball;
        this.gameManager = gameManager;
    }

    public override void Activate()
    {
        ball.SpeedUp();
        gameManager.prevTime = 0;
    }
}

public class SlowDown : Ability
{
    Ball ball;
    GameManager gameManager;
    public SlowDown(Ball ball, GameManager gameManager) : base("slowPower")
    {
        this.ball = ball;
        this.gameManager = gameManager;
    }

    public override void Activate()
    {
        ball.SlowDown();
        gameManager.prevTime = 0;
    }
}

public class Grow : Ability
{
    Paddle leftPaddle;
    Paddle rightPaddle;
    Ball ball;
    public Grow(Paddle leftPaddle, Paddle rightPaddle, Ball ball) : base("growPower")
    {
        this.leftPaddle = leftPaddle;
        this.rightPaddle = rightPaddle;
        this.ball = ball;
    }

    public override void Activate()
    {
        if (ball.Direction.x < 0)
        {
            rightPaddle.Grow();
        }
        else
        {
            leftPaddle.Grow();
        }
    }
}

public class Shrink : Ability
{
    Paddle leftPaddle;
    Paddle rightPaddle;
    Ball ball;
    public Shrink(Paddle leftPaddle, Paddle rightPaddle, Ball ball) : base("shrinkPower")
    {
        this.leftPaddle = leftPaddle;
        this.rightPaddle = rightPaddle;
        this.ball = ball;
    }

    public override void Activate()
    {
        if (ball.Direction.x >= 0)
        {
            rightPaddle.Shrink();
        }
        else
        {
            leftPaddle.Shrink();
        }
    }
}

public class ChangeDirection : Ability
{
    Ball ball;
    public ChangeDirection(Ball ball) : base("reversePower")
    {
        this.ball = ball;
    }

    public override void Activate()
    {
        ball.SetRandomDirection();
    }
}
