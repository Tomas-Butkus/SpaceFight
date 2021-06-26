using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float projectileFiringPeriod = 0.1f;
    [SerializeField] private float projectileSpeed = 10f;

    [Header("Player")]
    [SerializeField] private AudioClip deathSFX;
    [SerializeField] private AudioClip shootSFX;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float padding = 1f;
    [SerializeField] [Range(0, 1)] private float deathSoundVolume = 0.6f;
    [SerializeField] [Range(0, 1)] private float shootSoundVolume = 0.6f;
    [SerializeField] private int health = 200;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    Coroutine firingCoroutine;

    [SerializeField] private Joystick joystick;
    [SerializeField] private Button button;

    private void Start()
    {
        SetUpMoveBoundaries();
    }

    private void Update()
    {
        MoveByTouch();
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private void MoveByTouch()
    {
        var deltaX = joystick.Horizontal * Time.deltaTime * movementSpeed;
        var deltaY = joystick.Vertical * Time.deltaTime * movementSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

    public void Fire()
    {
        firingCoroutine = StartCoroutine(FireContinuously());
    }

    public void StopFiring()
    {
        StopCoroutine(firingCoroutine);
    }

    private IEnumerator FireContinuously()
    {
        while(true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position, shootSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if(!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSoundVolume);
    }

    public int GetHealth() { return health; }
}
