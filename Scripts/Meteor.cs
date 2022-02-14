using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor: MonoBehaviour
{
    [SerializeField] GameObject hitEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();

        if (damageDealer != null)
        {
            damageDealer.Hit();
        }

        if (collision.gameObject.tag == "Player")
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }
    }
}
