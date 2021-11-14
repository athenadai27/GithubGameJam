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
    Queue<WordBlock> idleBlockPool;
    Queue<WordBlock> activeBlockPool;

    // configurable settings
    [SerializeField]
    Vector3 relativeWordCloudCenter;

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

        // set destination for each block
        float totalWidth = 0;
        foreach (WordBlock block in newlyActivated)
        {
            totalWidth += block.textBox.preferredWidth;
        }
        
        float tentativeTotalWidth = 0;
        foreach (WordBlock block in newlyActivated)
        {
            tentativeTotalWidth  += block.textBox.preferredWidth;
            float destX = 0 - totalWidth / 2 + tentativeTotalWidth;

            // actually put it into a vector
            Vector3 newDest = relativeWordCloudCenter;
            newDest.x = destX;
            block.SetDest(newDest);
            block.Launch(gameObject.transform.position);
        }
    }

    private void Update()
    {
        HashSet<WordBlock> encounters = new HashSet<WordBlock>();
        while (true)
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
        }
    }
}
