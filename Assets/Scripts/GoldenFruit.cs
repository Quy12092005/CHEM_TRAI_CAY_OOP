using UnityEngine;

public class GoldenFruit : Fruit
{
    [Header("Golden Fruit Settings")]
    [SerializeField] private int goldenPoints = 5;

    protected override void Awake()
    {
        base.Awake();
        points = goldenPoints;
    }

    public override void OnSlice(Blade blade)
    {
        Debug.Log("Chem trung trai cay vang! Cong " + points + " diem.");
        base.OnSlice(blade);
    }
}