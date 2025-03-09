using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComboChecker : MonoBehaviour, IComboChecker
{
    [SerializeField] private ZoneController[] zones;
    [SerializeField] private ParticleSystem comboParticles;

    public event Action<Color> OnComboDetected = delegate { };
    private void OnEnable()
    {
        // Подписываемся на события всех зон
        foreach (var zone in zones)
        {
            zone.OnZoneChanged += CheckCombos;
        }
    }

    private void OnDisable()
    {
        // Отписываемся для избежания утечек памяти
        foreach (var zone in zones)
        {
            zone.OnZoneChanged -= CheckCombos;
        }
    }
    public void CheckCombos()
    {
        CheckHorizontalCombos();
        CheckVerticalCombos();
        CheckDiagonalCombos();
    }

    private void CheckHorizontalCombos()
    {
    Debug.Log("CheckHorizontalCombos()");

    foreach (var zone in zones)
    {
        // Проверяем, что в зоне ровно 3 круга и все одного цвета
        if (zone.GetCirclePositions().Length == 3 && zone.GetColor().HasValue)
        {
            Color comboColor = zone.GetColor().Value;
            
            // Спавним партиклы в позициях кругов
            SpawnParticles(zone.GetCirclePositions());
            
            // Очищаем зону
            zone.Clear();
            
            // Уведомляем о комбо
            OnComboDetected?.Invoke(comboColor);
        }
    }
    }
    private void CheckDiagonalCombos()
    {
        if (zones.Length != 3) return; // Только для 3 зон

        // Диагональ слева-направо (↓)
        var diagonal1 = new List<Vector2>();
        for (int i = 0; i < 3; i++)
        {
            var zone = zones[i];
            if (zone.GetCirclePositions().Length > i) // Проверка границ
            {
                Vector2 pos = zone.GetCirclePositions()[i];
                diagonal1.Add(pos);
            }
        }

        if (diagonal1.Count == 3 && CheckSameColor(diagonal1))
        {
            SpawnParticles(diagonal1.ToArray());
            for (int i = 0; i < 3; i++)
            {
                if (zones[i].GetCirclePositions().Length > i)
                    zones[i].RemoveCircleAt(i);
            }
            OnComboDetected?.Invoke(GetColor(diagonal1[0]));
        }

        // Диагональ справа-налево (↓)
        var diagonal2 = new List<Vector2>();
        for (int i = 0; i < 3; i++)
        {
            int zoneIndex = 2 - i;
            var zone = zones[zoneIndex];
            if (zone.GetCirclePositions().Length > i) // Проверка границ
            {
                Vector2 pos = zone.GetCirclePositions()[i];
                diagonal2.Add(pos);
            }
        }

        if (diagonal2.Count == 3 && CheckSameColor(diagonal2))
        {
            SpawnParticles(diagonal2.ToArray());
            for (int i = 0; i < 3; i++)
            {
                int zoneIndex = 2 - i;
                if (zones[zoneIndex].GetCirclePositions().Length > i)
                    zones[zoneIndex].RemoveCircleAt(i);
            }
            OnComboDetected?.Invoke(GetColor(diagonal2[0]));
        }
    }
    
    // private void CheckDiagonalCombos()
    // {
    //     if (zones.Length < 3) return;

    //     // Диагональ слева-направо (↓)
    //     var diagonal1 = new List<Vector2>();
    //     for (int i = 0; i < 3; i++)
    //     {
    //         if (i < zones[i].GetCirclePositions().Length)
    //         {
    //             Vector2 pos = zones[i].GetCirclePositions()[i]; // Vector2
    //             diagonal1.Add(pos);
    //         }
    //     }

    //     if (diagonal1.Count == 3 && CheckSameColor(diagonal1))
    //     {
    //         SpawnParticles(diagonal1.ToArray());
    //         ClearDiagonal(zones, 0, 1, 2); // Удаляем круги
    //         OnComboDetected?.Invoke(GetColor(diagonal1[0]));
    //     }

    //     // Диагональ справа-налево (↓)
    //     var diagonal2 = new List<Vector2>();
    //     for (int i = 0; i < 3; i++)
    //     {
    //         int zoneIndex = 2 - i;
    //         if (zoneIndex < zones[zoneIndex].GetCirclePositions().Length)
    //         {
    //             Vector2 pos = zones[zoneIndex].GetCirclePositions()[i];
    //             diagonal2.Add(pos);
    //         }
    //     }

    //     if (diagonal2.Count == 3 && CheckSameColor(diagonal2))
    //     {
    //         SpawnParticles(diagonal2.ToArray());
    //         ClearDiagonal(zones, 2, 1, 0); // Удаляем круги
    //         OnComboDetected?.Invoke(GetColor(diagonal2[0]));
    //     }
    // }
    private void CheckVerticalCombos()
    {
        if (zones.Length != 3) return;

        for (int column = 0; column < 3; column++)
        {
            var columnPositions = new List<Vector2>();
            foreach (var zone in zones)
            {
                if (zone.GetCirclePositions().Length > column) // Проверка границ
                {
                    Vector2 pos = zone.GetCirclePositions()[column];
                    columnPositions.Add(pos);
                }
            }

            if (columnPositions.Count == 3 && CheckSameColor(columnPositions))
            {
                SpawnParticles(columnPositions.ToArray());
                foreach (var zone in zones)
                {
                    if (zone.GetCirclePositions().Length > column)
                        zone.RemoveCircleAt(column);
                }
                OnComboDetected?.Invoke(GetColor(columnPositions[0]));
            }
        }
    }


    private bool CheckSameColor(List<Vector2> positions)
    {
        if (positions.Count == 0) return false;
        Color firstColor = GetColor(positions[0]);
        return positions.All(pos => GetColor(pos) == firstColor);
    }
    private Color GetColor(Vector2 position)
    {
        foreach (var zone in zones)
        {
            var positions = zone.GetCirclePositions();
            for (int i = 0; i < positions.Length; i++)
            {
                if (positions[i] == position)
                {
                    Color? color = zone.GetColor();
                    return color ?? Color.clear; // Защита от null
                }
            }
        }
        return Color.clear;
    }
    // private Color GetColor(Vector2 position)
    // {
    //     foreach (var zone in zones)
    //     {
    //         foreach (var circle in zone.GetCirclePositions())
    //         {
    //             if (circle == position)
    //             {
    //                 return zone.GetColor().Value;
    //             }
    //         }
    //     }
    //     return Color.clear;
    // }

    private void ClearDiagonal(ZoneController[] zones, int index1, int index2, int index3)
    {
        zones[index1].RemoveCircleAt(index1);
        zones[index2].RemoveCircleAt(index2);
        zones[index3].RemoveCircleAt(index3);
    }
    private void SpawnParticles(Vector2[] positions)
    {
        foreach (var pos in positions)
        {
            Instantiate(comboParticles, pos, Quaternion.identity);
        }
    }


}
