using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTongueTrigger : MonoBehaviour
{
    public HoujeilahCaptureTongue tutorialTongue;
    // Start is called before the first frame update
    void OnDisable()
    {
        tutorialTongue.Retract();
    }


}
