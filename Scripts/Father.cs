using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Father : MonoBehaviour
{
    [SerializeField] GameObject kidToSpawn;
    [SerializeField] int numberOfKids = 5;
    [SerializeField] bool canApplyForce = false;
    private GameObject[] kids;
    [SerializeField] private float minRange, maxRange;
    private int force = 10;

    public void SpawnKids()
    {
        for (int i = 0; i < numberOfKids; i++)
        {
            float x = Random.Range(-0.5f, 0.5f);
            float y = Random.Range(-0.5f, 0.5f);
            Vector3 offsetBetweenKids = new Vector3(x, y, transform.position.z);
            GameObject kid = Instantiate(kidToSpawn, transform.position + offsetBetweenKids, Quaternion.identity);
            if (canApplyForce)
            {
                ApplyForce(kid);
            }
        } 
    }

    private void ApplyForce(GameObject kid)
    {
            Vector2 forceToAdd = new Vector2(Random.Range(minRange, maxRange),
                Random.Range(minRange, maxRange));
            kid.GetComponent<Rigidbody2D>().AddForce(forceToAdd * force);
    }
}
