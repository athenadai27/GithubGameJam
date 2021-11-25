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
    IEnumerator FadeRoutine(){
        float lerp = 0f;
        while(lerp != 1f){
            Color overlayColor = overlayImage.color;
            overlayColor.a = Mathf.Lerp(1,0,lerp);
            yield return null;
        }
        
    }
}
