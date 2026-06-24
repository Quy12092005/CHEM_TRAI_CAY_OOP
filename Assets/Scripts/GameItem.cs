using UnityEngine;

public abstract class GameItem : MonoBehaviour
{
    [Header("OOP Item Settings")]
    [SerializeField] protected int points = 1;

    public int Points
    {
        get { return points; }
    }

    public abstract void OnSlice(Blade blade);
}