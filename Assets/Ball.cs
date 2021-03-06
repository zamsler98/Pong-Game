﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Helpers;

public class Ball : MonoBehaviour
{
    private static readonly System.Random random = new System.Random();
    private static readonly double MaxAngleOfBounce = DegreesToRadians(75);



    private SpeedCalculator speedCalculator = new SpeedCalculator();
    private double elapsedTime = 0;
    private bool PoweredUp = false;

    [SerializeField]
    public float speed = SpeedCalculator.StartingSpeed;

    public bool CanMove { get; set; } = false;

    public float radius;
    public Vector2 Direction
    {
        get
        {
            return direction;
        }
        set
        {
            direction = value;
        }
    }
    Vector2 direction;
    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }

    public GameManager GameManager { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        SetRandomDirection();
        radius = transform.localScale.x / 2;
    }

    public void SetRandomDirection()
    {
        var randDirection = new int[] { -1, 1 }[random.Next(2)];
        direction = GetRandomAngleInDirection(randDirection);
    }

    private Vector2 GetRandomAngleInDirection(int direction)
    {
        var randomFloat = (float)(random.NextDouble() * 2 - 1);
        return new Vector2(direction, randomFloat).normalized;
    }

    // Update is called once per frame
    public void Update()
    {
        if (CanMove)
        {
            if (!PoweredUp)
            {
                UpdateSpeed();
            }

            transform.Translate(direction * speed * Time.deltaTime);

            if (transform.position.y < GameManager.bottomLeft.y + radius && direction.y < 0)
            {
                AudioManager.PlayWallHit();
                direction.y = -direction.y;
            }
            if (transform.position.y > GameManager.topRight.y - radius && direction.y > 0)
            {
                AudioManager.PlayWallHit();
                direction.y = -direction.y;
            }

            if (transform.position.x < GameManager.bottomLeft.x + radius && direction.x < 0)
            {
                AudioManager.PlayGameOverLeft();
                GameManager.Point(false);

            }
            if (transform.position.x > GameManager.topRight.x - radius && direction.x > 0)
            {
                AudioManager.PlayGameOverRight();
                GameManager.Point(true);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Paddle")
        {
            var paddle = other.GetComponent<Paddle>();

            if (!paddle.isRight)
            {
                GameManager.PaddleHit();
            }

            AudioManager.PlayPaddleHit();

            var diffInYValues = other.transform.position.y - transform.position.y;
            var maxDifference = radius + paddle.Height;
            var percentOfCenter = diffInYValues / maxDifference;
            var bounceAngle = percentOfCenter * MaxAngleOfBounce;
            direction = new Vector2(paddle.isRight ? -1 : 1, (float)-Math.Sin(bounceAngle));
            direction.Normalize();
        }
        else if (other.tag == "PowerUp")
        {
            var powerUp = other.GetComponent<PowerUp>();

            powerUp.Activate();
            print("Collision with powerup");
        }
    }

    public void AddToElapsed(double deltaTime)
    {
        elapsedTime += deltaTime;
    }

    public void UpdateSpeed()
    {
        speed = speedCalculator.CalculateSpeed(elapsedTime);
    }

    public void SpeedUp()
    {
        PoweredUp = true;
        speed = SpeedCalculator.TopSpeed + 5;
    }

    public void SlowDown()
    {
        PoweredUp = true;
        speed = speed / 2;
    }

    public void StopPowerUp()
    {
        PoweredUp = false;
        UpdateSpeed();
    }
}

public class SpeedCalculator
{
    public const int StartingSpeed = 5;
    public const int TopSpeed = 25;
    public const int SecondsToReachTopSpeed = 45;

    private int a;
    private double b;

    public SpeedCalculator()
    {
        var closeToTopSpeed = TopSpeed * .99;
        a = StartingSpeed - TopSpeed;
        b = Math.Pow((closeToTopSpeed - TopSpeed) / a, (1.0 / SecondsToReachTopSpeed));
    }

    public float CalculateSpeed(double numSeconds)
    {
        return a * (float)Math.Pow(b, numSeconds) + TopSpeed;
    }
}
