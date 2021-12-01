using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHoujeilahOnDisable : MonoBehaviour
{
    public Transform travelTransform;
    public HoujeilahScript houjeilah;
    // Start is called before the first frame update
    void OnDisable()
    {
        houjeilah.GoToPosition(travelTransform.position,0f);
    }

}
