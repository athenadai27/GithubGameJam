using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInactive : MonoBehaviour
{
    public void Deactivate(){
        gameObject.SetActive(false);
    }
}
