using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeScreen : MonoBehaviour
{
    public Image overlayImage;
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
            Color overlayColor = overlayImage.color;
            overlayColor.a = Mathf.Lerp(0,1,lerp);
            overlayImage.color = overlayColor;
            yield return null;
        }
        
    }

    public IEnumerator FadeInRoutine(){
        float lerp = 0f;
        while(lerp < 1f){
            lerp += Time.deltaTime;
            Color overlayColor = overlayImage.color;
            overlayColor.a = Mathf.Lerp(1,0,lerp);
            overlayImage.color = overlayColor;
            yield return null;
        }
        
    }
}
