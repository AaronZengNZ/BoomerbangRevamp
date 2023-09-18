using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 3f;
    public float projectileDamage = 10f;
    public Rigidbody2D projectileRB;
    public float projectileLifetime = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //on trigger enter
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Enemy"){
            other.gameObject.GetComponent<Enemy>().TakeDamage(projectileDamage);
            Destroy(gameObject);
        }
        //also check for enemyboss
        if(other.gameObject.tag == "EnemyBoss"){
            other.gameObject.GetComponent<BossScript>().TakeDamage(projectileDamage);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //move
        projectileRB.velocity = transform.up * projectileSpeed;
        //destroy after lifetime
        projectileLifetime -= Time.deltaTime;
        if(projectileLifetime <= 0){
            Destroy(gameObject);
        }
    }
}
