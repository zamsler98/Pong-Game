using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private Ability ability;
    Sprite sprite;
    private bool activated = false;

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
        if (!activated)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            activated = true;
            ability.Activate();
            StartCoroutine(RunPowerUp(ability.Length));
        }
    }

    IEnumerator RunPowerUp(float length)
    {
        yield return new WaitForSeconds(length);
        ability.Deactivate();
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public abstract class Ability
{
    public Sprite Sprite { get; protected set; }
    public float Length { get; protected set; }

    public Ability(string spriteName, float length)
    {
        Sprite = Resources.Load<Sprite>("Sprites/" + spriteName);
        Length = length;
    }

    public abstract void Activate();

    public abstract void Deactivate();
}

public class SpeedUp : Ability
{
    public static bool Activated = false;

    Ball ball;
    GameManager gameManager;

    public SpeedUp(Ball ball, GameManager gameManager) : base("speedPower", 2)
    {
        this.ball = ball;
        this.gameManager = gameManager;
    }

    public override void Activate()
    {
        if (!Activated)
        {
            ball.SpeedUp();
            gameManager.prevTime = 0;
            Activated = true;
        }
    }

    public override void Deactivate()
    {
        ball.StopPowerUp();
        Activated = false;
        gameManager.prevTime = Time.time;
    }
}

public class SlowDown : Ability
{
    public static bool Activated;

    Ball ball;
    GameManager gameManager;
    public SlowDown(Ball ball, GameManager gameManager) : base("slowPower", 2)
    {
        this.ball = ball;
        this.gameManager = gameManager;
    }

    public override void Activate()
    {
        if (!Activated)
        {
            ball.SlowDown();
            gameManager.prevTime = 0;
        }
    }

    public override void Deactivate()
    {
        ball.StopPowerUp();
        Activated = false;
        gameManager.prevTime = Time.time;
    }
}

public class Grow : Ability
{
    public static bool Activated;

    Paddle leftPaddle;
    Paddle rightPaddle;
    Ball ball;

    private Paddle changedPaddle;
    public Grow(Paddle leftPaddle, Paddle rightPaddle, Ball ball) : base("growPower", 4)
    {
        this.leftPaddle = leftPaddle;
        this.rightPaddle = rightPaddle;
        this.ball = ball;
    }

    public override void Activate()
    {
        if (!Activated)
        {
            if (ball.Direction.x < 0)
            {
                rightPaddle.Grow();
                changedPaddle = rightPaddle;
            }
            else
            {
                leftPaddle.Grow();
                changedPaddle = leftPaddle;
            }
        }
    }

    public override void Deactivate()
    {
        Activated = false;
        changedPaddle.NormalSize();
    }
}

public class Shrink : Ability
{
    public static bool Activated;

    Paddle leftPaddle;
    Paddle rightPaddle;
    Ball ball;

    private Paddle changedPaddle;

    public Shrink(Paddle leftPaddle, Paddle rightPaddle, Ball ball) : base("shrinkPower", 4)
    {
        this.leftPaddle = leftPaddle;
        this.rightPaddle = rightPaddle;
        this.ball = ball;
    }

    public override void Activate()
    {
        if (!Activated)
        {
            if (ball.Direction.x >= 0)
            {
                rightPaddle.Shrink();
                changedPaddle = rightPaddle;
            }
            else
            {
                leftPaddle.Shrink();
                changedPaddle = leftPaddle;
            }
        }
    }

    public override void Deactivate()
    {
        Activated = false;
        changedPaddle.NormalSize();
    }
}

public class ChangeDirection : Ability
{
    private static bool Activated;

    Ball ball;
    public ChangeDirection(Ball ball) : base("reversePower", 0)
    {
        this.ball = ball;
    }

    public override void Activate()
    {
        if (!Activated)
        {
            ball.SetRandomDirection();
        }
    }

    public override void Deactivate()
    {
        Activated = false;
    }
}
