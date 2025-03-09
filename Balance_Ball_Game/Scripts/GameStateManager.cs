using System;
using System.Linq;
using UnityEngine;

public class GameStateManager : MonoBehaviour, IGameState
{
    [SerializeField] private ZoneController[] zones;
    public event Action OnGameOver = delegate { };
    
    private void Update()
    {
        if (IsGameOver())
        {
            OnGameOver?.Invoke();
            enabled = false;
        }
    }

    public bool IsGameOver()
    {
        if (zones == null || zones.Length != 3)
        {
            Debug.LogError("Game zones are not initialized correctly!");
            return false;
        }

        foreach (var zone in zones)
        {
            if (zone.GetCirclePositions().Length != 3)
                return false;
        }
        return true;
    }    
}