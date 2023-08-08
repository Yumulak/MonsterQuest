using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    public Vector3 moveSpeed = new Vector3(0,75,0);
    public float timeToFade = 1f;
    RectTransform textTransform;

    private float timeElapsed = 0f;

    TextMeshProUGUI textMeshPro;
    Color startColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        textTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        startColor = textMeshPro.color;
    }
    // Update is called once per frame
    void Update()
    {

        textTransform.position += moveSpeed * Time.deltaTime;
        timeElapsed += Time.deltaTime;
        float newAlpha = startColor.a * (1 - timeElapsed / timeToFade);
        textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
        if (timeElapsed > timeToFade)
        {
            Destroy(gameObject);
        }
    }
}
