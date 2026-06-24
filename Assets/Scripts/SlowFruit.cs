using System.Collections;
using UnityEngine;

public class SlowFruit : Fruit
{
    [Header("Slow Fruit Settings")]
    [SerializeField] private int slowFruitPoints = 2;
    [SerializeField] private float slowTimeScale = 0.5f;
    [SerializeField] private float slowDuration = 3f;

    protected override void Awake()
    {
        base.Awake();
        points = slowFruitPoints;
    }

    public override void OnSlice(Blade blade)
    {
        Debug.Log("Chem trung SlowFruit! Game bi lam cham trong " + slowDuration + " giay.");

        base.OnSlice(blade);

        StartCoroutine(SlowTime());
    }

    private IEnumerator SlowTime()
    {
        Time.timeScale = slowTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(slowDuration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}