using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private float health = 100;
    [SerializeField] private int scoreValue = 100;

    [Header("Prefabs")]
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject deathVFX;

    [Header("SFX")]
    [SerializeField] private AudioClip deathSFX;
    [SerializeField] private AudioClip shootSFX;

    [SerializeField] private float shotCounter;
    [SerializeField] private float minTimeBetweenShots = 0.2f;
    [SerializeField] private float maxTimeBetweenShots = 3f;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float durationOfExplosion = 1f;
    [SerializeField][Range(0, 1)] private float deathSoundVolume = 0.6f;
    [SerializeField][Range(0, 1)] private float shootSoundVolume = 0.3f;

    private void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    private void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject enemyLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        enemyLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position, shootSoundVolume);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
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
        FindObjectOfType<Score>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject particle = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(particle, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSoundVolume);
    }
}
