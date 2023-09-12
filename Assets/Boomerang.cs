using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject sprite;
    public Vector3 mousePosition;
    public Transform playerPos;
    public bool inHand = true;
    public float turnAround = 0f;
    public float velocity = 0f;
    public float direction = 0;
    public float throwVelocity = 10f;
    public float returnVelocity = 5f;
    public float mouseHomingAmount = 2f;
    public Vector2 destination;
    public bool returning = false;
    public ParticleSystem explosion;
    bool exploded = false;
    public float explosionRadius = 3f;
    public float explosionDamage = 50f;
    public float damage = 10f;
    public float boomerangSize = 1f;
    public GameObject bees;
    public bool shootBees = false;
    public GameObject flowerSprite;
    float beesCooldown = 0f;
    bool throwable = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player"){
            if(inHand == false){
                inHand = true;
            }
        }
    }

    void Bees(){
        if(shootBees){
            flowerSprite.SetActive(true);
        }
        else{
            flowerSprite.SetActive(false);
        }
        if(shootBees && inHand == false){
            beesCooldown -= Time.deltaTime;
            if(beesCooldown <= 0){
                GameObject newBee = Instantiate(bees, transform.position, Quaternion.identity);
                //rotate bees randomly
                newBee.transform.Rotate(0f, 0f, Random.Range(0f, 360f));
                beesCooldown = 0.1f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //update boomerang size
        sprite.transform.localScale = new Vector3(boomerangSize, boomerangSize, boomerangSize);
        //update collider
        GetComponent<CircleCollider2D>().radius = boomerangSize / 2;
        sprite.SetActive(!exploded && !inHand);
        if(inHand){
            rb.velocity = Vector2.zero;
            velocity = 0f;
            transform.position = playerPos.position;
        }
        Bees();
        Explode();
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        if (Input.GetMouseButtonDown(0) && inHand)
        {
            Throw();
        }
        else
        {
            //set rigidbody velocity using direciton
            rb.velocity = new Vector2(Mathf.Cos(direction * Mathf.Deg2Rad) * velocity, Mathf.Sin(direction * Mathf.Deg2Rad) * velocity);
            velocity -= returnVelocity * Time.deltaTime;
            //if velocity is less than 0 add 180 to direction
            if (velocity <= 0 && returning == false)
            {
                returning = true;
            }
            if(returning){
                //make direction point at player
                direction = Mathf.Atan2(playerPos.position.y - transform.position.y, playerPos.position.x - transform.position.x) * Mathf.Rad2Deg;
                direction += 180;
            }
            else{
                //slowly adjust direction to point at mouse at the rate of mouseHomingAmount
                direction = Mathf.LerpAngle(direction, Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg, mouseHomingAmount * Time.deltaTime);
            }
        }
    }

    public void Explode(){
        //if right mouse button is held
        if(Input.GetMouseButton(1) == false){
            return;
        }
        if(exploded){
            return;
        }
        exploded = true;
        //instantiate an explosion
        ParticleSystem explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);
        //deal damage to every enemy in radius
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach(Collider2D enemy in enemies){
            if(enemy.gameObject.tag == "Enemy"){
                enemy.gameObject.GetComponent<Enemy>().TakeDamage(explosionDamage);
            }
        }
        //if the player is in radius also stun player
        Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach(Collider2D player in players){
            if(player.gameObject.tag == "Player"){
                player.gameObject.GetComponent<Player>().Stun();
                throwable = false;
                StartCoroutine(waitAndThrowable(1.5f));
                transform.position = playerPos.position;
                return;
            }
        }
        //if upgrade is in radius take upgrade
        
        throwable = false;
        StartCoroutine(waitAndThrowable(0.4f));
        //tp to player
        transform.position = playerPos.position;
    }

    IEnumerator waitAndThrowable(float time){
        yield return new WaitForSeconds(time);
        throwable = true;
    }

    public void Throw()
    {
        if(throwable == false){
            return;
        }
        //return if distance to mouse is less than 0.5
        if (Vector2.Distance(transform.position, mousePosition) < 1.2f)
        {
            return;
        }
        inHand = false;
        returning = false;
        exploded = false;
        //set velocity to throw velocity multiplied by distance to mouse square rooted
        velocity = throwVelocity * Mathf.Sqrt(Vector2.Distance(transform.position, mousePosition));
        direction = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;
    }
}
