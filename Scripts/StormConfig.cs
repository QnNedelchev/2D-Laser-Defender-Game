using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormConfig : MonoBehaviour
{

    #region Singleton

    private static StormConfig instance;

    public static StormConfig MyInstance
    {
        get 
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StormConfig>();
            }

            return instance;
        }
    }

    #endregion

    [SerializeField] private GameObject stormElement; 
    [SerializeField] private GameObject positions;
    [SerializeField] float spawnTime = 1f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] int amount;

    public bool isStormActive;
    public bool started;

    #region Properties

    public GameObject MyStormElement { get { return stormElement; } }
    public float MySpawnTime { get { return spawnTime; } }
    public float MySpawnRandomFactor { get { return spawnRandomFactor; } }

    #endregion


    private void Update()
    {
        if(isStormActive && !started)
        {
            StartCoroutine(SpawnAllWaves());
            started = true;
        }
    }

    private IEnumerator SpawnAllWaves()
    {
        for (int positionIndex = 0; positionIndex < amount; positionIndex++)
        {
            yield return StartCoroutine(SpawnInWave(positionIndex));
        }
    }

    private IEnumerator SpawnInWave(int index)
    {   
       var newEnemy = Instantiate(stormElement, positions.transform.GetChild(index).position,
       Quaternion.identity);

       yield return new WaitForSeconds(spawnTime);
       Destroy(newEnemy.gameObject);
    }
}
