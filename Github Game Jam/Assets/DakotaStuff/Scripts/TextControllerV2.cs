using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Yarn.Unity;
using UnityEngine.UI;

/// <summary>
/// Mediator for all NPC dialogue behavior.
/// </summary>
public class TextControllerV2 : MonoBehaviour
{
    [SerializeField]
    DialogueRunner runner;

    // word block pooling
    [SerializeField]
    GameObject wordBlockPrefab;
    Queue<WordBlockV2> idleBlockPool = new Queue<WordBlockV2>();
    Queue<WordBlockV2> activeBlockPool = new Queue<WordBlockV2>();

    public List<WordBlockV2> idleBlockList = new List<WordBlockV2>();
    public List<WordBlockV2> activeBlockList = new List<WordBlockV2>();

    // configurable settings
    public Vector3 relativeWordCloudCenter;
    [SerializeField]
    float pauseBetweenLaunch = 1.0f;

    [SerializeField]
    string sentence;
    [SerializeField]
    List<string> specialWords;

    [SerializeField]
    WordGrabber wordGrabber;
    [SerializeField]
    StemController stemController;
    [SerializeField]
    List<GameObject> wordItemSpawnObjects;

    [SerializeField]
    int lineCount;

    [SerializeField]
    Transform npcCanvasTransform;

    public List<WordBlockV2> newlyActivated = new List<WordBlockV2>();

    public ActivateDeactivateAfterText activateDeactivateAfterText;
    /// <summary>
    /// very dangerous and evil, delete after testing
    /// </summary>
    private void OnEnable()
    {
        ShowLine(sentence);
    }

    public void ShowLine(string line)
    {
        string[] words = line.Split(' ');

        newlyActivated.Clear();
       
        // JIT creation of objects (might make some sense in terms of performance since NPCs repeate their speech)
        for (int i = 0; i < words.Length;i++)
        {
            string word = words[i];
            //if (idleBlockPool.Count <= 0)
            if (idleBlockList.Count <= 0)
            {
                
                GameObject newBlock = Instantiate(wordBlockPrefab);
                newBlock.transform.SetParent(npcCanvasTransform);
                WordBlockV2 newBlockScript = newBlock.GetComponent<WordBlockV2>();
                newBlockScript.PutWord(word);
                if (specialWords.Contains(word))
                {
                    newBlock.transform.GetChild(0).gameObject.AddComponent<GrabbableWord>();
                    newBlock.transform.GetChild(0).gameObject.GetComponent<GrabbableWord>().stemController = stemController;
                    newBlock.transform.GetChild(0).gameObject.GetComponent<GrabbableWord>().objectToSpawn = wordItemSpawnObjects[specialWords.FindIndex(x => x == word)];
                    wordGrabber.graphicRaycasters.Add(newBlock.GetComponent<GraphicRaycaster>());
                    newBlockScript.textBox.color = Color.yellow;
                }
                // activeBlockPool.Enqueue(newBlockScript);
                activeBlockList.Add(newBlockScript);
                newlyActivated.Add(newBlockScript);
            }
            else
            {
                //WordBlockV2 block = idleBlockPool.Dequeue();
                WordBlockV2 block = idleBlockList[i];
                //idleBlockList.RemoveAt(0);
                block.PutWord(word);
                activeBlockList.Add(block);
                //activeBlockPool.Enqueue(block);
                newlyActivated.Add(block);
            }
        }
        idleBlockList.Clear();
        StartCoroutine(LaunchSequence(newlyActivated));
    }

    private IEnumerator LaunchSequence(List<WordBlockV2> newlyActivatedList)
    {
        float totalWidth = 0;
        foreach (WordBlockV2 block in newlyActivatedList)
        {
            totalWidth += block.CalculateWidthEstimate();
            block.gameObject.SetActive(false);
        }

        float lineWidth = totalWidth / lineCount;
        float tentativeTotalWidth = 0;
        float stepDown = 0f;
        foreach (WordBlockV2 block in newlyActivatedList)
        {
            float width = block.CalculateWidthEstimate();
            if (tentativeTotalWidth >= lineWidth)
            {
                tentativeTotalWidth = 0;
                stepDown += 1f;
            }
            float destX = 0 - lineWidth / 2 + tentativeTotalWidth + width / 2;
            tentativeTotalWidth += width;

            // actually put it into a vector
            Vector3 newDest = relativeWordCloudCenter + transform.localPosition;
            newDest.x += transform.localPosition.x + destX;
            newDest.y -= stepDown;
            block.SetDest(newDest);
            block.Launch(gameObject.transform.localPosition);

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
        HashSet<WordBlockV2> encounters = new HashSet<WordBlockV2>();

        // while (activeBlockPool.Count > 0)
        if(activeBlockList.Count > 0){
            for(int i = 0; i < activeBlockList.Count;i++){
                idleBlockList.Add(activeBlockList[i]);
            }
            activeBlockList.Clear();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            for (int i = 0; i < idleBlockList.Count; i++)
            {
                idleBlockList[i].gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator FadeTextRoutine()
    {
        float lerp = 0f;
        while (lerp != 1)
        {
            lerp += Time.deltaTime;
            for (int i = 0; i < idleBlockList.Count; i++)
            {
                Color newTextColor = idleBlockList[i].textBox.color;
                newTextColor.a = Mathf.Lerp(1, 0, lerp);
            }
            yield return null;
        }

    }

    public void FadeText()
    {
        for (int i = 0; i < idleBlockList.Count; i++)
        {
            idleBlockList[i].gameObject.SetActive(false);
        }
        if(activateDeactivateAfterText != null){
            activateDeactivateAfterText.ActivateAndDeactivate();
        }
        gameObject.SetActive(false);
    }

    public bool CheckIfArrived(){
        if(idleBlockList.Count <= 0){
            return false;
        }
        bool hasArrived = true;
        for(int i = 0; i < idleBlockList.Count;i++){
            if(!idleBlockList[i].arrived){
                hasArrived = false;
            }
        }
        return hasArrived;
    }
}
