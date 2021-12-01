using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToAndDisappear : MonoBehaviour
{
    public Transform goToTransform;
    public Transform moveTransform;
    public float lerp;
    public Vector3 startPos;
    public Vector3 endPos;
    public float moveInSeconds;
    public bool facingLeft;
    public List<GameObject> objectsToActivate;
    public List<GameObject> objectsToDeactivate;
    public Animator frogAnim;
    // Start is called before the first frame update
    void OnEnable()
    {
        startPos = moveTransform.position;
        endPos = goToTransform.position;
        lerp = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        lerp += Time.deltaTime/moveInSeconds;
        Vector3 nextPos = Vector3.Lerp(startPos,endPos,lerp);
        if(!frogAnim.GetBool("Walking")){
            frogAnim.SetBool("Walking",true);
        }
        if(facingLeft){
            if(moveTransform.position.x > nextPos.x){
                moveTransform.localScale = Vector3.one;
            } else if(moveTransform.position.x < nextPos.x){
                moveTransform.localScale = new Vector3(-1,1,1);
            }
        } else{
            if(moveTransform.position.x > nextPos.x){
                moveTransform.localScale = new Vector3(-1,1,1);
            } else if(moveTransform.position.x < nextPos.x){
                moveTransform.localScale = Vector3.one;
            }
        }
        moveTransform.position = nextPos;
        if(lerp >= 1){
            for(int i = 0; i < objectsToActivate.Count;i++){
                objectsToActivate[i].SetActive(true);
            }
             for(int i = 0; i < objectsToDeactivate.Count;i++){
                objectsToDeactivate[i].SetActive(false);
            }
        }
    }
}
