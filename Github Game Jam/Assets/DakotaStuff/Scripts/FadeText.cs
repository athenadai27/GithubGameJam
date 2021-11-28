using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FadeText : MonoBehaviour
{
    public TextMeshProUGUI textToFade;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeIn(){
        
    }
    public IEnumerator FadeOutRoutine(){
        float lerp = 0f;
        while(lerp < 1f){
            lerp += Time.deltaTime;
            Color textColor = textToFade.color;
            textColor.a = Mathf.Lerp(0,1,lerp);
            textToFade.color = textColor;
            yield return null;
        }
        
    }

    public IEnumerator FadeInRoutine(){
        float lerp = 0f;
        while(lerp < 1f){
            lerp += Time.deltaTime;
            Color textColor = textToFade.color;
            textColor.a = Mathf.Lerp(1,0,lerp);
            textToFade.color = textColor;
            yield return null;
        }
        
    }
}
