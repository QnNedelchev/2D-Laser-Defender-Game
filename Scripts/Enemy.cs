using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float health = 200;
    [SerializeField] int scoreValue = 150;


    [Header("Shooting")]
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject shotExit;
    float shotCounter;


    [Header("VFX")]
    [SerializeField] GameObject explosionVFX;
    [SerializeField] float VFXTime;


    [Header("SFX")]
    [SerializeField] AudioClip laserSFX;
    [SerializeField] [Range(0, 1)] float laserSFXvolume;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] float deathSFXvolume;

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
        if (shotCounter <= 0)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            ProcessHit(damageDealer);
        }
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        if (health <= 0)
        {
            Die();
        }
        health -= damageDealer.GetDamage();

        damageDealer.Hit();
    }

    private void Fire()
    {
        GameObject projectile = Instantiate(projectilePrefab, shotExit.transform.position, Quaternion.identity) as GameObject;
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position,laserSFXvolume);
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        AudioSource.PlayClipAtPoint(deathSFX,Camera.main.transform.position, deathSFXvolume);
        GameObject explosion = Instantiate(explosionVFX, transform.position, Quaternion.identity);
        
        CameraShake.MyInstance.Shake();
        Father father = GetComponent<Father>();
        
        if(father != null)
        {
            father.SpawnKids();
        }
        Destroy(explosion, VFXTime);
        Destroy(gameObject);
    }
}
