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
    protected DialogueRunner runner;

    // word block pooling
    [SerializeField]
    GameObject wordBlockPrefab;
    [SerializeField]
    GameObject clickableBlockPrefab;
    Queue<WordBlock> idleBlockPool = new Queue<WordBlock>();
    Queue<WordBlock> activeBlockPool = new Queue<WordBlock>();

    Queue<ClickableBlock> idleClickablePool = new Queue<ClickableBlock>();
    Queue<ClickableBlock> activeClickablePool = new Queue<ClickableBlock>();

    // configurable settings
    [SerializeField]
    Vector3 relativeWordCloudCenter;
    [SerializeField]
    float pauseBetweenLaunch = 1.0f;
    [SerializeField]
    YarnProgram linesForThisCharacter;

    // runtime only
    HashSet<string> keywords = new HashSet<string>();

    bool trigger = false;

    private void Start()
    {
        // actual logic
        runner.AddCommandHandler("Keyword", HandleKeyword);
        runner.Add(linesForThisCharacter);

        // test
        if (!trigger)
            runner.StartDialogue("TrivialStart");

        trigger = true;
    }

    public void ShowLine(string line)
    {
        string[] words = line.Split(' ');

        List<WordBlock> newlyActivated = new List<WordBlock>();
        
        // JIT creation of objects (might make some sense in terms of performance since NPCs repeate their speech)
        foreach (string word in words) 
        {
            // TODO: there's code copying probably refactor it somewhere

            if (!keywords.Contains(word)) { 
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
            else
            {
                if (idleClickablePool.Count <= 0)
                {
                    GameObject newBlock = Instantiate(clickableBlockPrefab);
                    ClickableBlock newClickableScript = newBlock.GetComponent<ClickableBlock>();
                    newClickableScript.PutWord(word);
                    activeClickablePool.Enqueue(newClickableScript);
                    newlyActivated.Add(newClickableScript);
                }
                else
                {
                    ClickableBlock block = idleClickablePool.Dequeue();
                    block.PutWord(word);
                    activeClickablePool.Enqueue(block);
                    newlyActivated.Add(block);
                }
            }
        }

        StartCoroutine(LaunchSequence(newlyActivated));
    }

    private IEnumerator LaunchSequence(List<WordBlock> newlyActivated)
    {
        float totalWidth = 0;
        foreach (WordBlock block in newlyActivated)
        {
            totalWidth += block.CalculateWidthEstimate();
            block.gameObject.SetActive(false);
        }

        float tentativeTotalWidth = 0;
        foreach (WordBlock block in newlyActivated)
        {
            float width = block.CalculateWidthEstimate();
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

        TriggerDialogue();
    }

    protected virtual void TriggerDialogue()
    {
    }

    private void HandleKeyword(string[] args)
    {
        if (args == null || args.Length < 1)
        {
            throw new UnityException("Keyword command error: expecting arguments but given none.");
        }

        foreach (string arg in args)
        {
            keywords.Add(arg);
        }
    }
}
