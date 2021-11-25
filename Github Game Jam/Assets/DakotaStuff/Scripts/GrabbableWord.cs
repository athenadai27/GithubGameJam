using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableWord : MonoBehaviour
{
    public GameObject objectToSpawn;

    public StemController stemController;
    Transform parentTransform;
    // Start is called before the first frame update
    void Start()
    {
        parentTransform = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GrabItem(Transform parentTransform){
      transform.SetParent(parentTransform);
      stemController.grabbedWord = this;
      //  GameObject newObject = Instantiate(objectToSpawn,parentTransform.position,objectToSpawn.transform.rotation,parentTransform);
       // gameObject.SetActive(false);
    }

    public GameObject SpawnItem(){
        GameObject newObject = Instantiate(objectToSpawn,transform.parent.parent.position,objectToSpawn.transform.rotation,transform.parent.parent);
        return newObject;
    }

    public void Reset(){
        
        transform.SetParent(parentTransform);
        transform.position = parentTransform.position;
        
        transform.parent.gameObject.SetActive(false);
    }
}
