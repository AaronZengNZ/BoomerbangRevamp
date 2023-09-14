using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrade : MonoBehaviour
{
    //on destroy, destroy all other upgrades
    public string upgrade;
    public float amount;
    public string type = "add";
    public string upgrade2;
    public float amount2;
    public string type2;
    public bool twoUpgrades = false;
    public string downgrade;
    public float downgradeAmount;
    public string downgradeType;
    public bool hasDowngrade = false;
    public UpgradeManager upgradeManager;
    public bool touchingPlayer = false;
    public float fillAmount = 0f;
    public Image fillImage;
    public string description = "placeholder";
    public UpgradeDescription upgradeDescription;
    public float enemyType = 0f;
    public Sprite[] enemyTypeSprites;
    public SpriteRenderer enemyTypeSpriteRenderer;
    public GameObject enemyTypeShowerParent;
    public EnemySpawner enemySpawner;
    public bool isEnemyUnlocker = false;
    public string upgradeName = "null";
    void Start(){
        upgradeManager = GameObject.Find("UpgradeManager").GetComponent<UpgradeManager>();
        upgradeDescription = GameObject.Find("UpgradeDescription").GetComponent<UpgradeDescription>();
        enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
    }
    void OnDestroy(){
        foreach(GameObject upgrade in GameObject.FindGameObjectsWithTag("Upgrade")){
            Destroy(upgrade);
        }
    }

    void Update(){
        if(touchingPlayer){
            fillAmount += Time.deltaTime /3f;
            fillImage.fillAmount = fillAmount;
            if(fillAmount >= 1f){
                TakeUpgrade();
            }
        }
        else{
            fillAmount = 0f;
            fillImage.fillAmount = fillAmount;
        }
        if(isEnemyUnlocker){
            if(enemyType <= 0f){
                enemyType = 1f;
            }
                enemyTypeSpriteRenderer.sprite = enemyTypeSprites[(int)enemyType-1];
                enemyTypeShowerParent.SetActive(true);
        }
    }

    //on trigger enter
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            touchingPlayer = true;
            upgradeDescription.ChangeUpgradeText(description);
            upgradeDescription.upgradeShowing = true;
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            touchingPlayer = false;
            upgradeDescription.upgradeShowing = false;
        }
    }

    public void TakeUpgrade(){
        //destroy
        //add upgrade
        if(enemyType != 0f){
            enemySpawner.AddEnemySpawner(enemyType);
        }
        upgradeManager.IncreaseStat(upgrade, amount, type, true);
        if(twoUpgrades){
            upgradeManager.IncreaseStat(upgrade2, amount2, type2);
        }
        if(hasDowngrade){
            upgradeManager.IncreaseStat(downgrade, downgradeAmount, downgradeType);
        }
        Destroy(gameObject);

    }
}
