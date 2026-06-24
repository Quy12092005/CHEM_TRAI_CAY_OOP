using UnityEngine;

public class HealFruit : Fruit
{
    [Header("Heal Fruit Settings")]
    [SerializeField] private int healFruitPoints = 2;
    [SerializeField] private int healAmount = 1;

    protected override void Awake()
    {
        base.Awake();
        points = healFruitPoints;
    }

    public override void OnSlice(Blade blade)
    {
        Debug.Log("Chem trung HealFruit! Hoi " + healAmount + " mang.");

        base.OnSlice(blade);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddLife(healAmount);
        }
    }
}