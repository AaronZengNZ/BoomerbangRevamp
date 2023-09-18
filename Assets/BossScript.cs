using System.Xml.Schema;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BossScript : MonoBehaviour
{
    public GameObject projectile;
    public Transform[] firingLocations;
    GameObject player;
    public Animator animator;
    public string bossType = "fractal";
    public float attackTypes = 2f;
    public float attackRandomiseTime = 5f;
    public float currentAttack = 0f;
    public float moveSpeed = 2f;
    public GameObject childEnemy;
    public float firingLocationsDestroyed = 0f;
    float prevAttack = 0f;
    public float maxHp = 2000f;
    public float hp = 2000f;
    public bool dead = false;
    public GameObject damageNumber;
    public bool touchingBoomerang = false;
    Boomerang boomerang;
    float damageTakenTemp = 0f;
    public float rollCooldown = 2f;
    public Image healthBar;
    bool invulnerable = false;
    public bool moving = false;
    float animatorSpd = 0.5f;
    Vector2 movement;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        boomerang = GameObject.Find("Boomerang").GetComponent<Boomerang>();
        StartCoroutine(FractalBossAttackRandomiser());
        if(bossType == "fractal"){
            StartCoroutine(FractalBoss());
        }
        if(bossType == "cuboid"){
            StartCoroutine(CuboidBoss());
        }
    }

    public void FireProjectile(float firingLocation){
        if(firingLocations[(int)firingLocation].GetComponent<SpriteRenderer>().color.a == 0){
            return;
        }
        GameObject tempProjectile = Instantiate(projectile, firingLocations[(int)firingLocation].position, Quaternion.identity);
        //make projectile rotate towards player
        tempProjectile.transform.right = player.transform.position - tempProjectile.transform.position;
    }

    public void DestroyFiringLocation(float firingLocation){
        // make completely transparent
        firingLocations[(int)firingLocation].GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 0);
        Instantiate(childEnemy, firingLocations[(int)firingLocation].position, Quaternion.identity);
    }
    public void TakeDamage(float damageTaken){
        if(invulnerable){
            return;
        }
        hp -= damageTaken;
        damageTakenTemp += damageTaken;
        //instantiate damage number
        if(damageTaken < boomerang.damage / 20f){
            if(damageTakenTemp >= boomerang.damage / 20f){
                GameObject tempDamageNumber = Instantiate(damageNumber, transform.position, Quaternion.identity);
                tempDamageNumber.GetComponent<DamageNumber>().damage = Mathf.Round(damageTakenTemp);
                damageTakenTemp = 0f;
            }
        }
        else{
            GameObject tempDamageNumber = Instantiate(damageNumber, transform.position, Quaternion.identity);
            tempDamageNumber.GetComponent<DamageNumber>().damage = Mathf.Round(damageTaken);
        }
        if(hp <= 0){
            Die();
        }
    }
    public void DeleteGameobject(){
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(invulnerable){
            return;
        }
        UnityEngine.Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "Boomerang"){
            touchingBoomerang = true;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag == "Boomerang"){
            touchingBoomerang = false;
        }
    }

    public void Die(){
        //disable all colliders
        foreach(Collider2D collider in GetComponents<Collider2D>()){
            collider.enabled = false;
        }
        //disable all colliders of children
        foreach(Collider2D collider in GetComponentsInChildren<Collider2D>()){
            collider.enabled = false;
        }
        dead = true;
        //activate dead bool in animator
        animator.SetBool("Dead", true);
    }

    IEnumerator FractalBoss(){
        while(true){
            //if attack type is 0, disable all animator bools
            if(currentAttack == 0f){
                animator.SetBool("Moving", false);
            }
            if(currentAttack == 1f){
                animator.SetBool("Moving", true);
                //wait for 0.35 seconds
                yield return new WaitForSeconds(0.75f);
                //move for 3 seconds
                float timeLeft = 2f;
                while(timeLeft > 0){
                    transform.position += (player.transform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
                    timeLeft -= Time.deltaTime;
                    yield return null;
                }
                //set moving to false
                animator.SetBool("Moving", false);
                //move for 0.875 seconds
                timeLeft = 1.45f;
                while(timeLeft > 0){
                    transform.position += (player.transform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
                    timeLeft -= Time.deltaTime;
                    yield return null;
                }
                currentAttack = 0f;
            }
            if(currentAttack == 2f){
                //set shoot to true, wait 2 seconds, set it to false
                animator.SetBool("Shoot", true);
                yield return new WaitForSeconds(2f);
                animator.SetBool("Shoot", false);
                currentAttack = 0f;
            }
            if(currentAttack == 3f){
                //destroy firing location
                DestroyFiringLocation(firingLocationsDestroyed);
                firingLocationsDestroyed++;
                //wait 3 seconds
                yield return new WaitForSeconds(1f);
                if(firingLocationsDestroyed >= 4f){
                    //set phase two trigger on animator
                    animator.SetTrigger("PhaseTwo");
                    //make firing locations visible
                    //wait for a second
                    yield return new WaitForSeconds(1f);
                    for(int i = 0; i < firingLocations.Length; i++){
                        firingLocations[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    }
                    firingLocationsDestroyed = 0f;
                }
                currentAttack = 0f;
            }
            yield return null;
        }
    }

    IEnumerator CuboidBoss(){
        invulnerable = true;
        //wait 4 seconds
        yield return new WaitForSeconds(3f);
        invulnerable = false;
        while(true){
            //wait roll cooldown, set roll bool to true
            while(moving){
                yield return null;
            }
            //check the player's position and roll towards it. if the player is above the boss, roll up, etc. Pick the furthest direction.
            float yDifference = Mathf.Abs(player.transform.position.y - transform.position.y);
            float xDifference = Mathf.Abs(player.transform.position.x - transform.position.x);
            if(yDifference > xDifference){
                //roll up or down
                if(player.transform.position.y > transform.position.y){
                    //roll up
                    animator.SetBool("RollUp", true);
                }
                else{
                    //roll down
                    animator.SetBool("RollDown", true);
                }
            }
            else{
                //roll left or right
                if(player.transform.position.x > transform.position.x){
                    //roll right
                    animator.SetBool("RollRight", true);
                }
                else{
                    //roll left
                    animator.SetBool("RollLeft", true);
                }
            }
            moving = true;
        }
    }

    public void SetAnimatorBool(string boolName){
        animator.SetBool(boolName, false);
        movement = Vector2.zero;
    }

    public void SetMovement(string dir){
        if(dir == "up"){
            movement = new Vector2(0f, moveSpeed);
        }
        else if(dir == "down"){
            movement = new Vector2(0f, -moveSpeed);
        }
        else if(dir == "left"){
            movement = new Vector2(-moveSpeed, 0f);
        }
        else if(dir == "right"){
            movement = new Vector2(moveSpeed, 0f);
        }
        else{
            moving = false;
            movement = Vector2.zero;
        }
    }

    IEnumerator FractalBossAttackRandomiser(){
        //wait 2 seconds
        yield return new WaitForSeconds(5f);
        invulnerable = false;
        while(true){
            //randomise attack type
            if(firingLocationsDestroyed < 2f && prevAttack != 1f){
                currentAttack = 1f;
                prevAttack = 1f;
            }
            else if(firingLocationsDestroyed >= 2f || prevAttack == 1f){
                currentAttack = Mathf.Ceil(UnityEngine.Random.Range(1f, attackTypes));
                prevAttack = currentAttack;
            }
            else{
                currentAttack = Mathf.Ceil(UnityEngine.Random.Range(0f, attackTypes));
                prevAttack = currentAttack;
            }
            yield return new WaitForSeconds(attackRandomiseTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!invulnerable){
            healthBar.fillAmount = hp / maxHp;
        }
        if(touchingBoomerang){
            //take boomerang damage damage
            TakeDamage(boomerang.damage * Time.deltaTime);
        }
        if(bossType == "cuboid"){
            if(hp <= 0){
                Die();
            }
            //scale animator speed based on hp
            animatorSpd = 1f - (hp / maxHp * 0.5f);
            animator.speed = animatorSpd;
            transform.position += new Vector3(movement.x, movement.y, 0f) * Time.deltaTime * moveSpeed * animatorSpd;
        }
        if(bossType == "fractal"){
        //flip the entire gameobject to face player
            if(player.transform.position.x > transform.position.x){
                transform.localScale = new Vector3(-1.25f, 1.25f, 1);
            }
            else{
                transform.localScale = new Vector3(1.25f, 1.25f, 1);
            }
        }
    }
}
