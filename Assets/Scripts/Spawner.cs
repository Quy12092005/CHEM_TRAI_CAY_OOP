using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Fruit Settings")]
    public GameObject[] normalFruitPrefabs;
    public GameObject[] specialFruitPrefabs;

    [Range(0f, 1f)]
    public float specialFruitChance = 0.12f;

    [Header("Bomb Settings")]
    public GameObject[] bombPrefabs;

    [Range(0f, 1f)]
    public float bombChance = 0.10f;

    [Header("Spawn Settings")]
    public float minSpawnDelay = 0.60f;
    public float maxSpawnDelay = 1.20f;

    public float minAngle = -15f;
    public float maxAngle = 15f;

    public float minForce = 16f;
    public float maxForce = 20f;

    public float maxLifetime = 5f;

    private Collider spawnArea;

    private void Awake()
    {
        spawnArea = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(1f);

        while (enabled)
        {
            GameObject prefab = ChooseRandomItem();

            if (prefab != null)
            {
                Vector3 position = GetRandomPosition();
                Quaternion rotation = Random.rotation;

                GameObject item = Instantiate(prefab, position, rotation);

                Rigidbody rb = item.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddForce(GetRandomForce(), ForceMode.Impulse);
                    rb.AddTorque(Random.insideUnitSphere * 5f, ForceMode.Impulse);
                }

                Destroy(item, maxLifetime);
            }

            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);
        }
    }

    private GameObject ChooseRandomItem()
    {
        bool spawnBomb = Random.value < bombChance;

        if (spawnBomb && bombPrefabs != null && bombPrefabs.Length > 0)
        {
            int bombIndex = Random.Range(0, bombPrefabs.Length);
            return bombPrefabs[bombIndex];
        }

        bool spawnSpecialFruit = Random.value < specialFruitChance;

        if (spawnSpecialFruit && specialFruitPrefabs != null && specialFruitPrefabs.Length > 0)
        {
            int specialIndex = Random.Range(0, specialFruitPrefabs.Length);
            return specialFruitPrefabs[specialIndex];
        }

        if (normalFruitPrefabs != null && normalFruitPrefabs.Length > 0)
        {
            int fruitIndex = Random.Range(0, normalFruitPrefabs.Length);
            return normalFruitPrefabs[fruitIndex];
        }

        return null;
    }

    private Vector3 GetRandomPosition()
    {
        Bounds bounds = spawnArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        float z = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(x, y, z);
    }

    private Vector3 GetRandomForce()
    {
        float force = Random.Range(minForce, maxForce);
        float angle = Random.Range(minAngle, maxAngle);

        Vector3 direction = Quaternion.Euler(0f, 0f, angle) * Vector3.up;

        return direction * force;
    }
}