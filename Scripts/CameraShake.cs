using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    #region Singleton

    private static CameraShake instance;

    public static CameraShake MyInstance
    {
        get 
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CameraShake>();
            }
            return instance;
        }
    
    }

    #endregion

    [SerializeField] Animator anim;

    public void Shake()
    {
        int rand = Random.Range(0, 3);

        if (rand == 0)
        {
            anim.SetTrigger("shake");
        }
        else if (rand == 1)
        {
            anim.SetTrigger("shake1");
        }
        else if (rand == 2)
        {
            anim.SetTrigger("shake2");
        }
    }
}
