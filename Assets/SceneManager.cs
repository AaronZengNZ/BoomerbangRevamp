using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneManager : MonoBehaviour
{
    public Animator slideTransition;
    public GameObject slideTransitionGameobject;
    public bool loading = false;

    void Start(){
        //StartCoroutine(DisableSlideTransition());
    }
    public void LoadScene(string sceneName){
        if(loading){
            return;
        }
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }
    public void LoadFirstScene(){
        UnityEngine.Debug.Log("load attempt");
        if(loading){
            return;
        }
        StartCoroutine(LoadSceneNumberCoroutine(0));
    }
    public void ReloadScene(){
        if(loading){
            return;
        }
        StartCoroutine(LoadSceneNumberCoroutine(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator DisableSlideTransition(){
        yield return new WaitForSeconds(1f);
        slideTransitionGameobject.SetActive(false);
    }

    IEnumerator LoadSceneCoroutine(string sceneName){
        loading = true;
        slideTransitionGameobject.SetActive(true);
        //set bool
        slideTransition.SetBool("Start", true);
        yield return new WaitForSeconds(4f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadSceneNumberCoroutine(float number){
        loading = true;
        slideTransitionGameobject.SetActive(true);
        //set bool
        slideTransition.SetBool("Start", true);
        yield return new WaitForSeconds(4f);
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)number);
    }
}
