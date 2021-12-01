using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaMusicTrigger : MonoBehaviour
{
    [SerializeField]
    string key;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            MusicManager.instance.TriggerMusic(key);
        }
    }
}
