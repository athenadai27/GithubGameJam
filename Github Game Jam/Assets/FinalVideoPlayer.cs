using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class FinalVideoPlayer : MonoBehaviour
{
     public VideoPlayer videoPlayer;
    public string targetScene;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

            if (videoPlayer.frame == (long)videoPlayer.frameCount - 1)
            {
                SceneManager.LoadScene(targetScene);

            }
        }

    }

    

