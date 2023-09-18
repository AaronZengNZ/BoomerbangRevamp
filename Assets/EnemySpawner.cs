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
    public float enemySpawner = 0f;
    public GameObject[] enemyPrefabs;
    public GameObject victoryCanvas;
    public GameObject[] selectedEnemyPrefabs;
    public float phase = 0f;
    public float bossHpMulti = 1f;
    public float[] bossWaves;
    public GameObject[] bosses;
    // Start is called before the first frame update
    void Start()
    {
        upgradeManager = GameObject.Find("UpgradeManager").GetComponent<UpgradeManager>();
        PersistData persistData;
        persistData = GameObject.Find("PersistData").GetComponent<PersistData>();
        //set hp to persistData's hp
        enemyHpMulti = persistData.enemyHpMulti;
        //set speed to persistData's speed
        spdMulti = persistData.enemySpdMulti;
        bossHpMulti = persistData.bossHpMulti;
    }
    public void AddEnemySpawner(float enemyType){
        enemyWaveLists[(int)enemySpawner].enemyType = enemyPrefabs[(int)enemyType];
        //add enemyPrefabs[enemyType] to selectedEnemyPrefabs
        float enemyIndex = 0f;
        GameObject[] tempSelectedEnemyPrefabs;
        tempSelectedEnemyPrefabs = new GameObject[selectedEnemyPrefabs.Length + 1];
        for(int i = 0; i < selectedEnemyPrefabs.Length; i++){
            tempSelectedEnemyPrefabs[i] = selectedEnemyPrefabs[i];
        }
        tempSelectedEnemyPrefabs[selectedEnemyPrefabs.Length] = enemyPrefabs[(int)enemyType];
        selectedEnemyPrefabs = tempSelectedEnemyPrefabs;
        enemySpawner++;
    }

    public void ResetSpawners(){
        //empty out all of the enemywaveLists's enemy types
        for(int i = 0; i < enemyWaveLists.Length; i++){
            enemyWaveLists[i].enemyType = null;
        }
        phase++;
    }

    public void SetAnimatorVariable(){
        waveIndicator.SetBool("NewWave", false);
    }

    public void NextWave(){
        StartCoroutine(SummonWave());
    }

    void IncreaseEnemyStats(){
        enemyHpMulti += 0.05f;
        spdMulti += 0.025f;
    }

    IEnumerator SummonWave(){
        if(waveNum >= 21){
            //destory player
            Destroy(GameObject.Find("Player"));
            Destroy(GameObject.Find("Boomerang"));
            //find enemyspawner, upgrade manager and set both of them inactive
            GameObject.Find("UpgradeManager").SetActive(false);
            //activate gameovercanvas
            victoryCanvas.SetActive(true);
            //set gameOverCanvas's animator's transition bool
            victoryCanvas.GetComponent<Animator>().SetBool("Transition", true);
            Destroy(gameObject);
        }
        //check wave for boss
        for(int i = 0; i < bossWaves.Length; i++){
            if(waveNum == bossWaves[i]){
                ResetSpawners();
                enemySpawner = 0f;
                waveIndicator.SetBool("NewWave", true);
                GameObject boss = Instantiate(bosses[i], Vector2.zero, Quaternion.identity);
                boss.GetComponent<BossScript>().hp *= bossHpMulti;
                boss.GetComponent<BossScript>().maxHp *= bossHpMulti;
                //wait until boss is dead
                while(boss != null){
                    yield return null;
                }
                //deal 999 damage to every enemy
                foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")){
                    enemy.GetComponent<Enemy>().TakeDamage(999f);
                }
                //upgradeManager's phase add one
                upgradeManager.phase++;
                //create boss upgrades
                upgradeManager.CreateBossUpgrades();
                waveNum++;
                yield break;
            }
        }   
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
            upgrade++;
            upgradeManager.CreateUpgrades();
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
        GameObject enemy;
        if(phase >= 1f){
            //instantiate random enemy of selected enemies
            enemy = Instantiate(selectedEnemyPrefabs[(int)Random.Range(0, phase*2f)], spawnLocation, Quaternion.identity);
        }
        else{
            enemy = Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
        }
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

    void Update(){
        waveTextIndicator.text = "Wave " + waveNum;
        //check boss wave
        for(int i = 0; i < bossWaves.Length; i++){
            if(waveNum == bossWaves[i]){
                waveTextIndicator.text = "Boss Wave";
            }
        }
    }
}
