using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameType GameType { get; private set; }

    public Ball ball;
    public Paddle paddle;

    public static Vector2 bottomLeft;
    public static Vector2 topRight;

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


        Instantiate(ball);
        CreatePlayerOne();
        CreatePlayerTwo();
    }

    private void CreatePlayerOne()
    {
        var player = Instantiate(paddle) as Paddle;
        player.Init(false, new PlayerControl("PaddleLeft"));
    }

    private void CreatePlayerTwo()
    {
        var player = Instantiate(paddle) as Paddle;
        player.Init(true, new PlayerControl("PaddleRight"));
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public enum GameType
{
    EASY,
    MEDIUM,
    HARD,
    MULTIPLAYER
}
