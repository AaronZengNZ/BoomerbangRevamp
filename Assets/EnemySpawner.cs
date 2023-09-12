using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float[] waves;
    public float waveNum = 1f;
    public float interval = 0.3f;
    public float spdMulti = 1f;
    public float enemyHpMulti = 1f;
    public Animator waveIndicator;
    public TextMeshProUGUI waveTextIndicator;
    public EnemyWaveList[] enemyWaveLists;
    public float[] wavesWithUpgrades;
    UpgradeManager upgradeManager;
    public float upgrade = 0f;
    // Start is called before the first frame update
    void Start()
    {
        upgradeManager = GameObject.Find("UpgradeManager").GetComponent<UpgradeManager>();
    }

    public void SetAnimatorVariable(){
        waveIndicator.SetBool("NewWave", false);
    }

    public void NextWave(){
        StartCoroutine(SummonWave());
    }

    void IncreaseEnemyStats(){
        enemyHpMulti += 0.1f;
        spdMulti += 0.05f;
    }

    IEnumerator SummonWave(){
        waveIndicator.SetBool("NewWave", true);
        for(int j = 0; j < waves[(int)waveNum - 1]; j++){
            SpawnEnemy();
            yield return new WaitForSeconds(interval / waves[(int)waveNum - 1]);
        }
        //spawn secondary enemies
        for(int i = 0; i < enemyWaveLists.Length; i++){
            //if there are any enemies to be spawned
            if(enemyWaveLists[i].waves[(int)waveNum-1] > 0f){
                for(int j = 0; j < enemyWaveLists[i].waves[(int)waveNum-1]; j++){
                   SpawnSecondaryEnemy(enemyWaveLists[i].enemyType);
                   yield return new WaitForSeconds(interval / enemyWaveLists[i].waves[(int)waveNum-1]);
                }
            }
        }
        //wait until no enemies exist
        while(GameObject.FindGameObjectsWithTag("Enemy").Length > 0){
            yield return null;
        }
        //increment wave number
        waveNum++;
        if(waveNum == wavesWithUpgrades[(int)upgrade]+1f){
            upgradeManager.CreateUpgrades();
            upgrade++;
        }
        else{
            NextWave();
        }
        IncreaseEnemyStats();
    }

    void SpawnSecondaryEnemy(GameObject prefab){
        //find somewhere random and instantiate enemy
        Vector2 spawnLocation = new Vector2(Random.Range(-8f, 8f), Random.Range(-4f, 4f));
        GameObject enemy = Instantiate(prefab, spawnLocation, Quaternion.identity);
        //if enemy has children that have the enemy script
        if(enemy.transform.childCount > 0){
            for(int i = 0; i < enemy.transform.childCount; i++){
                if(enemy.transform.GetChild(i).GetComponent<Enemy>() != null){
                    enemy.transform.GetChild(i).GetComponent<Enemy>().speed *= spdMulti;
                    enemy.transform.GetChild(i).GetComponent<Enemy>().hp *= enemyHpMulti;
                }
            }
        }
        //if enemy has an enemy script
        if(enemy.GetComponent<Enemy>() != null){
            enemy.GetComponent<Enemy>().speed *= spdMulti;
            enemy.GetComponent<Enemy>().hp *= enemyHpMulti;
        }
    }

    void SpawnEnemy(){
        //find somewhere random and instantiate enemy
        Vector2 spawnLocation = new Vector2(Random.Range(-8f, 8f), Random.Range(-4f, 4f));
        GameObject enemy = Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
        enemy.GetComponent<Enemy>().speed *= spdMulti;
        enemy.GetComponent<Enemy>().hp *= enemyHpMulti;
    }

    void Update(){
        waveTextIndicator.text = "Wave " + waveNum;
    }
}
