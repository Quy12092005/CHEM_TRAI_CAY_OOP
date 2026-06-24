using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private Text comboText;

    [Header("Combo Settings")]
    [SerializeField] private float comboTimeLimit = 0.06f;
    [SerializeField] private int bonusEveryCombo = 3;
    [SerializeField] private int bonusPoints = 5;

    private int currentCombo = 0;
    private float lastSliceTime = 0f;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        ResetCombo();
    }

    private void Update()
    {
        if (currentCombo > 0)
        {
            float timeSinceLastSlice = Time.unscaledTime - lastSliceTime;

            if (timeSinceLastSlice > comboTimeLimit)
            {
                ResetCombo();
            }
        }
    }

    public void RegisterFruitSlice()
    {
        float timeSinceLastSlice = Time.unscaledTime - lastSliceTime;

        if (timeSinceLastSlice <= comboTimeLimit)
        {
            currentCombo++;
        }
        else
        {
            currentCombo = 1;
        }

        lastSliceTime = Time.unscaledTime;

        if (currentCombo >= 2)
        {
            UpdateComboText("Combo x" + currentCombo);
        }
        else
        {
            UpdateComboText("");
        }

        if (currentCombo > 0 && currentCombo % bonusEveryCombo == 0)
        {
            GameManager.Instance.IncreaseScore(bonusPoints);
            UpdateComboText("Combo x" + currentCombo + "  +" + bonusPoints);
            Debug.Log("Combo x" + currentCombo + " thuong " + bonusPoints + " diem!");
        }
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        UpdateComboText("");
    }

    private void UpdateComboText(string text)
    {
        if (comboText != null)
        {
            comboText.text = text;
        }
    }
}