using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogItemTutorialExtras : MonoBehaviour
{
    public EnemyAlertTutorial enemyAlertTutorial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FinishWakeUp(){
        enemyAlertTutorial.FinishWakeUp();
    }
}
