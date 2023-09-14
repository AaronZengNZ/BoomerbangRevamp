using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject tutorialChecker;
    public void EnableTutorialChecker(){
        tutorialChecker.SetActive(true);
    }
}
