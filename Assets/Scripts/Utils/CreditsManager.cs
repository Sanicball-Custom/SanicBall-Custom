using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour {
    public int timeBetweenTransitions = 30;
    int fixedUpdateLoops = 0;
    public GameObject[] transitionElements;
    public GenesisFade fader;
    bool countLoops = true;
    int transitionIndex = 0;

    void Start() {
        StartCoroutine(fader.Unfade());
    }

    void FixedUpdate() {
        if (transitionIndex >= transitionElements.Length) return;
        if(fixedUpdateLoops >= timeBetweenTransitions) {
            StartCoroutine(ShowTransition());
            fixedUpdateLoops = 0;
        }
        if (countLoops) fixedUpdateLoops++;
    }

    IEnumerator ShowTransition() {
        countLoops = false;
        yield return fader.Fade();
        transitionElements[transitionIndex].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        transitionIndex++;
        if (transitionIndex < transitionElements.Length) {
            transitionElements[transitionIndex].SetActive(true);
            yield return new WaitForSeconds(0.25f);
            yield return fader.Unfade();
            countLoops = true;
        }
    }
}
