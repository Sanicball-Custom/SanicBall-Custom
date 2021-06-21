using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsSceneWarp : MonoBehaviour {
    public GenesisFadePostProcessing fader;
    
    public void GoToCreditsScene(string sceneName) {
        StartCoroutine(Warp(sceneName));
    }

    IEnumerator Warp(string sceneName) {
        yield return fader.Fade();
        yield return new WaitForSeconds(2);
        yield return SceneManager.LoadSceneAsync(sceneName);
    }
}
