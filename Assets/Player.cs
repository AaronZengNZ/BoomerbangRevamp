using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D playerRB;
    public float speed;
    public float maxVelocity = 10f;
    public Vector2 localMovement;
    public GameObject sprite;
    public GameObject damageSprite;
    public Animator animator;
    bool stunned = false;
    public float spdMulti = 1f;
    public bool looping = false;
    public GameObject walls;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal") * speed * spdMulti, Input.GetAxis("Vertical") * speed * spdMulti);
        playerRB.velocity = movement;
        if(stunned){
            playerRB.velocity = Vector2.zero;
        }
        FlipSprites();
        UpdateAnimatorParameters();
        LoopScreen();
        SetActiveWalls();
    }

    void SetActiveWalls(){
        if(looping){
            walls.SetActive(false);
        }
        else{
            walls.SetActive(true);
        }
    }

    void LoopScreen(){
        //loop around the screen
        if(looping){
            if(transform.position.x > 8.9f){
                transform.position = new Vector3(-8.8f, transform.position.y, transform.position.z);
            }
            else if(transform.position.x < -8.9f){
                transform.position = new Vector3(8.8f, transform.position.y, transform.position.z);
            }
            if(transform.position.y > 5){
                transform.position = new Vector3(transform.position.x, -4.9f, transform.position.z);
            }
            else if(transform.position.y < -5){
                transform.position = new Vector3(transform.position.x, 4.9f, transform.position.z);
            }
        }
    }

    void UpdateAnimatorParameters(){
        //set bool moving to true if velocity is greater than 0
        animator.SetBool("Moving", playerRB.velocity.magnitude > 0);
    }

    public void Stun(){
        StartCoroutine(EnumStun());
    }

    IEnumerator EnumStun(){
        stunned = true;
        //make the damagesprite flash in and outS
        for(int i = 0; i < 3; i++){
            damageSprite.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            damageSprite.SetActive(false);
            yield return new WaitForSeconds(0.25f);
        }
        stunned = false;
    }

    void FlipSprites(){
        if(playerRB.velocity.x > 0){
            sprite.transform.localScale = new Vector3(1, 1, 1);
        }
        else if(playerRB.velocity.x < 0){
            sprite.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}