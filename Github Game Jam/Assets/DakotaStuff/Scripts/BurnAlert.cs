using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnAlert : MonoBehaviour
{
    public List<EnemyAlert> alertedEnemies;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Burn(){
        for(int i = 0; i < alertedEnemies.Count;i++){
            alertedEnemies[i].SetThreatLevel(2);
        }
    }
}
