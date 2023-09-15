using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Player : MonoBehaviour
{
    public Rigidbody2D playerRB;
    public float speed;
    public float maxVelocity = 10f;
    public Vector2 localMovement;
    public GameObject sprite;
    public GameObject damageSprite;
    public GameObject stunSprite;
    public Animator animator;
    bool stunned = false;
    public float spdMulti = 1f;
    public bool looping = false;
    public GameObject walls;
    public Image HealthBar;
    public float hp = 100f;
    public float maxHp = 100f;
    public float regenSpd = 10f;
    public TextMeshProUGUI speedText;
    public GameObject gameOverCanvas;
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
        SetUI();
        Heal();
    }
    public void TakeDamage(float damage){
        hp -= damage;
        if(hp <= 0){
            Die();
        }
    }
    void Heal(){
        //set damageSprite's opacity based on hp 
        damageSprite.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 2 - (hp / maxHp * 2f));
        if(hp < maxHp){
            hp += Time.deltaTime * regenSpd;
        }
    }
    void Die(){
        //destroy player, boomerang, and deal 9999 damage to all enemies
        Destroy(GameObject.Find("Boomerang"));
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")){
            enemy.GetComponent<Enemy>().TakeDamage(999f);
        }
        //find enemyspawner, upgrade manager and set both of them inactive
        GameObject.Find("EnemySpawner").SetActive(false);
        GameObject.Find("UpgradeManager").SetActive(false);
        //activate gameovercanvas
        gameOverCanvas.SetActive(true);
        //set gameOverCanvas's animator's transition bool
        gameOverCanvas.GetComponent<Animator>().SetBool("Transition", true);
        Destroy(gameObject);
    }

    void SetUI(){
        //set health bar sprite's fill
        HealthBar.fillAmount = hp / maxHp;
        //set speed text
        speedText.text = (spdMulti*speed).ToString("F0") + " SPD";
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
            stunSprite.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            stunSprite.SetActive(false);
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