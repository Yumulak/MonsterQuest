using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;
    public Canvas gameCanvas;

    private void Awake()
    {
        gameCanvas = FindObjectOfType<Canvas>();
    }

    public void CharacterTookDamage(GameObject character, int damagereceived)
    {
        // spawn text at hit position
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        //instantiate copy of UIManager prefab and set text on it
        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        tmpText.text = damagereceived.ToString();
    }

    public void CharacterHealed(GameObject character, int healthreceived) 
    {
        // spawn text at hit position
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        //instantiate copy of UIManager prefab and set text on it
        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        tmpText.text = healthreceived.ToString();
    }
}
