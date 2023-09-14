using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Animator tutorialAnimator;
    public float stage = 0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckOne());
    }

    IEnumerator CheckOne(){
        //wait until wasd or arrow keys are pressed for more than a total of 1 second
        float timePressed = 0f;
        while(timePressed < 0.3f){
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow)){
                timePressed += Time.deltaTime;
            }else{
                timePressed = 0f;
            }
            yield return null;
        }
        //set 1 trigger
        tutorialAnimator.SetTrigger("1");
        stage++;
    }
    public void TwoChecked(){
        if(stage == 1f){
            //set 2 trigger
            tutorialAnimator.SetTrigger("2");
            stage++;
        }
    }
    public void ThreeChecked(){
        if(stage == 2f){
            //set 3 trigger
            tutorialAnimator.SetTrigger("3");
            stage++;
        }
    }
    public void FourChecked(){
        if(stage == 3f){
            //set 4 trigger
            tutorialAnimator.SetTrigger("4");
            stage++;
        }
    }
}
