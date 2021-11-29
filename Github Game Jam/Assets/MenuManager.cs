using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{

    public GameObject menuButtons;
    public GameObject illustration1;
    public GameObject illustration2;
    //Athena Stuff
    public bool videoPlayed;
    public GameObject mainMenu;
    public GameObject videoTexture;

    // Start is called before the first frame update
    void Start()
    {
        //Athena Stuff
        /*if (videoPlayed)
        {
            menuButtons.SetActive(true);
            mainMenu.SetActive(true);
            videoTexture.SetActive(false);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateObject(GameObject objectToActivate){
        objectToActivate.SetActive(true);
        menuButtons.SetActive(false);
    }

    public void DeactivateObject(GameObject objectToDeactivate){
        // objectToDeactivate.SetActive(false);
        // Debug.Log("myself");
    }

    public void SwapMainMenuIllustrations(){
        illustration1.SetActive(!illustration1.activeSelf);
        illustration2.SetActive(!illustration2.activeSelf);
    }

    public void SetTextColorWhite(TextMeshProUGUI textMesh){
        textMesh.color = Color.white;
    }

    public void SetTextColorBlack(TextMeshProUGUI textMesh){
        textMesh.color = Color.black;
    }
}
