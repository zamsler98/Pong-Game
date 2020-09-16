using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPaddleControls
{
    /// <summary>
    /// Gets the direction that the paddle should move
    /// </summary>
    /// <returns>1 if paddle moves up else -1</returns>
    float GetDirection();
}

public class PlayerControl : IPaddleControls
{
    string input;
    public PlayerControl(string inputString)
    {
        input = inputString;
    }

    public float GetDirection()
    {
        return Input.GetAxis(input);
    }
}

public abstract class AI : IPaddleControls
{
    public Ball Ball { get; set; }
    protected Paddle paddle;
    public AI(Ball ball, Paddle paddle)
    {
        this.paddle = paddle;
        this.Ball = ball;
    }

    public abstract float GetDirection();

    /// <summary>
    /// Returns the direction to move the paddle towards the given y value
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    protected float MoveTowardsPoint(float y)
    {
        if (Mathf.Abs(y - paddle.Position.y) <= paddle.Height / 20)
        {
            return 0;
        }
        return paddle.Position.y >= y ? -1 : 1;
    }
}

public class EasyAI : AI
{

    public EasyAI(Ball ball, Paddle paddle) : base(ball, paddle) { }

    public override float GetDirection()
    {
        if (BallMovingTowards())
        {
            return MoveTowardsPoint(Ball.Position.y);
        }
        return 0;
    }

    /// <summary>
    /// Checks whether the ball is past halfway and moving towards the paddle
    /// </summary>
    /// <returns></returns>
    private bool BallMovingTowards()
    {
        if (paddle.isRight)
        {
            return Ball.Position.x >= 0 && Ball.Direction.x >= 0;
        }
        return Ball.Position.x <= 0 / 2 && Ball.Direction.x <= 0;
    }
}

public class MediumAI : AI
{
    public MediumAI(Ball ball, Paddle paddle) : base(ball, paddle) { }

    public override float GetDirection()
    {
        return MoveTowardsPoint(Ball.Position.y);
    }
}

public class HardAI : AI
{

    public HardAI(Ball ball, Paddle paddle) : base(ball, paddle) { }


    public override float GetDirection()
    {
        if (BallMovingTowards())
        {
            return MoveTowardsPoint(CalculateTrajectory());
        }
        else
        {
            return MoveTowardsPoint(0);
        }
    }

    private float CalculateTrajectory()
    {
        var fakeBallPosition = new Vector3(Ball.Position.x, Ball.Position.y, Ball.Position.z);
        var fakeBallSpeed = Ball.speed;
        var fakeBallRadius = Ball.radius;
        var fakeBallDirection = new Vector3(Ball.Direction.x, Ball.Direction.y, 0);

        //Only works for right paddle
        while (fakeBallPosition.x <= paddle.Position.x)
        {
            fakeBallPosition += fakeBallDirection * fakeBallSpeed * .01f;
            if (fakeBallPosition.y < GameManager.bottomLeft.y + fakeBallRadius && fakeBallDirection.y < 0)
            {
                fakeBallDirection.y = -fakeBallDirection.y;
            }
            if (fakeBallPosition.y > GameManager.topRight.y - fakeBallRadius && fakeBallDirection.y > 0)
            {
                fakeBallDirection.y = -fakeBallDirection.y;
            }
        }
        return fakeBallPosition.y;
    }

    /// <summary>
    /// Checks whether the ball is moving towards the paddle
    /// </summary>
    /// <returns></returns>
    private bool BallMovingTowards()
    {
        if (paddle.isRight)
        {
            return Ball.Direction.x >= 0;
        }
        return Ball.Direction.x <= 0;
    }
}