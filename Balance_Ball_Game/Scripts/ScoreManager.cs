using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScoreManager : IScoreManager
{
    private int score;
    private readonly Dictionary<Color, int> scoreMap = new Dictionary<Color, int>
    {
        { Color.red, 10 },
        { Color.blue, 20 },
        { Color.green, 30 }
    };

    public void AddScore(Color color)
    {
        if (scoreMap.TryGetValue(color, out int value))
        {
            score += value;
        }
    }

    public int GetCurrentScore() => score;
}