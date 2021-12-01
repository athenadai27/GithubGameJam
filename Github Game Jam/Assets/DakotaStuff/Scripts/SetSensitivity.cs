using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SetSensitivity : MonoBehaviour
{
    public Slider stemSensitivitySlider;
    public StemController stemController;
    public TextMeshProUGUI sensitivityText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStemSensitivity(){
        stemController.SetSensitivity(stemSensitivitySlider.value/5);
        sensitivityText.text = stemSensitivitySlider.value.ToString();
    }
}
