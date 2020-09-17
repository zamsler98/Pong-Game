using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public TextMeshProUGUI Hits;
    public TextMeshProUGUI TimePlayed;
    public TextMeshProUGUI Wins;
    public TextMeshProUGUI Losses;
    public TextMeshProUGUI PowerUps;
    public Button ClearButton;

    public void Clear()
    {
        ClearButton.enabled = false;
        ClearButton.enabled = true;
        DataAccess.Clear();
        Start();
    }

    private void Start()
    {
        Hits.text = DataAccess.GetTotalHits().ToString();
        Wins.text = DataAccess.GetWins().ToString();
        Losses.text = DataAccess.GetLosses().ToString();
        PowerUps.text = DataAccess.GetPowerUps().ToString();

        var totalSeconds = DataAccess.GetTotalSeconds();
        TimePlayed.text = SecondsToString(totalSeconds);
    }

    private string SecondsToString(long totalSeconds)
    {
        var hours = (int)(totalSeconds / 3600);
        var secondsRemaining = totalSeconds % 3600;
        var minutes = (int)(secondsRemaining / 60);
        var seconds = (int)(secondsRemaining % 60);
        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }


}
