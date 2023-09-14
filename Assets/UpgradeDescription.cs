using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UpgradeDescription : MonoBehaviour
{
    public TextMeshProUGUI upgradeName;
    public bool upgradeShowing = false;
    public Animator upgradeAnimator;
    public TextMeshProUGUI addEnemyText;
    public GameObject addEnemyHolder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangeUpgradeText(string text, bool addEnemy = false){
        upgradeName.text = text;
        addEnemyHolder.SetActive(addEnemy);
    }

    public void ChangeEnemyText(string text){
        addEnemyText.text = text;
    }

    // Update is called once per frame
    void Update()
    {
        //set animator bool to upgradeShowing
        upgradeAnimator.SetBool("Showing", upgradeShowing);
    }
}
