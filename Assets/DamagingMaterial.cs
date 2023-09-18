using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingMaterial : MonoBehaviour
{
    public float damage = 20f;
    public float constantDamage = 10f;
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }
    void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            other.gameObject.GetComponent<Player>().TakeDamage(constantDamage * Time.deltaTime);
        }
    }
}
