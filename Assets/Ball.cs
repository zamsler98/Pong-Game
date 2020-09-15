using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Helpers;

public class Ball : MonoBehaviour
{
    private static readonly double MaxAngleOfBounce = DegreesToRadians(75);


    [SerializeField]
    public float speed;

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
        direction = Vector2.one.normalized;
        radius = transform.localScale.x / 2;
    }

    // Update is called once per frame
    public void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if (transform.position.y < GameManager.bottomLeft.y + radius && direction.y < 0)
        {
            direction.y = -direction.y;
        }
        if (transform.position.y > GameManager.topRight.y - radius && direction.y > 0)
        {
            direction.y = -direction.y;
        }

        // Game over
        if (transform.position.x < GameManager.bottomLeft.x + radius && direction.x < 0)
        {
            Debug.Log("Right player wins");

            Time.timeScale = 0;
            enabled = false;
        }
        if (transform.position.x > GameManager.topRight.x - radius && direction.x > 0)
        {
            Debug.Log("Left player wins");

            Time.timeScale = 0;
            enabled = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Paddle")
        {
            var paddle = other.GetComponent<Paddle>();

            var diffInYValues = other.transform.position.y - transform.position.y;
            var maxDifference = radius + paddle.Height;
            var percentOfCenter = diffInYValues / maxDifference;
            var bounceAngle = percentOfCenter * MaxAngleOfBounce;
            direction = new Vector2(paddle.isRight ? -1 : 1, (float)-Math.Sin(bounceAngle));
            direction.Normalize();
        }
    }
}
