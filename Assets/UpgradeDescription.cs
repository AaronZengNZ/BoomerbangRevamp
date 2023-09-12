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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangeUpgradeText(string text){
        upgradeName.text = text;
    }

    // Update is called once per frame
    void Update()
    {
        //set animator bool to upgradeShowing
        upgradeAnimator.SetBool("Showing", upgradeShowing);
    }
}
