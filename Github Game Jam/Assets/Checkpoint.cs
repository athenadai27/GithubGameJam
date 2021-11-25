using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Checkpoint : MonoBehaviour
{
    public Transform playerTransform;
    public Transform frogMinibossTransform;
    public PlayerController playerController;
    public Vector3 playerPosition;
    public FrogMiniboss frogMiniboss;
    public Vector3 frogMinibossPosition;
    public Image overlayImage;
    // Start is called before the first frame update
    void Start()
    {
        frogMinibossPosition = frogMinibossTransform.position;
        playerPosition = playerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset(){
        StartCoroutine(ResetRoutine());
    }

    IEnumerator ResetRoutine(){
        Debug.Log("reset");
        float lerp = 0f;
        while(lerp < 1f){
            lerp += Time.deltaTime;
            Color overlayColor = overlayImage.color;
            overlayColor.a = Mathf.Lerp(0,1,lerp);
            overlayImage.color = overlayColor;
            yield return null;
        }
        playerController.Reset();
        frogMiniboss.Reset();
        playerTransform.position = playerPosition;
        frogMinibossTransform.position = frogMinibossPosition;
        frogMinibossTransform.localScale = Vector3.one;
        lerp = 0f;
        Debug.Log("Here");
        while(lerp < 1f){
            lerp += Time.deltaTime;
            Color overlayColor = overlayImage.color;
            overlayColor.a = Mathf.Lerp(1,0,lerp);
             overlayImage.color = overlayColor;
            yield return null;
        }
    }
}
