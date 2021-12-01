using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleScript : MonoBehaviour
{
    public float shrinkSpeed;
    public float startShrinkSpeed;
    public BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        startShrinkSpeed = shrinkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x <= 0){
            Destroy(this.gameObject);
        } else{
            transform.localScale -= Vector3.one*Time.deltaTime*shrinkSpeed;
        }
        if(transform.localScale.x <= .4f){
            boxCollider.enabled = false;
        }
    }
}
