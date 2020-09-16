using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float speed = 10;
    float height;

    public bool isRight;
    public IPaddleControls controls;
    public bool CanMove { get; set; } = true;

    Sprite sprite;

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

    public Vector2 Position
    {
        get
        {
            return transform.position;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            var axis = controls.GetDirection();
            float move = axis * Time.deltaTime * speed;

            if (move < 0 && transform.position.y < GameManager.bottomLeft.y + height / 2)
            {
                move = 0;
            }
            if (move > 0 && transform.position.y > GameManager.topRight.y - height / 2)
            {
                move = 0;
            }
            transform.Translate(move * Vector2.up);
        }
    }

    public void Init(bool isPaddleRight, IPaddleControls controls)
    {
        this.controls = controls;
        Vector2 pos;
        isRight = isPaddleRight;

        if (isPaddleRight)
        {
            pos = new Vector2(GameManager.topRight.x, 0);
            pos -= Vector2.right * transform.localScale.x;
            sprite = this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/alienShip");
        }
        else
        {
            pos = new Vector2(GameManager.bottomLeft.x, 0);
            pos += Vector2.right * transform.localScale.x;
            sprite = this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/spaceShip");
        }
        var collider = this.GetComponent<BoxCollider2D>();
        collider.size = sprite.bounds.size;
        collider.offset = sprite.bounds.center;
        height = collider.size.y;

        //Update this paddle's position
        transform.position = pos;
    }

    public void ResetToMiddle()
    {
        if (isRight)
        {
            transform.position = new Vector2(GameManager.topRight.x, 0);
            transform.position -= Vector3.right * transform.localScale.x;
        }
        else
        {
            transform.position = new Vector2(GameManager.bottomLeft.x, 0);
            transform.position += Vector3.right * transform.localScale.x;
        }
    }
}
