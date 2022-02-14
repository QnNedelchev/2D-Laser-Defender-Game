using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPowerUP : MonoBehaviour
{
    Material material;
    private bool isDissolving = false;
    [SerializeField] private float fade = 1f;
    [SerializeField] private float statAmount;

    private void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
       if (isDissolving)
       {
           Dissolve();
       }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isDissolving = true;
            GiveStat(collision);
        }
    }

    private void Dissolve() 
    {
        fade -= Time.deltaTime;

        if (fade <= 0)
        {
            Destroy(gameObject);
        }
        material.SetFloat("_Fade", fade);
    }

    private void GiveStat(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        player.PickedArmor = true;
        player.MyArmor.MyCurrentValue += statAmount;
    }
}
