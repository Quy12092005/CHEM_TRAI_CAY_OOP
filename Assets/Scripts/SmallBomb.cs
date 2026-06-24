using UnityEngine;

public class SmallBomb : Bomb
{
    [Header("Small Bomb Settings")]
    [SerializeField] private int minusPoints = 3;

    protected override void ExplodeBomb()
    {
        GetComponent<Collider>().enabled = false;

        if (ComboManager.Instance != null)
        {
            ComboManager.Instance.ResetCombo();
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySmallBomb();
        }

        GameManager.Instance.HitSmallBomb(minusPoints);

        Debug.Log("Chem trung bom nho.");

        Destroy(gameObject);
    }
}