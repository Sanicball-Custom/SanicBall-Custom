using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Transition {
    public GameObject gameObject;
    public int stayTime;
}

public class CreditsManager : MonoBehaviour {
    public int startDelaySeconds = 1;
    public int endDelaySeconds = 1;
    public string menuScene = "Menu_Sonic1";
    int fixedUpdateLoops = 0;
    public Transition[] transitionElements;
    public GenesisFadePostProcessing fader;
    bool countLoops = false;
    int transitionIndex = 0;
    public AudioSource music;

    void Start() {
        StartCoroutine(StartDelayed());
    }

    IEnumerator StartDelayed() {
        yield return new WaitForSeconds(startDelaySeconds);
        yield return fader.Unfade();
        countLoops = true;
    }

    IEnumerator EndDelayed() {
        yield return new WaitForSeconds(endDelaySeconds);
        countLoops = false;
        yield return SceneManager.LoadSceneAsync(menuScene);
    }

    IEnumerator UserEnded() {
        countLoops = false;
        yield return fader.Fade();
        for (float i = 1; i >= 0; i -= 0.05f) {
            music.volume = i;
            yield return new WaitForSeconds(0.025f);
        }
        SceneManager.LoadScene(menuScene);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            StartCoroutine(UserEnded());
        }
    }

    void FixedUpdate() {
        if (transitionIndex >= transitionElements.Length) return;
        if(fixedUpdateLoops >= transitionElements[transitionIndex].stayTime) {
            StartCoroutine(ShowTransition());
            fixedUpdateLoops = 0;
        }
        if (countLoops) fixedUpdateLoops++;
    }

    IEnumerator ShowTransition() {
        countLoops = false;
        yield return fader.Fade();
        transitionElements[transitionIndex].gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        transitionIndex++;
        if (transitionIndex < transitionElements.Length) {
            transitionElements[transitionIndex].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            yield return fader.Unfade();
            countLoops = true;
        } else {
            yield return EndDelayed();
        }
    }
}
