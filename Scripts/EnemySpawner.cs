using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waves;
    [SerializeField] int startingWave = 0;
    [SerializeField] private bool looping;
    [SerializeField] private bool stormMode;

    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllLaves());
        }
        while (looping);
    }


    private IEnumerator SpawnAllLaves()
    {
        for (int waveIndex = startingWave; waveIndex < waves.Count; waveIndex++)
        {
            if (waveIndex % 2 == 0)
            {
                StormConfig.MyInstance.isStormActive = true;
                StormConfig.MyInstance.started = false;
            }
            else StormConfig.MyInstance.isStormActive = false;
            
            var currentWave = waves[waveIndex];

            yield return StartCoroutine(SpawnInWave(currentWave));
        }
    }

    private IEnumerator SpawnInWave(WaveConfig wave)
    {
        for (int enemyCount = 0; enemyCount < wave.MyNumberOfEnemies; enemyCount++)
        {
            var newEnemy = Instantiate(wave.MyEnemy, wave.GetWaypoints()[0].position,
          Quaternion.identity);
           
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(wave);

            yield return new WaitForSeconds(wave.MySpawnTime);
        }
    }

}
