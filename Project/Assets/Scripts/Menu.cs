using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] SceneTransitioner sceneTransitioner;
    [SerializeField] TMP_Text menuText;
    bool fadedText;
    bool fadeText;

    void Start()
    {
        StartCoroutine(FadoutTimer());
    }

    IEnumerator FadoutTimer()
    {
        yield return new WaitForSeconds(3.5f);
        fadeText = true;
    }

    void Update()
    {
        if (fadeText)
        {
            if (fadedText == false)
                menuText.alpha -= Time.deltaTime;
            else
                menuText.alpha += Time.deltaTime;

            if (fadedText == false && menuText.alpha <= 0)
            {
                menuText.text = "press space";
                fadedText = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
            sceneTransitioner.TransitionToScene("Game");
    }
}
