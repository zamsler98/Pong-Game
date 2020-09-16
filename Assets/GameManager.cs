﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameType GameType { get; private set; }

    public Ball ball;
    public Paddle paddle;

    public static Vector2 bottomLeft;
    public static Vector2 topRight;

    private Ball gameBall;
    private Paddle leftPaddle;
    private Paddle rightPaddle;

    private float startRoundTime;

    private int leftScore = 0;
    private int rightScore = 0;

    public static void StartGame(GameType gameType)
    {
        GameType = gameType;
        SceneManager.LoadScene("Game");
    }

    // Start is called before the first frame update
    void Start()
    {
        startRoundTime = Time.time;
        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));


        gameBall = Instantiate(ball) as Ball;
        gameBall.GameManager = this;
        leftPaddle = Instantiate(paddle) as Paddle;
        leftPaddle.Init(false, new PlayerControl("PaddleLeft"));
        rightPaddle = Instantiate(paddle) as Paddle;
        IPaddleControls controls = null;
        switch (GameManager.GameType)
        {
            case GameType.MULTIPLAYER:
                controls = new PlayerControl("PaddleRight");
                break;
            case GameType.EASY:
                controls = new EasyAI(gameBall, rightPaddle);
                rightPaddle.speed = 4;
                break;
            case GameType.MEDIUM:
                controls = new MediumAI(gameBall, rightPaddle);
                rightPaddle.speed = 5;
                break;
            case GameType.HARD:
                controls = new HardAI(gameBall, rightPaddle);
                rightPaddle.speed = 7;
                break;
        }
        rightPaddle.Init(true, controls);
    }

    // Update is called once per frame
    void Update()
    {
        var timeDiff = Time.time - startRoundTime;
        gameBall.UpdateSpeed(timeDiff);
    }

    /// <summary>
    /// Scores a point for
    /// </summary>
    /// <param name="isRight"></param>
    public void Point(bool isRight)
    {
        Destroy(gameBall.gameObject);

        if (isRight)
        {
            leftScore++;
        }
        else
        {
            rightScore++;
        }
        print($"Left score: {leftScore}");
        print($"Right score: {rightScore}");
        gameBall = Instantiate(ball) as Ball;
        gameBall.GameManager = this;

        if (rightPaddle.controls is AI)
        {
            ((AI)rightPaddle.controls).Ball = gameBall;
        }

        startRoundTime = Time.time;
    }
}

public enum GameType
{
    EASY,
    MEDIUM,
    HARD,
    MULTIPLAYER
}
