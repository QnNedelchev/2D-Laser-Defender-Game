using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FollowTartget : MonoBehaviour
{
    private GameObject target;
    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            var targetPos = target.transform.position;
            var movementThisFrame = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, movementThisFrame);
        }
    }
}
