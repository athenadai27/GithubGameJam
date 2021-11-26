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
    public FrogKingScript frogKing;
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
         playerController.Reset();
        if(frogMiniboss){
            frogMiniboss.Reset();
        }
        if(frogKing){
            frogKing.Reset();
        }
        
        
        playerTransform.position = playerPosition;
        frogMinibossTransform.position = frogMinibossPosition;
        frogMinibossTransform.localScale = Vector3.one;
        // lerp = 0f;
        Debug.Log("Here");
        ItemScript[] items = FindObjectsOfType<ItemScript>();
        for(int i = 0; i < items.Length;i++){
            items[i].Break();
        }
        //StartCoroutine(ResetRoutine());
    }

    // IEnumerator ResetRoutine(){
    //     // Debug.Log("reset");
    //     // float lerp = 0f;
    //     // while(lerp < 1f){
    //     //     lerp += Time.deltaTime;
    //     //     Color overlayColor = overlayImage.color;
    //     //     overlayColor.a = Mathf.Lerp(0,1,lerp);
    //     //     overlayImage.color = overlayColor;
    //     //     yield return null;
    //     // }
    //     playerController.Reset();
    //     if(frogMiniboss){
    //         frogMiniboss.Reset();
    //     }
    //     if(frogKing){
    //         frogKing.Reset();
    //     }
        
        
    //     playerTransform.position = playerPosition;
    //     frogMinibossTransform.position = frogMinibossPosition;
    //     frogMinibossTransform.localScale = Vector3.one;
    //     // lerp = 0f;
    //     Debug.Log("Here");
    //     ItemScript[] items = FindObjectsOfType<ItemScript>();
    //     for(int i = 0; i < items.Length;i++){
    //         items[i].Break();
    //     }
    //     // while(lerp < 1f){
    //     //     lerp += Time.deltaTime;
    //     //     Color overlayColor = overlayImage.color;
    //     //     overlayColor.a = Mathf.Lerp(1,0,lerp);
    //     //      overlayImage.color = overlayColor;
    //     //     yield return null;
    //     // }
    // }
}
