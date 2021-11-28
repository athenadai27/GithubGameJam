using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfBoutheinaHiding : MonoBehaviour
{
    public PlayerController boutheina;
    public GameObject objectToActivate;
    public GameObject promptToDeactivate;
    public List<GameObject> objectsToDeactivate;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (boutheina.playerState == PlayerController.PlayerStates.hiding)
        {
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
            }

            if (promptToDeactivate != null)
            {
                promptToDeactivate.SetActive(false);

            }
            for (int i = 0; i < objectsToDeactivate.Count; i++)
            {
                objectsToDeactivate[i].SetActive(false);
            }
            this.enabled = false;
        }
    }
}
