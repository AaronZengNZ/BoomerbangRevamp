using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistData : MonoBehaviour
{
    public float playerMaxHP = 100f;
    public float playerRegen = 10f;
    public float playerSpeed = 5f;
    public float boomerangDps = 50f;
    public float explosionDmg = 50f;
    public float explosionSize = 2.5f;
    public float enemyHpMulti = 1f;
    public float enemySpdMulti = 1f;
    public float bossHpMulti = 1f;
    // Start is called before the first frame update
    void Start()
    {
        //scene persist if no other are existing
        if(GameObject.FindGameObjectsWithTag("Persist").Length == 1){
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    public void SetData(string dataType, float amount){
        if(dataType == "playerMaxHP"){
            playerMaxHP = amount;
        }
        if(dataType == "playerRegen"){
            playerRegen = amount;
        }
        if(dataType == "playerSpeed"){
            playerSpeed = amount;
        }
        if(dataType == "boomerangDps"){
            boomerangDps = amount;
        }
        if(dataType == "explosionDmg"){
            explosionDmg = amount;
        }
        if(dataType == "explosionSize"){
            explosionSize = amount;
        }
        if(dataType == "enemyHpMulti"){
            enemyHpMulti = amount;
        }
        if(dataType == "enemySpdMulti"){
            enemySpdMulti = amount;
        }
        if(dataType == "bossHpMulti"){
            bossHpMulti = amount;
        }
    }
}
