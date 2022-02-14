using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kid : MonoBehaviour
{
    [SerializeField] private float health = 20;
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private AudioClip deathSFX;
    [SerializeField] private GameObject deathVFX;
    [SerializeField][Range(0, 1)] float deathSFXvolume;
    

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

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXvolume);
        GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.identity);
        
        CameraShake.MyInstance.Shake();
        Destroy(explosion, 1.5f);
        Destroy(gameObject);
    }
}
