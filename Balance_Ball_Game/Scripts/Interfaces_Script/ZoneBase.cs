using UnityEngine;

public abstract class ZoneBase : MonoBehaviour
{
    public abstract bool AddCircle(GameObject circle);
    public abstract void Clear();
    public abstract Color? GetColor();
    public abstract Vector2[] GetCirclePositions();
}