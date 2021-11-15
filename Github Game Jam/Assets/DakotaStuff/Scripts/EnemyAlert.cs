using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EnemyAlert : MonoBehaviour
{
    public TextMeshProUGUI enemyText;
    public enum AlertLevels { curious, searching, alarmed, regular };
    public AlertLevels alertLevel;
    public int currentThreatLevel;
    public List<GameObject> sentenceControllers;
    public GameObject activeSentenceController;

    public float deAggroWaitTime;
    public Transform playerTransform;
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (alertLevel)
        {
            case AlertLevels.curious:
                if (Time.time > deAggroWaitTime)
                {
                    SetThreatLevel(0);
                }
                break;
            case AlertLevels.searching:
                if (Time.time > deAggroWaitTime)
                {
                    SetThreatLevel(1);
                }
                if(playerTransform.position.x < transform.position.x){
                    transform.position += Vector3.left*moveSpeed*Time.deltaTime;
                } else if(playerTransform.position.x > transform.position.x){
                    transform.position += Vector3.right*moveSpeed*Time.deltaTime;
                }
                break;
            case AlertLevels.alarmed:
                if (Time.time > deAggroWaitTime)
                {
                    SetThreatLevel(2);
                }
                break;
            case AlertLevels.regular:
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("AlertLight"))
        {
            int threatLevel = collider.gameObject.GetComponent<AlertLight>().threatLevel;
            
            SetThreatLevel(threatLevel);
            
        }
    }

    public void SetThreatLevel(int threatLevel)
    {
            if(currentThreatLevel != threatLevel){
                activeSentenceController.SetActive(false);
            }
            currentThreatLevel = threatLevel;
            if(threatLevel == 0){
                sentenceControllers[0].SetActive(true);
                activeSentenceController = sentenceControllers[0];
                alertLevel = AlertLevels.regular;

            }
            else if (threatLevel == 1 )
            {
                sentenceControllers[1].SetActive(true);
                activeSentenceController = sentenceControllers[1];
                alertLevel = AlertLevels.curious;
                deAggroWaitTime = Time.time + 5f;
            }
            else if (threatLevel == 2 )
            {
                alertLevel = AlertLevels.searching;
                sentenceControllers[2].SetActive(true);
                activeSentenceController = sentenceControllers[2];
                deAggroWaitTime = Time.time + 7f;
            }
            else if (threatLevel == 3)
            {
                alertLevel = AlertLevels.alarmed;
                sentenceControllers[3].SetActive(true);
                activeSentenceController = sentenceControllers[3];
                deAggroWaitTime = Time.time + 9f;
            }
    }
}
