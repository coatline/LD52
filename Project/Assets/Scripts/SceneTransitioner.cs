using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitioner : MonoBehaviour
{
    [SerializeField] float fadeDuration = 1;
    [SerializeField] Image fade;
    bool fadeout;

    string scene;

    public void TransitionToScene(string sceneName)
    {
        scene = sceneName;
        fadeout = true;
    }

    void Update()
    {
        if (fadeout == false)
        {
            if (fade.color.a > 0)
                fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fade.color.a - Time.deltaTime / fadeDuration);
        }
        else if (fade.color.a < 1)
        {
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fade.color.a + Time.deltaTime / fadeDuration);

            if (fade.color.a >= 1)
                SceneManager.LoadScene(scene);
        }
    }
}
