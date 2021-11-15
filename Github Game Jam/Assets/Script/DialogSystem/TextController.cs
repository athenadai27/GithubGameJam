using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Yarn.Unity;
using UnityEngine.UI;

/// <summary>
/// Mediator for all NPC dialogue behavior.
/// </summary>
public class TextController : MonoBehaviour
{
    [SerializeField]
    DialogueRunner runner;

    // word block pooling
    [SerializeField]
    GameObject wordBlockPrefab;
    Queue<WordBlock> idleBlockPool = new Queue<WordBlock>();
    Queue<WordBlock> activeBlockPool = new Queue<WordBlock>();

    // configurable settings
    [SerializeField]
    Vector3 relativeWordCloudCenter;
    [SerializeField]
    float pauseBetweenLaunch = 1.0f;

    /// <summary>
    /// very dangerous and evil, delete after testing
    /// </summary>
    private void Start()
    {
        ShowLine("some line here");
    }

    public void ShowLine(string line)
    {
        string[] words = line.Split(' ');

        List<WordBlock> newlyActivated = new List<WordBlock>();
        
        // JIT creation of objects (might make some sense in terms of performance since NPCs repeate their speech)
        foreach (string word in words) 
        {
            if (idleBlockPool.Count <= 0)
            {
                GameObject newBlock = Instantiate(wordBlockPrefab);
                WordBlock newBlockScript = newBlock.GetComponent<WordBlock>();
                newBlockScript.PutWord(word);
                activeBlockPool.Enqueue(newBlockScript);
                newlyActivated.Add(newBlockScript);
            }
            else
            {
                WordBlock block = idleBlockPool.Dequeue();
                block.PutWord(word);
                activeBlockPool.Enqueue(block);
                newlyActivated.Add(block);
            }
        }

        StartCoroutine(LaunchSequence(newlyActivated));
    }

    private IEnumerator LaunchSequence(List<WordBlock> newlyActivated)
    {
        float totalWidth = 0;
        foreach (WordBlock block in newlyActivated)
        {
            totalWidth += block.textBox.preferredWidth * block.transform.localScale.x;
            block.gameObject.SetActive(false);
        }

        float tentativeTotalWidth = 0;
        foreach (WordBlock block in newlyActivated)
        {
            float width = block.textBox.preferredWidth * block.transform.localScale.x;
            float destX = 0 - totalWidth / 2 + tentativeTotalWidth + width / 2;
            tentativeTotalWidth += width;

            // actually put it into a vector
            Vector3 newDest = relativeWordCloudCenter + transform.position;
            newDest.x = destX;
            block.SetDest(newDest);
            block.Launch(gameObject.transform.position);

            // wait lock
            float timeElapsed = 0;
            while (timeElapsed < pauseBetweenLaunch)
            {
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
    }

    private void Update()
    {
        HashSet<WordBlock> encounters = new HashSet<WordBlock>();
        while (activeBlockPool.Count > 0)
        {
            WordBlock top = activeBlockPool.Dequeue();
            if (top.gameObject.activeSelf)
            {
                encounters.Add(top);
                activeBlockPool.Enqueue(top);
            }
            else
            {
                idleBlockPool.Enqueue(top);
            }

            if (encounters.Contains(top))
                break;
        }
    }
}
