using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameType GameType { get; private set; }

    public Ball ball;
    public Paddle paddle;
    public AudioSource AudioSource;
    public ScoreBoard ScoreBoard;
    public PowerUp PowerUp;

    public static Vector2 bottomLeft;
    public static Vector2 topRight;

    private Ball gameBall;
    private Paddle leftPaddle;
    private Paddle rightPaddle;

    private GameData GameData;

    private Delay delay = new Delay();

    public float prevTime = 0;

    public static void StartGame(GameType gameType)
    {
        GameType = gameType;
        SceneManager.LoadScene("Game");
    }

    // Start is called before the first frame update
    void Start()
    {
        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        AudioManager.AudioSource = AudioSource;
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
        leftPaddle.CanMove = false;
        rightPaddle.CanMove = false;

        NewGameData();
    }

    public void StartGame()
    {
        prevTime = Time.time;
        gameBall.CanMove = true;
        leftPaddle.CanMove = true;
        rightPaddle.CanMove = true;
        ScoreBoard.StartTimer();
        StartCoroutine(CreatePowerUps());
    }

    IEnumerator CreatePowerUps()
    {
        var random = new System.Random();
        int randNum;
        while (Application.isPlaying)
        {
            if (!delay.IsDelayed)
            {
                randNum = random.Next(5);
                if (randNum == 0)
                {
                    print("Creating powerup");
                    var powerUp = Instantiate(PowerUp);
                    randNum = random.Next(5);
                    Ability ability = null;
                    switch (randNum)
                    {
                        case 0:
                            ability = new SpeedUp(gameBall, this);
                            break;
                        case 1:
                            ability = new SlowDown(gameBall, this);
                            break;
                        case 2:
                            ability = new Grow(leftPaddle, rightPaddle, gameBall);
                            break;
                        case 3:
                            ability = new Shrink(leftPaddle, rightPaddle, gameBall);
                            break;
                        case 4:
                            ability = new ChangeDirection(gameBall);
                            break;
                    }
                    var position = new Vector3((float)random.NextDouble() * 6 - 3, (float)random.NextDouble() * 6 - 3, 0);
                    powerUp.Init(ability, position);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (delay.IsDelayed)
        {
            if (delay.IsDone())
            {
                if (delay.Type == DelayType.NEWROUND)
                {
                    prevTime = Time.time;
                }
                gameBall.CanMove = true;
                leftPaddle.CanMove = true;
                rightPaddle.CanMove = true;
                delay.Stop();
                ScoreBoard.StartTimer();
            }
            else
            {
                return;
            }
        }

        if (prevTime != 0)
        {
            var timeDiff = Time.time - prevTime;
            gameBall.AddToElapsed(timeDiff);
            prevTime = Time.time;
        }
    }

    private void NewGameData()
    {
        GameData = new GameData()
        {
            GameType = GameType,
            Complete = false,
        };
    }

    /// <summary>
    /// Scores a point for
    /// </summary>
    /// <param name="isRight"></param>
    public void Point(bool isRight)
    {
        ScoreBoard.Point(isRight);
        if (ScoreBoard.IsEndGame())
        {
            GameData.Complete = true;
            GameData.NumOfSeconds = (int)ScoreBoard.NumSeconds + ScoreBoard.NumMinutes * 60;
            if (ScoreBoard.LeftScore == 3)
            {
                GameData.Win = true;
            }
            DataAccess.SaveGameData(GameData);
            NewGameData();
            ScoreBoard.EndGame();
        }
        NewRound();
    }

    public void NewRound()
    {
        Destroy(gameBall.gameObject);
        gameBall = Instantiate(ball) as Ball;
        gameBall.GameManager = this;

        if (rightPaddle.controls is AI)
        {
            ((AI)rightPaddle.controls).Ball = gameBall;
        }

        leftPaddle.ResetToMiddle();
        rightPaddle.ResetToMiddle();

        StartDelay(DelayType.NEWROUND);
    }

    public void StartResumeDelay()
    {
        if (!delay.IsDelayed)
        {
            StartDelay(DelayType.RESUME);
        }
    }

    public void StartDelay(DelayType type)
    {
        ScoreBoard.PauseTimer();
        leftPaddle.CanMove = false;
        rightPaddle.CanMove = false;
        gameBall.CanMove = false;
        delay.Start(type);
    }

    public void PaddleHit()
    {
        GameData.NumPaddleHits++;
    }

    public void SaveGame()
    {
        GameData.NumOfSeconds = (int)ScoreBoard.NumSeconds + ScoreBoard.NumMinutes * 60;
        DataAccess.SaveGameData(GameData);
        NewGameData();
    }
}

public class GameData
{
    public GameType GameType { get; set; }
    public bool Complete { get; set; }
    public int NumOfSeconds { get; set; }
    public int NumPaddleHits { get; set; }
    public int NumPowerUps { get; set; }
    public bool Win { get; set; }
}

public enum GameType
{
    EASY,
    MEDIUM,
    HARD,
    MULTIPLAYER
}

public enum DelayType
{
    RESUME,
    NEWROUND,
    NONE
}

public class Delay
{
    private float startOfDelay;
    private float lengthOfDelay;
    public DelayType Type { get; private set; }

    public bool IsDelayed
    {
        get
        {
            return Type != DelayType.NONE;
        }
    }

    public Delay()
    {
        Type = DelayType.NONE;
    }

    public void Start(DelayType type)
    {
        this.Type = type;
        switch (type)
        {
            case DelayType.RESUME:
                lengthOfDelay = .25f;
                break;
            case DelayType.NEWROUND:
                lengthOfDelay = 2;
                break;
            case DelayType.NONE:
                return;
        }
        startOfDelay = Time.time;
    }

    public void Stop()
    {
        Type = DelayType.NONE;
    }

    public bool IsDone()
    {
        if (Type != DelayType.NONE)
        {
            var timeDiff = Time.time - startOfDelay;
            return timeDiff >= lengthOfDelay;
        }
        return false;
    }
}
