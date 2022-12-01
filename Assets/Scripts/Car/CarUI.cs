using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;

public class CarUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private Animator speedometer;

    public void UpdateUI(float value)
    {
        if (value >= 0)
        {
            speedText.text = Mathf.RoundToInt(value).ToString();
            speedometer.SetFloat("speed",value);
        }
        else if(value < 0)
        {
            speedText.text = Mathf.RoundToInt(Mathf.Abs(value)).ToString();
            speedometer.SetFloat("speed",Mathf.Abs(value));
        }
        
    }
}
