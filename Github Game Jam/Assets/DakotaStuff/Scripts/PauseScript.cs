using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public bool paused;
    public GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(paused){
                Time.timeScale = 1f;
                paused = false;
                menu.SetActive(false);
                Cursor.visible = false;
            } else{
                Time.timeScale = 0f;
                paused = true;
                menu.SetActive(true);
                Cursor.visible = true;
            }
            
        }
    }
}
