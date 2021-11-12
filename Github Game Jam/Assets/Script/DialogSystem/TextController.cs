using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Yarn.Unity;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    [SerializeField]
    GameObject wordContainerPrefab;

    [SerializeField]
    DialogueRunner runner;

    public bool inDialog { get; set; } = false;
    Dictionary<string, Sprite> nameToPfp = new Dictionary<string, Sprite>();

    public static TextController instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ShowLine(string line)
    {
        // TODO: break line
        string[] words = line.Split(' ');
        
        // TODO: put each word into a block

        // TODO: float up blocks by some sequence (first iteration just send them up together)
    }


    public void StartDialog(string node)
    {
        if (!inDialog)
            runner.StartDialogue(node);
    }
}
