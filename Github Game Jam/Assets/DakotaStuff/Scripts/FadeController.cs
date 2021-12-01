using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    public FadeScreen fadeScreen;
    public FadeText fadeText;
    public float waitForFadeTime;
    public bool fading;
    public GameObject firstTextTrigger;
    // Start is called before the first frame update
    void Start()
    {
        waitForFadeTime = Time.time + 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > waitForFadeTime && !fading){
            StartCoroutine(fadeScreen.FadeInRoutine());
            StartCoroutine(fadeText.FadeInRoutine());
            fading = true;
        } else if(fading){
            if(fadeText.textToFade.color.a == 0f){
                firstTextTrigger.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}
