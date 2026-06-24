using UnityEngine;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Spawner spawner;
    [SerializeField] private Text levelText;

    private int currentLevel = 1;

    public void ResetDifficulty()
    {
        currentLevel = 1;
        ApplyDifficulty();
        UpdateLevelText();
    }

    public void UpdateDifficultyByScore(int score)
    {
        int newLevel = GetLevelFromScore(score);

        if (newLevel != currentLevel)
        {
            currentLevel = newLevel;
            ApplyDifficulty();
            UpdateLevelText();

            Debug.Log("Tang do kho! Level hien tai: " + currentLevel);
        }
    }

    private int GetLevelFromScore(int score)
    {
        if (score < 20)
        {
            return 1;
        }
        else if (score < 40)
        {
            return 2;
        }
        else if (score < 70)
        {
            return 3;
        }
        else if (score < 100)
        {
            return 4;
        }
        else
        {
            return 5;
        }
    }

    private void ApplyDifficulty()
    {
        if (spawner == null)
        {
            return;
        }

        switch (currentLevel)
        {
            case 1:
                // 0 - 19 điểm: dễ
                spawner.bombChance = 0.10f;
                spawner.specialFruitChance = 0.15f;
                spawner.minSpawnDelay = 0.60f;
                spawner.maxSpawnDelay = 1.20f;
                spawner.minForce = 16f;
                spawner.maxForce = 20f;
                break;

            case 2:
                // 20 - 39 điểm: trung bình
                spawner.bombChance = 0.18f;
                spawner.specialFruitChance = 0.12f;
                spawner.minSpawnDelay = 0.45f;
                spawner.maxSpawnDelay = 0.95f;
                spawner.minForce = 19f;
                spawner.maxForce = 24f;
                break;

            case 3:
                // 40 - 69 điểm: khó
                spawner.bombChance = 0.28f;
                spawner.specialFruitChance = 0.10f;
                spawner.minSpawnDelay = 0.32f;
                spawner.maxSpawnDelay = 0.75f;
                spawner.minForce = 23f;
                spawner.maxForce = 29f;
                break;

            case 4:
                // 70 - 99 điểm: rất khó
                spawner.bombChance = 0.40f;
                spawner.specialFruitChance = 0.08f;
                spawner.minSpawnDelay = 0.22f;
                spawner.maxSpawnDelay = 0.55f;
                spawner.minForce = 28f;
                spawner.maxForce = 35f;
                break;

            default:
                // 100+ điểm: cực khó
                spawner.bombChance = 0.55f;
                spawner.specialFruitChance = 0.05f;
                spawner.minSpawnDelay = 0.15f;
                spawner.maxSpawnDelay = 0.38f;
                spawner.minForce = 33f;
                spawner.maxForce = 42f;
                break;
        }

        Debug.Log(
            "Level " + currentLevel +
            " | Bomb Chance: " + spawner.bombChance +
            " | Special Chance: " + spawner.specialFruitChance +
            " | Spawn Delay: " + spawner.minSpawnDelay + " - " + spawner.maxSpawnDelay +
            " | Force: " + spawner.minForce + " - " + spawner.maxForce
        );
    }

    private void UpdateLevelText()
    {
        if (levelText == null)
        {
            return;
        }

        if (currentLevel >= 5)
        {
            levelText.text = "Level: 5+";
        }
        else
        {
            levelText.text = "Level: " + currentLevel.ToString();
        }
    }
}