using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
public class MainMenuVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public bool fadeInImage;
    public Image mainMenuImage;
    public GameObject menuButtons;
    //Athena Stuff
    public bool videoPlayed = false;
    public GameObject mainMenu;
    public GameObject videoTexture;
    public bool madeThingsAppear;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (fadeInImage)
        {
            if (videoPlayer.frame == (long)videoPlayer.frameCount - 1)
            {
                fadeInImage = false;
                StartCoroutine("FadeInImage");

            }
        }
        // else //Athena Stuff
        // {
            
        //     menuButtons.SetActive(true);
        //     mainMenu.SetActive(true);
        //     videoTexture.SetActive(false);
        // }

    }

    IEnumerator FadeInImage()
    {
        float fadeStep = 0;
        while (fadeStep < 1)
        {
            fadeStep += Time.deltaTime;
            Color newColor = mainMenuImage.color;
            newColor.a = Mathf.Lerp(0, 1, fadeStep);
            mainMenuImage.color = newColor;
            yield return null;
        }
        menuButtons.SetActive(true);
        mainMenu.SetActive(true);
        videoTexture.SetActive(false);
        //Athena Stuff
        Debug.Log("Played");
    }
}
