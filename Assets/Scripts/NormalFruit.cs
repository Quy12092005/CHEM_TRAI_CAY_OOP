using UnityEngine;

public class NormalFruit : Fruit
{
    [Header("Normal Fruit Settings")]
    [SerializeField] private int normalPoints = 1;

    protected override void Awake()
    {
        base.Awake();
        points = normalPoints;
    }

    public override void OnSlice(Blade blade)
    {
        Debug.Log("Chem trung trai cay thuong! Cong " + points + " diem.");
        base.OnSlice(blade);
    }
}