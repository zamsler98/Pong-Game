﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField]
    float speed;
    float height;

    public bool isRight;
    IPaddleControls controls;

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

    public void Init(bool isPaddleRight, IPaddleControls controls)
    {
        this.controls = controls;
        Vector2 pos;
        isRight = isPaddleRight;
        if (isPaddleRight)
        {
            pos = new Vector2(GameManager.topRight.x, 0);
            pos -= Vector2.right * transform.localScale.x;
        }
        else
        {
            pos = new Vector2(GameManager.bottomLeft.x, 0);
            pos += Vector2.right * transform.localScale.x;
        }

        //Update this paddle's position
        transform.position = pos;
    }
}
