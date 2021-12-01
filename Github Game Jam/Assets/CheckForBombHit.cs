using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForBombHit : MonoBehaviour
{
    public GameObject poof;
    public GameObject garbaraAngryRadius;
    public GameObject waterHoseOn;
    public GameObject waterHoseOff;
    public FrogMiniboss garbara;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BombHit(){
        Debug.Log("bomb hit");
        Instantiate(poof,transform.position,poof.transform.rotation);
        garbaraAngryRadius.SetActive(true);
        gameObject.SetActive(false);
        waterHoseOn.SetActive(false);
        waterHoseOff.SetActive(true);
        garbara.bossState = FrogMiniboss.BossStates.pissed;
    }
}
