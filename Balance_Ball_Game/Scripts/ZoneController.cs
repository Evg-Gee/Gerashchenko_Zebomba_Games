using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneController : ZoneBase
{
    [SerializeField] private int maxCircles = 3;
    private List<GameObject> circles = new List<GameObject>();
    public event Action OnZoneChanged;
    
    
    void OnCollisionEnter2D(Collision2D collision)
    {        
        if (collision.gameObject.CompareTag("Circle"))
        {
            // Проверяем, что круг полностью вошел в зону
            if (collision.contacts.Any(contact => GetComponent<BoxCollider2D>().bounds.Contains(contact.point)))
            {
                Debug.Log("Circle yo Zone");
                HandleCircleCollision(collision.gameObject);
            }
        }
    }
    private void HandleCircleCollision(GameObject circle)
    {
        if (AddCircle(circle))
        {
            // Останавливаем физику круга
            Rigidbody2D rb = circle.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0;
                rb.isKinematic = true;
            }
        }
        else
        {
            // Уничтожаем круг, если зона заполнена
            Debug.Log(" Destroy(circle);" + gameObject.name);
            Destroy(circle);
        }
    }

    public override bool AddCircle(GameObject circle)
    {
        if (circles.Count >= maxCircles) 
        {
            return false;
        }
        
        circle.transform.SetParent(transform);
        circle.transform.localPosition = Vector2.up * circles.Count * 1.65f;
        circles.Add(circle);
        OnZoneChanged?.Invoke(); // Уведомляем об изменении
        return true;
    }

    public override void Clear()
    {
        foreach (var circle in circles) Destroy(circle);
        circles.Clear();
    }

    public override Color? GetColor()
    {
        if (circles.Count == 0) return null;
        return circles[0].GetComponent<SpriteRenderer>().color;
    }

    public override Vector2[] GetCirclePositions()
    {
        return circles
        .Where(c => c != null)
        .Select(c => (Vector2)c.transform.position)
        .ToArray();
    }
    public SpriteRenderer GetCircleAtPosition(int index)
    {
        if (index < 0 || index >= circles.Count) return null;
        return circles[index].GetComponent<SpriteRenderer>();
    }

    public void RemoveCircleAt(int index)
    {   
        if (index < 0 || index >= circles.Count) return;
        Destroy(circles[index]);
        circles.RemoveAt(index);
        OnZoneChanged?.Invoke();
    }
}