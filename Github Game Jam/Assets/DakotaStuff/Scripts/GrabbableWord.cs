using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableWord : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToSpawn;

    [SerializeField]
    private StemController stemController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GrabItem(Transform parentTransform){
      transform.parent.SetParent(parentTransform);
      stemController.grabbedWord = this;
      //  GameObject newObject = Instantiate(objectToSpawn,parentTransform.position,objectToSpawn.transform.rotation,parentTransform);
       // gameObject.SetActive(false);
    }

    public void SpawnItem(){
        GameObject newObject = Instantiate(objectToSpawn,transform.parent.parent.position,objectToSpawn.transform.rotation,transform.parent.parent);
        gameObject.SetActive(false);
    }
}
