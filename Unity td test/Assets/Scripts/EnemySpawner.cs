using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public PathNode m_startNode;
    private int m_liveEnemy = 0;
    public List<WaveData> waves;
    int enemyIndex = 0;
    int waveIndex = 0;

    void Start() {
        
        StartCoroutine(SpawnEnemies());
        
    }

    IEnumerator SpawnEnemies() {
        yield return new WaitForEndOfFrame();   //execute after start func
        GameManager.Instance.SetWave(waveIndex + 1);

        WaveData wave = waves[waveIndex];
        yield return new WaitForSeconds(wave.interval);
        while (enemyIndex < wave.enemyPrefab.Count) {
            Vector3 dir = m_startNode.transform.position - this.transform.position;
            GameObject enemyObj = (GameObject)Instantiate(wave.enemyPrefab[enemyIndex], transform.position, Quaternion.LookRotation(dir));
            Enemy enemy = enemyObj.GetComponent<Enemy>();
            enemy.m_currentNode = m_startNode;

            //this is a enemy data exmple,in a game project actually,put data in a database such as SQLite
            enemy.m_life = wave.level * 3;
            enemy.m_maxlife = enemy.m_life;

            m_liveEnemy++;
            enemy.onDeath = new System.Action<Enemy>(
                (Enemy e) => {m_liveEnemy--;}
            );

            enemyIndex++;
            yield return new WaitForSeconds(wave.interval);
        }

        while (m_liveEnemy > 0) yield return 0;

        enemyIndex = 0;
        waveIndex++;
        if (waveIndex < waves.Count) {
            StartCoroutine(SpawnEnemies());
        } else {
            Debug.Log("You Win!");
        }
    }
    private void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position, "spawner.tif");
    }

}
