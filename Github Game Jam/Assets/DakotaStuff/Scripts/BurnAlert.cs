using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnAlert : MonoBehaviour
{
    public List<EnemyAlert> alertedEnemies;
    public List<EnemyAlertTutorial> tutorialAlertedEnemies;
    public List<GameObject> deactivateObjects;
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
            alertedEnemies[i].Lure();
        }
        for(int i = 0; i < tutorialAlertedEnemies.Count;i++){
            tutorialAlertedEnemies[i].Lure();
        }
        for(int i = 0;i < deactivateObjects.Count;i++){
            deactivateObjects[i].SetActive(false);
        }
    }
}
