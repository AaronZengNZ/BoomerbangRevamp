using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySelector : MonoBehaviour
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
    public PersistData persistData;
    // Start is called before the first frame update
    void Start()
    {
        //find
        persistData = GameObject.Find("PersistData").GetComponent<PersistData>();
    }

    public void SetData(){
        persistData.SetData("playerMaxHP", playerMaxHP);
        persistData.SetData("playerRegen", playerRegen);
        persistData.SetData("playerSpeed", playerSpeed);
        persistData.SetData("boomerangDps", boomerangDps);
        persistData.SetData("explosionDmg", explosionDmg);
        persistData.SetData("explosionSize", explosionSize);
        persistData.SetData("enemyHpMulti", enemyHpMulti);
        persistData.SetData("enemySpdMulti", enemySpdMulti);
        persistData.SetData("bossHpMulti", bossHpMulti);
    }
}
