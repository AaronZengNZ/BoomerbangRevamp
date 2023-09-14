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
    float maxHp = 0f;
    float speedMulti = 1f;
    float spriteSize = 1f;
    // Start is called before the first frame update
    void Start()
    {
        maxHp = hp;
        //find player
        player = GameObject.Find("Player");
        if(!spawned){
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn(){
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
        //set rb velocity
        rb.velocity = (player.transform.position - transform.position).normalized * speed * speedMulti;
        if(specialStat == "wobbler"){
            //add math sin and math cos to rb velocity
            rb.velocity += new Vector2(Mathf.Sin(Time.time * 4f)*5f, Mathf.Cos(Time.time * 4f)*5f);
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
        Destroy(gameObject);
    }

    public void TakeDamage(){
        //find boomerang damage from boomerang
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
            boomerangsTouching++;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag == "Boomerang"){
            boomerangsTouching--;
        }
    }
}
