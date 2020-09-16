using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Helpers;

public class Ball : MonoBehaviour
{
    private static readonly System.Random random = new System.Random();
    private static readonly double MaxAngleOfBounce = DegreesToRadians(75);

    public AudioSource audioClip;

    private SpeedCalculator speedCalculator = new SpeedCalculator();

    [SerializeField]
    public float speed = SpeedCalculator.StartingSpeed;


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

    // Start is called before the first frame update
    void Start()
    {
        var randDirection = new int[] { -1, 1 }[random.Next(2)];
        direction = GetRandomAngleInDirection(randDirection);
        radius = transform.localScale.x / 2;
    }

    private Vector2 GetRandomAngleInDirection(int direction)
    {
        var randomFloat = (float)(random.NextDouble() * 2 - 1);
        return new Vector2(direction, randomFloat).normalized;
    }

    // Update is called once per frame
    public void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        var clip = Resources.Load<AudioClip>("Sounds/force-field-impact");

        if (transform.position.y < GameManager.bottomLeft.y + radius && direction.y < 0)
        {
            audioClip.PlayOneShot(clip);
            direction.y = -direction.y;
        }
        if (transform.position.y > GameManager.topRight.y - radius && direction.y > 0)
        {
            audioClip.PlayOneShot(clip);
            direction.y = -direction.y;
        }

        // Game over
        if (transform.position.x < GameManager.bottomLeft.x + radius && direction.x < 0)
        {
            Debug.Log("Right player wins");

            clip = Resources.Load<AudioClip>("Sounds/game-over");
            audioClip.PlayOneShot(clip);
            Time.timeScale = 0;
            enabled = false;
        }
        if (transform.position.x > GameManager.topRight.x - radius && direction.x > 0)
        {
            Debug.Log("Left player wins");

            clip = Resources.Load<AudioClip>("Sounds/game-over-arcade");
            audioClip.PlayOneShot(clip);
            Time.timeScale = 0;
            enabled = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Paddle")
        {
            var paddle = other.GetComponent<Paddle>();
            audioClip.Play();

            var diffInYValues = other.transform.position.y - transform.position.y;
            var maxDifference = radius + paddle.Height;
            var percentOfCenter = diffInYValues / maxDifference;
            var bounceAngle = percentOfCenter * MaxAngleOfBounce;
            direction = new Vector2(paddle.isRight ? -1 : 1, (float)-Math.Sin(bounceAngle));
            direction.Normalize();
        }
    }

    public void UpdateSpeed(double numSeconds)
    {
        speed = speedCalculator.CalculateSpeed(numSeconds);
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
