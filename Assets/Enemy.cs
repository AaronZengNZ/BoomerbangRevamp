using System.Security.Cryptography;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public CircleCollider2D collider;
    public Rigidbody2D rb;
    public float hp = 100f;
    float boomerangsTouching = 0f;
    GameObject player;
    public float speed = 1f;
    public GameObject sprite;
    public GameObject damageSprite;
    public GameObject warningSprite;
    public GameObject gameSprite;
    public float warningTime = 2f;
    bool spawned = false;
    float boomerangDamageTaken = 0f;
    float boomerangTimeTaken = 0f;
    public GameObject damageNumber;
    public GameObject childEnemy;
    public bool spawnsChild = false;
    public string specialStat = "none";
    public float damage = 20f;
    Vector2 randomTargetingOffset;
    Vector2 offsettedPlayer;
    public float offsetSize = 1.5f;
    float maxHp = 0f;
    float speedMulti = 1f;
    float spriteSize = 1f;
    bool touchingPlayer = false;
    public bool tutorialTarget = false;
    public Tutorial tutorialScript;
    public GameObject projectile;
    public float firerate = 0.4f;
    public Transform shootPosition;
    public bool shootsProjectiles = false;
    public bool instaSpawn = false;
    float damaget = 0f;
    // Start is called before the first frame update
    void Start()
    {
        maxHp = hp;
        //find player
        player = GameObject.Find("Player");
        if(!spawned){
            StartCoroutine(Spawn());
        }
        //randomise random offset
        randomTargetingOffset = new Vector2(Random.Range(-offsetSize, offsetSize), Random.Range(-offsetSize, offsetSize));
    }

    IEnumerator Spawn(){
        if(instaSpawn){
            collider.enabled = true;
            rb.simulated = true;
            spawned = true;
            //enable warning sprite
            warningSprite.SetActive(false);
            gameSprite.SetActive(true);
            //start shoot coroutine
            if(shootsProjectiles){
            StartCoroutine(Shoot());
            }
            //break
            yield break;
        }
        gameSprite.SetActive(false);
        warningSprite.SetActive(true);
        //disable collider and rb
        collider.enabled = false;
        rb.simulated = false;
        //wait for warning time
        yield return new WaitForSeconds(warningTime);
        //enable collider and rb
        collider.enabled = true;
        rb.simulated = true;
        spawned = true;
        //enable warning sprite
        warningSprite.SetActive(false);
        gameSprite.SetActive(true);
        //start shoot coroutine
        if(shootsProjectiles){
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot(){
        while(true){
            yield return new WaitForSeconds(1f / firerate * Random.Range(0.8f, 1.2f));
            //spawn projectile
            GameObject newProjectile = Instantiate(projectile, shootPosition.position, Quaternion.identity);
            //rotate projectile towards player
            newProjectile.transform.right = (player.transform.position - transform.position).normalized;  
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!spawned){
            return;
        }
        if(boomerangsTouching > 0f){
            TakeDamage();
        }
        if(hp <= 0){
            Die();
        }
        if(touchingPlayer){
            player.GetComponent<Player>().TakeDamage(damage * Time.deltaTime);
        }
        offsettedPlayer = new Vector2(player.transform.position.x + randomTargetingOffset.x, player.transform.position.y + randomTargetingOffset.y);
        //if distance to player is less than random offset squared
        if(Vector2.Distance(transform.position, player.transform.position) < randomTargetingOffset.sqrMagnitude + 1){
            if(!shootsProjectiles){
                rb.velocity = (player.transform.position - transform.position).normalized * speed * speedMulti;
            }
            else{
                rb.velocity = (offsettedPlayer - (Vector2)transform.position).normalized * speed * speedMulti;
            }
        }
        else{
            //set rb velocity
            rb.velocity = (offsettedPlayer - (Vector2)transform.position).normalized * speed * speedMulti;
        }
        
        if(specialStat == "wobbler"){
            //add math sin and math cos to rb velocity
            rb.velocity += new Vector2(Mathf.Sin(Time.time * 4f)*2f, Mathf.Cos(Time.time * 4f)*2f);
        }
        if(specialStat == "slime"){
            //game gamesprite size based on hp
            spriteSize = hp/maxHp*1.2f+0.3f;
            speedMulti = 3f/(hp/maxHp*2f+1f);
        }
        //if taking damage, enable damage sprite
        damageSprite.SetActive(boomerangsTouching > 0);
        FlipSprites();
    }

    void Die(){
        if(spawnsChild){
            Instantiate(childEnemy, transform.position, Quaternion.identity);
        }
        if(specialStat == "exploder"){
            //spawn 5 projectiles at random angles
            float tempRandom = Random.Range(0f, 100f)/100f;
            for(int i = 0; i < 9; i++){
                GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
                newProjectile.transform.right = new Vector2(Mathf.Cos((i+tempRandom) * 40f * Mathf.Deg2Rad), Mathf.Sin((i+tempRandom) * 40f * Mathf.Deg2Rad));
            }
        }
        Destroy(gameObject);
    }

    public void TakeDamage(){
        //find boomerang damage from boomerang
        if(tutorialTarget){
            damaget += Time.deltaTime;
            if(damaget >= 0.3f){
                tutorialScript.TwoChecked();
            }
        }
        float boomerangDamage = GameObject.Find("Boomerang").GetComponent<Boomerang>().damage;
        hp -= boomerangsTouching * boomerangDamage * Time.deltaTime;
        boomerangDamageTaken += boomerangsTouching * boomerangDamage * Time.deltaTime;
        boomerangTimeTaken += Time.deltaTime;
        if(boomerangTimeTaken >= 0.05f){
            //spawn damage number
            GameObject newDamageNumber = Instantiate(damageNumber, transform.position, Quaternion.identity);
            newDamageNumber.GetComponent<DamageNumber>().damage = Mathf.Round(boomerangDamageTaken);
            boomerangDamageTaken = 0f;
            boomerangTimeTaken = 0f;
        }
    }
    public void TakeDamage(float damage){
        if(damage == 999f){
            //destroy gameobject
            Destroy(gameObject);
        }
        if(tutorialTarget){
            if(damage >= 50f){
                tutorialScript.ThreeChecked();
            }
        }
        hp -= damage;
        //spawn damage number
        GameObject newDamageNumber = Instantiate(damageNumber, transform.position, Quaternion.identity);
        newDamageNumber.GetComponent<DamageNumber>().damage = damage;
    }

    void FlipSprites(){
        //flip sprites based on velocity
        if(rb.velocity.x > 0){
            sprite.transform.localScale = new Vector3(-1 * spriteSize, 1 * spriteSize, 1);
        }
        else if(rb.velocity.x < 0){
            sprite.transform.localScale = new Vector3(1 * spriteSize, 1 * spriteSize, 1);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Boomerang"){
            //if boomerang is not damaging
            if(other.gameObject.GetComponent<Boomerang>().damaging){
                boomerangsTouching++;
            }
        }
        if(other.gameObject.tag == "Player"){
            touchingPlayer = true;
            //deal a burst of 20% of dps
            player.GetComponent<Player>().TakeDamage(damage * 0.2f);
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag == "Boomerang"){
            if(boomerangsTouching > 0f){
                boomerangsTouching--;
            }
        }
        if(other.gameObject.tag == "Player"){
            touchingPlayer = false;
        }
    }
}
