using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    private Image content;
    private float currentFill;
    private float currentValue;
    [SerializeField] float lerpSpeed;
    [SerializeField] private TextMeshProUGUI valueText;

    #region Properties
    public float MyMaxValue { get; set; }

    public float MyCurrentValue
    {
        get
        {
            return currentValue;
        }

        set
        {
            if (value > MyMaxValue)
            {
                currentValue = MyMaxValue;
            }

            else if (value < 0)
            {
                currentValue = 0;
            }
            else
            {
                currentValue = value;
            }

            currentFill = currentValue / MyMaxValue;
            valueText.text = currentValue.ToString() + "/" + MyMaxValue.ToString();
        }
    }

    #endregion 

    void Start()
    {
        content = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFill != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, 
                currentFill, Time.deltaTime * lerpSpeed);
        }
    }

    public void Initialize(float maxValue, float currentValue)
    {
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
    }
}
