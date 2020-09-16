using System;
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

    private float startGameTime;

    public static void StartGame(GameType gameType)
    {
        GameType = gameType;
        SceneManager.LoadScene("Game");
    }

    // Start is called before the first frame update
    void Start()
    {
        startGameTime = Time.time;
        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));


        gameBall = Instantiate(ball) as Ball;
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
                break;
            case GameType.MEDIUM:
                controls = new MediumAI(gameBall, rightPaddle);
                break;
            case GameType.HARD:
                controls = new HardAI(gameBall, rightPaddle);
                break;
        }
        rightPaddle.Init(true, controls);
    }

    // Update is called once per frame
    void Update()
    {
        var timeDiff = Time.time - startGameTime;
        print(timeDiff);
        gameBall.UpdateSpeed(timeDiff);
    }
}

public enum GameType
{
    EASY,
    MEDIUM,
    HARD,
    MULTIPLAYER
}
