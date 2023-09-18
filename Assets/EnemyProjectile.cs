using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage = 20f;
    public float speed = 10f;
    public float lifetime = 5f;
    public bool homing = false;
    public float homingDistance = 2f;
    public GameObject player;
    public Rigidbody2D rb;
    public GameObject childProjectile;
    public float childProjectilesInstantiate = 0f;
    public float rotationOffset = 45f;
    public float slightHoming = 0f;
    public float slightHomingDuration = 1f;
    public float slightHomingDelay = 0.4f;
    // Start is called before the first frame update
    void Start()
    {
        //find player
        player = GameObject.Find("Player");
        //start move coroutine
        StartCoroutine(Move());
    }

    IEnumerator Move(){
        //if homing, move towards player if distance is less than homing distance. else just set rb velocity constantly using a while loop
        float timeLeft = lifetime;
        rb.velocity = transform.right * speed;
        while(timeLeft > 0){
            if(homing){
                if(Vector2.Distance(transform.position, player.transform.position) < homingDistance){
                    rb.velocity = (player.transform.position - transform.position).normalized * speed;
                }
                else{
                    rb.velocity = Vector2.zero;
                }
            }
            else{
                if(slightHoming > 0f && timeLeft > (lifetime - slightHomingDuration) && timeLeft < (lifetime - slightHomingDelay)){
                    //add rb velocity instead of setting (use vector 2)
                    Vector3 temp = (player.transform.position - transform.position).normalized * slightHoming;
                    rb.velocity += new Vector2(temp.x, temp.y);
                }
                yield return null;
            }
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        //destroy self
        Destroy(gameObject);
    }

    //on collision
    void OnTriggerEnter2D(Collider2D other){
        //if player
        if(other.gameObject.tag == "Player"){
            //deal damage to player
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
            //destroy self
            Destroy(gameObject);
        }
    }

    void OnDestroy(){
        if(childProjectilesInstantiate > 0){
            for(int i = 0; i < childProjectilesInstantiate; i++){
                //instantiate child projectiles
                GameObject newProjectile = Instantiate(childProjectile, transform.position, Quaternion.identity);
                //rotate child projectiles
                newProjectile.transform.right = Quaternion.AngleAxis(rotationOffset + (360 / childProjectilesInstantiate) * i, Vector3.forward) * transform.right;
            }
        }
    }
}
