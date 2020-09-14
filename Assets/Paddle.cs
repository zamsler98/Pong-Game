using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField]
    float speed;
    float height;

    string input;
    public bool isRight;

    public float Height
    {
        get
        {
            return height;
        }
        private set
        {
            height = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        height = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        var axis = Input.GetAxis(input);
        if (axis > 0)
        {
            MoveUp();
        }
        else if (axis < 0)
        {
            MoveDown();
        }
    }

    public void MoveDown()
    {
        float move = -1 * Time.deltaTime * speed;

        if (transform.position.y < GameManager.bottomLeft.y + height / 2 && move < 0)
        {
            move = 0;
        }
        transform.Translate(move * Vector2.up);
    }

    public void MoveUp()
    {
        float move = Time.deltaTime * speed;
        if (transform.position.y > GameManager.topRight.y - height / 2 && move > 0)
        {
            move = 0;
        }
        transform.Translate(move * Vector2.up);
    }

    public void Init(bool isPaddleRight)
    {
        Vector2 pos;
        isRight = isPaddleRight;
        if (isPaddleRight)
        {
            pos = new Vector2(GameManager.topRight.x, 0);
            pos -= Vector2.right * transform.localScale.x;
            input = "PaddleRight";
        }
        else
        {
            pos = new Vector2(GameManager.bottomLeft.x, 0);
            pos += Vector2.right * transform.localScale.x;
            input = "PaddleLeft";
        }

        //Update this paddle's position
        transform.position = pos;

        transform.name = input;
    }
}
