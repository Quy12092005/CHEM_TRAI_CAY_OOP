using UnityEngine;

public class Bomb : GameItem, ISliceable
{
    public override void OnSlice(Blade blade)
    {
        ExplodeBomb();
    }

    protected virtual void ExplodeBomb()
    {
        GetComponent<Collider>().enabled = false;

        if (ComboManager.Instance != null)
        {
            ComboManager.Instance.ResetCombo();
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBomb();
        }

        GameManager.Instance.HitBigBomb();

        Debug.Log("Chem trung bom to.");

        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Blade blade = other.GetComponent<Blade>();

            if (blade != null)
            {
                OnSlice(blade);
            }
        }
    }
}