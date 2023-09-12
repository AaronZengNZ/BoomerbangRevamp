using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public Rigidbody2D rb;
    public float fling = 3f;
    public TextMeshProUGUI text;
    public float timeToFade = 0.8f;
    public float damage = 0f;
    public float originalTextSize = 400f;
    // Start is called before the first frame update
    void Start()
    {
        //set rb velocity, random y vel (only positive) and random x vel (positive and negatice)
        rb.velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized * fling;
        //start fade away
        StartCoroutine(FadeAway());
    }

    void Update(){
        //set text to damage
        text.text = damage.ToString();
        //scale text size with damage square rooted
        text.fontSize = originalTextSize * (0.8f+Mathf.Sqrt(damage/2f)/10f);
    }

    IEnumerator FadeAway(){
        yield return new WaitForSeconds(0.2f);
        //fade away
        float alpha = 1f;
        while(alpha > 0f){
            alpha -= Time.deltaTime / timeToFade;
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            yield return null;
        }
        Destroy(gameObject);
    }
}
