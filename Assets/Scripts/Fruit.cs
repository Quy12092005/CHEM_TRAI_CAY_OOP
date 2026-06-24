using UnityEngine;

public abstract class Fruit : GameItem, ISliceable
{
    public GameObject whole;
    public GameObject sliced;

    protected Rigidbody fruitRigidbody;
    protected Collider fruitCollider;
    protected ParticleSystem juiceEffect;

    protected virtual void Awake()
    {
        fruitRigidbody = GetComponent<Rigidbody>();
        fruitCollider = GetComponent<Collider>();
        juiceEffect = GetComponentInChildren<ParticleSystem>();
    }

    public override void OnSlice(Blade blade)
    {
        Slice(blade.direction, blade.transform.position, blade.sliceForce);
    }

    protected virtual void Slice(Vector3 direction, Vector3 position, float force)
    {
        GameManager.Instance.IncreaseScore(points);
        if (ComboManager.Instance != null)
        {
            ComboManager.Instance.RegisterFruitSlice();
        }
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayFruitSlice();
        }

        fruitCollider.enabled = false;
        whole.SetActive(false);

        sliced.SetActive(true);
        juiceEffect.Play();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody slice in slices)
        {
            slice.velocity = fruitRigidbody.velocity;
            slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
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