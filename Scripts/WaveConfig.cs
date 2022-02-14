using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float spawnTime = 0.5f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] int numberOfEnemies = 5;
    [SerializeField] float moveSpeed = 2f;

    private bool isStorm;

    #region Properties
    public GameObject MyEnemy { get { return enemyPrefab; } }
    public float MySpawnTime { get { return spawnTime; } }
    public float MySpawnRandomFactor { get { return spawnRandomFactor; } }
    public float MyMoveSpeed { get { return moveSpeed; } }
    public int MyNumberOfEnemies { get { return numberOfEnemies; } }
    public bool IsStorm { get { return isStorm; } }
    #endregion

    #region Methods
    public List<Transform> GetWaypoints()
    {
        var waveWaypoints = new List<Transform>();

        foreach(Transform child in pathPrefab.transform)
        {
            waveWaypoints.Add(child);
        }
        return waveWaypoints;
    }
    #endregion
}
