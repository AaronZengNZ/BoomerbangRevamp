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

    public void CreateUpgrades(){
        //create upgrade buttons
        getUpgrade.SetActive(true);
        float[] tempEnemyTypes = enemyTypes;
        GameObject[] tempRepeatableUpgrades = repeatableUpgrades;
        for(int i = 0; i < upgradeButtonWaypoints.Length; i++){
            //create upgrade button
            GameObject upgradeButton = Instantiate(tempRepeatableUpgrades[Random.Range(0, tempRepeatableUpgrades.Length)], upgradeButtonWaypoints[i].position, Quaternion.identity);
            //set parent
            upgradeButton.transform.SetParent(upgradeButtonWaypoints[i]);
            GameObject[] tempTempRepeatbleUpgrades = new GameObject[tempRepeatableUpgrades.Length - 1];
            int tempTempRepeatbleUpgradesIndex = 0;
            for(int j = 0; j < tempRepeatableUpgrades.Length; j++){
                if(tempRepeatableUpgrades[j] != upgradeButton){
                    tempTempRepeatbleUpgrades[tempTempRepeatbleUpgradesIndex] = tempRepeatableUpgrades[j];
                    tempTempRepeatbleUpgradesIndex++;
                }
            }
            tempRepeatableUpgrades = tempTempRepeatbleUpgrades;
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
        }
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
        //next wave
        if(main == true){
            enemySpawner.NextWave();
            getUpgrade.SetActive(false);
        }
   }
}