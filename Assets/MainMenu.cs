using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject tutorialChecker;
    public GameObject[] classes;
    public void EnableTutorialChecker(){
        tutorialChecker.SetActive(true);
    }
    public void EnableClass(float classNum){
        classes[(int)classNum].SetActive(true);
        //disable all others
        for(int i = 0; i < classes.Length; i++){
            if(i != (int)classNum){
                classes[i].SetActive(false);
            }
        }
    }

}

