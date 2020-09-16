using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public TextMeshProUGUI Timer;
    public TextMeshProUGUI LeftWins;
    public TextMeshProUGUI RightWins;

    public Image Dot1;
    public Image Dot2;
    public Image Dot3;
    public Image Dot4;
    public Image Dot5;
    public Image Dot6;
    public Image[] Dots;

    public int LeftScore { get; private set; } = 0;
    public int RightScore { get; private set; } = 0;
    public int NumberLeftWins { get; private set; } = 0;
    public int NumberRightWins { get; private set; } = 0;

    public float NumSeconds { get; private set; } = 0;
    public int NumMinutes { get; private set; } = 0;
    public float StartTime { get; private set; }



    private void Start()
    {
        Dots = new Image[] { Dot1, Dot2, Dot3, Dot4, Dot5, Dot6 };
    }



    public void UpdateScoreDots()
    {
        for (var i = 0; i < LeftScore; i++)
        {
            MarkDot(i);
        }
        for (var i = LeftScore; i < 3; i++)
        {
            UnMarkDot(i);
        }
        for (var i = 0; i < RightScore; i++)
        {
            MarkDot(i + 3);
        }
        for (var i = RightScore; i < 3; i++)
        {
            UnMarkDot(i + 3);
        }
    }

    public void UpdateWins()
    {
        LeftWins.text = NumberLeftWins.ToString();
        RightWins.text = NumberRightWins.ToString();
    }

    public void MarkDot(int i)
    {
        Dots[i].sprite = Resources.Load<Sprite>("Sprites/fullCircle");
    }

    public void UnMarkDot(int i)
    {
        Dots[i].sprite = Resources.Load<Sprite>("Sprites/emptyCircle");
    }

    public void Point(bool isRight)
    {
        if (isRight)
        {
            LeftScore++;
        }
        else
        {
            RightScore++;
        }
        UpdateScoreDots();
    }

    public bool IsEndGame()
    {
        if (LeftScore == 3 || RightScore == 3)
        {
            if (LeftScore == 3)
            {
                NumberLeftWins++;
            }
            else
            {
                NumberRightWins++;
            }
            UpdateWins();
            LeftScore = 0;
            RightScore = 0;
            UpdateScoreDots();
            return true;
        }
        return false;
    }

    public void SetTimer()
    {
        if (NumSeconds >= 60)
        {
            NumSeconds = 0;
            NumMinutes++;
        }
        var seconds = (int)NumSeconds;
        Timer.text = $"{NumMinutes:D2}:{seconds:D2}";
    }

    public void StartTimer()
    {
        StartTime = Time.time;
    }

    public void PauseTimer()
    {
        StartTime = 0;
    }

    private void Update()
    {
        print(NumSeconds);
        if (StartTime != 0)
        {
            NumSeconds += Time.time - StartTime;
            StartTime = Time.time;
            SetTimer();
        }
    }
}
