using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public float damage = 30f;
    public string proportionateDamage = "none";
    public float damageProportion = 0.5f;
    public float attackCooldown = 0.4f;
    public float attackRange = 0.5f;
    public float moveSpeed = 1f;
    public string target = "player";
    Boomerang boomerang;
    // Start is called before the first frame update
    void Start()
    {
        //start attack
        StartCoroutine(AttackEnemies());
        //find boomerang
        boomerang = GameObject.Find("Boomerang").GetComponent<Boomerang>();
    }

    void Update(){
        //move towards player
        if(target == "player"){
            transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position, moveSpeed * Time.deltaTime);
        }
        if(target == "enemy"){
            //find the closest enemy
            GameObject closestEnemy = null;
            float closestDistance = Mathf.Infinity;
            foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")){
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if(distance < closestDistance){
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
            //if no enemy, check for bosses
            if(closestEnemy == null){
                foreach(GameObject enemyBoss in GameObject.FindGameObjectsWithTag("EnemyBoss")){
                    float distance = Vector2.Distance(transform.position, enemyBoss.transform.position);
                    if(distance < closestDistance){
                        closestDistance = distance;
                        closestEnemy = enemyBoss;
                    }
                }
            }
            //move towards closest enemy
            if(closestEnemy != null){
                transform.position = Vector2.MoveTowards(transform.position, closestEnemy.transform.position, moveSpeed * Time.deltaTime);
            }
            else{
                //just move towards player, but stop at 1
                transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position, moveSpeed * Time.deltaTime);

            }
        }
        SetDamageProportion();
    }

    void SetDamageProportion(){
        if(proportionateDamage == "none"){
            return;
        }
        if(proportionateDamage == "dps"){
            damage = boomerang.damage * damageProportion;
        }
        if(proportionateDamage == "explosion"){
            damage = boomerang.explosionDamage * damageProportion;
        }
    }

    IEnumerator AttackEnemies(){
        //get all enemies in range
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attackRange);
        //loop through all enemies in range
        foreach(Collider2D enemy in enemiesInRange){
            //if enemy has a health script
            if(enemy.GetComponent<Enemy>()){
                //deal damage to enemy
                enemy.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
        //also check for enemybosses
        foreach(GameObject enemyBoss in GameObject.FindGameObjectsWithTag("EnemyBoss")){
            if(Vector2.Distance(transform.position, enemyBoss.transform.position) < attackRange){
                enemyBoss.GetComponent<BossScript>().TakeDamage(damage);
            }
        }
        //wait for attack cooldown
        yield return new WaitForSeconds(attackCooldown);
        //start attack again
        StartCoroutine(AttackEnemies());
    }
}
