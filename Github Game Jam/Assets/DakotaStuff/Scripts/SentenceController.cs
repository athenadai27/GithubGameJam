using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceController : MonoBehaviour
{
    public List<GameObject> words;
    public List<Vector3> wordPositions;
    public List<float> wordLerps;
    public List<bool> wordWigglesUp;
    public List<Transform> wordGoToTransforms;
    public float wordWiggleLerp;
    public float wiggleSpeed;
    public float travelSpeed;

    public enum SentenceStates { talking, done };
    public SentenceStates sentenceState;
    public int currentWordIndex;
    public float wordLerp;


    void Awake()
    {
        for (int i = 0; i < words.Count; i++)
        {
            wordPositions[i] = words[i].transform.position;
        }
    }
    void OnEnable()
    {
        wordLerp = 0;
        sentenceState = SentenceStates.talking;
        currentWordIndex = 0;
        for (int i = 0; i < words.Count; i++)
        {
            words[i].transform.position = wordPositions[i];
            words[i].transform.localScale = Vector3.zero;
        }
        Debug.Log("enabled");
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("startPositions");

    }

    // Update is called once per frame
    void Update()
    {
        switch (sentenceState)
        {
            case SentenceStates.talking:
                if (words[currentWordIndex].transform.position != wordGoToTransforms[currentWordIndex].position)
                {
                    wordLerp += Time.deltaTime * travelSpeed;
                    words[currentWordIndex].transform.position = Vector3.Lerp(wordPositions[currentWordIndex], wordGoToTransforms[currentWordIndex].position, wordLerp);
                    words[currentWordIndex].transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, wordLerp);
                }
                else
                {
                    currentWordIndex++;
                    wordLerp = 0;
                    if (currentWordIndex > words.Count - 1)
                    {
                        sentenceState = SentenceStates.done;
                    }
                }
                break;
            case SentenceStates.done:
                break;
        }
        // for(int i = 0; i < words.Count;i++){

        //     if(wordWigglesUp[i]){
        //          wordLerps[i] += Time.deltaTime*wiggleSpeed;
        //          if(wordLerps[i] >= 1){
        //              wordWigglesUp[i] = false;
        //          }
        //     } else{
        //         wordLerps[i] -= Time.deltaTime*wiggleSpeed;
        //          if(wordLerps[i] <= 0){
        //              wordWigglesUp[i] = true;
        //          }
        //     }
        //     words[i].transform.position = Vector3.Lerp(wordPositions[i], wordPositions[i] + Vector3.up,wordLerps[i]);
        // }
    }
}
