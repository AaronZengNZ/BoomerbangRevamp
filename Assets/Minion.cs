using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public float damage = 30f;
    public float attackCooldown = 0.4f;
    public float attackRange = 0.5f;
    public float moveSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        //start attack
        StartCoroutine(AttackEnemies());
    }

    void Update(){
        //move towards player
        transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position, moveSpeed * Time.deltaTime);
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
        //wait for attack cooldown
        yield return new WaitForSeconds(attackCooldown);
        //start attack again
        StartCoroutine(AttackEnemies());
    }
}
