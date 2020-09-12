using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ball ball;
    public Paddle paddle;

    public static Vector2 bottomLeft;
    public static Vector2 topRight;

    // Start is called before the first frame update
    void Start()
    {
        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));


        Instantiate(ball);
        var paddle1 = Instantiate(paddle) as Paddle;
        var paddle2 = Instantiate(paddle) as Paddle;
        paddle1.Init(true); //Right paddle
        paddle2.Init(false); //Left paddle
    }

    // Update is called once per frame
    void Update()
    {

    }
}
