using UnityEngine;

public class Blade : MonoBehaviour
{
    public float sliceForce = 5f;
    public float minSliceVelocity = 0.01f;

    [Header("Audio Settings")]
    [SerializeField] private float swingSoundCooldown = 0.25f;

    private Camera mainCamera;
    private Collider sliceCollider;
    private TrailRenderer sliceTrail;

    public Vector3 direction { get; private set; }

    private bool slicing;
    private float lastSwingSoundTime;

    private void Awake()
    {
        mainCamera = Camera.main;
        sliceCollider = GetComponent<Collider>();
        sliceTrail = GetComponentInChildren<TrailRenderer>();
    }

    private void OnEnable()
    {
        StopSlice();
    }

    private void OnDisable()
    {
        StopSlice();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartSlice();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopSlice();
        }
        else if (slicing)
        {
            ContinueSlice();
        }
    }

    private void StartSlice()
    {
        Vector3 position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0f;
        transform.position = position;

        slicing = true;

        if (sliceCollider != null)
        {
            sliceCollider.enabled = true;
        }

        if (sliceTrail != null)
        {
            sliceTrail.enabled = true;
            sliceTrail.Clear();
        }

        PlaySwingSound();
    }

    private void StopSlice()
    {
        slicing = false;

        if (sliceCollider != null)
        {
            sliceCollider.enabled = false;
        }

        if (sliceTrail != null)
        {
            sliceTrail.enabled = false;
        }
    }

    private void ContinueSlice()
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;

        direction = newPosition - transform.position;

        float velocity = direction.magnitude / Time.deltaTime;

        if (sliceCollider != null)
        {
            sliceCollider.enabled = velocity > minSliceVelocity;
        }

        if (velocity > minSliceVelocity)
        {
            PlaySwingSoundWithCooldown();
        }

        transform.position = newPosition;
    }

    private void PlaySwingSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBladeSwing();
        }

        lastSwingSoundTime = Time.time;
    }

    private void PlaySwingSoundWithCooldown()
    {
        if (Time.time - lastSwingSoundTime < swingSoundCooldown)
        {
            return;
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBladeSwing();
        }

        lastSwingSoundTime = Time.time;
    }
}