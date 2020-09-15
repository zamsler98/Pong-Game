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

public class EasyAI : IPaddleControls
{
    Ball ball;
    Paddle paddle;
    public EasyAI(Ball ball, Paddle paddle)
    {
        this.ball = ball;
        this.paddle = paddle;
    }

    public float GetDirection()
    {
        if (BallMovingTowards())
        {
            return paddle.Position.y > ball.Position.y ? -1 : 1;
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
            return ball.Position.x >= 0 && ball.Direction.x >= 0;
        }
        return ball.Position.x <= 0 / 2 && ball.Direction.x <= 0;
    }
}

public class MediumAI : IPaddleControls
{
    Ball ball;
    Paddle paddle;
    public MediumAI(Ball ball, Paddle paddle)
    {
        this.ball = ball;
        this.paddle = paddle;
    }

    public float GetDirection()
    {
        return paddle.Position.y > ball.Position.y ? -1 : 1;
    }
}

public class HardAI : IPaddleControls
{
    Ball ball;
    Paddle paddle;
    public HardAI(Ball ball, Paddle paddle)
    {
        this.ball = ball;
        this.paddle = paddle;
    }

    public float GetDirection()
    {
        if (BallMovingTowards())
        {
            return paddle.Position.y >= CalculateTrajectory() ? -1 : 1;
        }
        else
        {
            return paddle.Position.y >= 0 ? -1 : 1;
        }
    }

    private float CalculateTrajectory()
    {
        var fakeBallPosition = new Vector3(ball.Position.x, ball.Position.y, ball.Position.z);
        var fakeBallSpeed = ball.speed;
        var fakeBallRadius = ball.radius;
        var fakeBallDirection = new Vector3(ball.Direction.x, ball.Direction.y, 0);

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
            return ball.Direction.x >= 0;
        }
        return ball.Direction.x <= 0;
    }
}