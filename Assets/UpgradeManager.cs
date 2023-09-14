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
    public GameObject[] repeatableUpgrades;
    public GameObject getUpgrade;
    public float[] enemyTypes;
    public float[] upgradeIds;
    public string[] enemyDescriptions;

    public void CreateUpgrades(){
        //create upgrade buttons
        getUpgrade.SetActive(true);
        float[] tempEnemyTypes = enemyTypes;
        float[] tempUpgradeIds = upgradeIds;
        float upgradeIdsLeft = tempUpgradeIds.Length;
        for(int i = 0; i < upgradeButtonWaypoints.Length; i++){
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
            upgradeButton.GetComponent<Upgrade>().enemyType = tempEnemyTypes[Random.Range(0, tempEnemyTypes.Length)];
            //use a for loop to remove
            float[] tempTempEnemyTypes = new float[tempEnemyTypes.Length - 1];
            int tempTempEnemyTypesIndex = 0;
            for(int j = 0; j < tempEnemyTypes.Length; j++){
                if(tempEnemyTypes[j] != upgradeButton.GetComponent<Upgrade>().enemyType){
                    tempTempEnemyTypes[tempTempEnemyTypesIndex] = tempEnemyTypes[j];
                    tempTempEnemyTypesIndex++;
                }
            }
            tempEnemyTypes = tempTempEnemyTypes;
            UnityEngine.Debug.Log(tempEnemyTypes);
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
        //next wave
        if(main == true){
            enemySpawner.NextWave();
            getUpgrade.SetActive(false);
        }
   }
}
