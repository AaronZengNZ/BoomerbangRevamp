using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Player player;
    public Boomerang boomerang;
    public EnemySpawner enemySpawner;
    public Transform[] upgradeButtonWaypoints;
    public GameObject[] oneTimeUpgrades;
    public GameObject[] repeatableUpgrades;
    public GameObject getUpgrade;
    public float[] enemyTypes;
    float[] upgradeIds;
    public float[] bossUpgradeIds;
    public string[] enemyDescriptions;
    public GameObject emptySign;
    public GameObject blueButterfly;
    public float phase = 0f;
    public float enemyTypesSelected = 0f;

    void Start(){
        //set upgradeIds based on repeatable ugprades
        upgradeIds = new float[repeatableUpgrades.Length];
        for(int i = 0; i < repeatableUpgrades.Length; i++){
            upgradeIds[i] = i;
        }
        //set bossUpgradeIds based on one time upgrades
        bossUpgradeIds = new float[oneTimeUpgrades.Length];
        for(int i = 0; i < oneTimeUpgrades.Length; i++){
            bossUpgradeIds[i] = i;
        }
        CreateBossUpgrades();
    }

    public void CreateBossUpgrades(){
        //create one time upgrades(they do not have an enemy)
        getUpgrade.SetActive(true);
        float[] tempUpgradeIds = bossUpgradeIds;
        float upgradeIdsLeft = tempUpgradeIds.Length;
        for(int i = 0; i < upgradeButtonWaypoints.Length; i++){
            //create upgrade button
            float randomisedUpgrade = tempUpgradeIds[(int)Mathf.Floor(UnityEngine.Random.Range(0, upgradeIdsLeft))];
            upgradeIdsLeft--;
            //remove randomised upgrade from temp upgrade ids
            float[] tempTempUpgradeIds = new float[tempUpgradeIds.Length - 1];
            int tempTempUpgradeIdsIndex = 0;
            for(int j = 0; j < tempUpgradeIds.Length; j++){
                if(tempUpgradeIds[j] != randomisedUpgrade){
                    tempTempUpgradeIds[tempTempUpgradeIdsIndex] = tempUpgradeIds[j];
                    tempTempUpgradeIdsIndex++;
                }
            }
            tempUpgradeIds = tempTempUpgradeIds;
            GameObject upgradeButton = Instantiate(oneTimeUpgrades[(int)randomisedUpgrade], upgradeButtonWaypoints[i].position, Quaternion.identity);
            //set parent
            upgradeButton.transform.SetParent(upgradeButtonWaypoints[i]);
        }
    }

    public void RemoveOneTimeUpgrade(string name){
        //remove one time upgrade
        float[] tempUpgradeIds = new float[oneTimeUpgrades.Length - 1];
        int tempUpgradeIdsIndex = 0;
        int upgradeId = 0;
        bool reached = false;
        for(int i = 0; i < oneTimeUpgrades.Length; i++){
            if(oneTimeUpgrades[i].name != name){
                tempUpgradeIds[tempUpgradeIdsIndex] = i;
                tempUpgradeIdsIndex++;
                if(reached == false){
                    upgradeId++;
                }
            }else{reached=true;}
        }
        bossUpgradeIds = tempUpgradeIds;
    }

    public void CreateUpgrades(){
        //create upgrade buttons
        getUpgrade.SetActive(true);
        float[] tempEnemyTypes = enemyTypes;
        float[] tempUpgradeIds = upgradeIds;
        float upgradeIdsLeft = tempUpgradeIds.Length;
        float tempEnemySelected = 0f;
        for(int i = 0; i < upgradeButtonWaypoints.Length+1f; i++){
            //create upgrade button
            float randomisedUpgrade = tempUpgradeIds[(int)Random.Range(0, upgradeIdsLeft)];
            //remove randomised upgrade from temp upgrade ids
            float[] tempTempUpgradeIds = new float[tempUpgradeIds.Length - 1];
            int tempTempUpgradeIdsIndex = 0;
            for(int j = 0; j < tempUpgradeIds.Length; j++){
                if(tempUpgradeIds[j] != randomisedUpgrade){
                    tempTempUpgradeIds[tempTempUpgradeIdsIndex] = tempUpgradeIds[j];
                    tempTempUpgradeIdsIndex++;
                }
            }
            tempUpgradeIds = tempTempUpgradeIds;
            upgradeIdsLeft--;
            GameObject upgradeButton = Instantiate(repeatableUpgrades[(int)randomisedUpgrade], upgradeButtonWaypoints[i].position, Quaternion.identity);
            //set parent
            upgradeButton.transform.SetParent(upgradeButtonWaypoints[i]);
            //set upgrade enemy type
            float randomNumber = Random.Range(0, (4f + phase*3f) - enemyTypesSelected - tempEnemySelected);
            UnityEngine.Debug.Log(tempEnemyTypes[(int)((4f + phase*3f) - enemyTypesSelected - tempEnemySelected)]);
            upgradeButton.GetComponent<Upgrade>().enemyType = tempEnemyTypes[(int)randomNumber];
            float[] tempTempEnemyTypes = new float[tempEnemyTypes.Length - 1];
            int tempTempEnemyTypesIndex = 0;
            for(int j = 0; j < tempEnemyTypes.Length; j++){
                if(tempEnemyTypes[j] != upgradeButton.GetComponent<Upgrade>().enemyType){
                    tempTempEnemyTypes[tempTempEnemyTypesIndex] = tempEnemyTypes[j];
                    tempTempEnemyTypesIndex++;
                }
            }
            tempEnemySelected++;
            tempEnemyTypes = tempTempEnemyTypes;
        }
    }
    public void RemoveEnemyType(float id){
        float[] tempEnemyTypes = new float[enemyTypes.Length - 1];
        int tempEnemyTypesIndex = 0;
        for(int i = 0; i < enemyTypes.Length; i++){
            if(enemyTypes[i] != id){
                tempEnemyTypes[tempEnemyTypesIndex] = enemyTypes[i];
                tempEnemyTypesIndex++;
            }
        }
        enemyTypes = tempEnemyTypes;
        enemyTypesSelected++;
    }
    public void IncreaseStat(string stat, float amount, string type,bool main = false){
        if(stat == "playerSpd"){
            //add spd multi
            if(type == "multi"){
                player.spdMulti *= amount;
            }
            else{
                player.spdMulti += amount;
            }
        }
        if(stat == "boomerangDmg"){
            //add boomerang dmg
            if(type == "multi"){
                boomerang.damage *= amount;
            }
            else{
                boomerang.damage += amount;
            }
        }
        if(stat == "explosionDmg"){
            //add explosion dmg
            if(type == "multi"){
                boomerang.explosionDamage *= amount;
            }
            else{
                boomerang.explosionDamage += amount;
            }
        }
        if(stat == "enemySpd"){
            //add enemy spd
            if(type == "multi"){
                enemySpawner.spdMulti *= amount;
            }
            else{
                enemySpawner.spdMulti += amount;
            }
        }
        if(stat == "boomerangSize"){
            //add boomerang size
            if(type == "multi"){
                boomerang.boomerangSize *= amount;
            }
            else{
                boomerang.boomerangSize += amount;
            }
        }
        if(stat == "loopyPlayer"){
            if(type == "setBoolTrue"){
                player.looping = true;
            }
        }
        if(stat == "flowerBoomerang"){
            if(type == "setBoolTrue"){
                boomerang.shootBees = true;
            }
        }
        if(stat == "homingBoomerang"){
            if(type == "multi"){
                boomerang.mouseHomingAmount *= amount;
            }
            else{
                boomerang.mouseHomingAmount += amount;
            }
        }
        if(stat == "returnVelocity"){
            if(type == "multi"){
                boomerang.returnVelocity *= amount;
            }
            else{
                boomerang.returnVelocity += amount;
            }
        }
        if(stat == "explosionCooldown"){
            if(type == "multi"){
                boomerang.explosionCooldown *= amount;
            }
            else{
                boomerang.explosionCooldown += amount;
            }
        }
        if(stat == "explosionSize"){
            if(type == "multi"){
                boomerang.explosionRadius *= amount;
            }
            else{
                boomerang.explosionRadius += amount;
            }
        }
        if(stat == "maxHp"){
            if(type == "multi"){
                player.maxHp *= amount;
            }
            else{
                player.maxHp += amount;
            }
        }
        if(stat == "healthRegen"){
            if(type == "multi"){
                player.regenSpd *= amount;
            }
            else{
                player.regenSpd += amount;
            }
        }
        if(stat == "emptySign"){
            //instantiate an empty sign at a random position
            Instantiate(emptySign, new Vector3(Random.Range(-6.8f, 6.8f), Random.Range(-3.4f, 3.4f), 0), Quaternion.identity);
        }
        if(stat == "blueButterfly"){
            //instantiate a blue butterfly at a random position
            Instantiate(blueButterfly, new Vector3(Random.Range(-6.8f, 6.8f), Random.Range(-3.4f, 3.4f), 0), Quaternion.identity);
        }
        //next wave
        if(main == true){
            enemySpawner.NextWave();
            getUpgrade.SetActive(false);
        }
   }
}
