using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GenesisFadePostProcessing : MonoBehaviour {
    public int fadeSpeed = 25;
    public bool unfadeOnStart = false;
    Volume volume;
    void Start() {
        volume = GetComponent<Volume>();
        if (unfadeOnStart) StartCoroutine(Unfade());
    }

    void Update() {}

    public IEnumerator Unfade() {
        fadeSpeed = Mathf.Abs(fadeSpeed);
        return FadingLoop();
    }

    public IEnumerator Fade() {
        fadeSpeed = -fadeSpeed;
        return FadingLoop();
    }
    
    public IEnumerator FadingLoop() {
        //var new_lut = lut;
        Color32 color = fadeSpeed > 0 ? Color.black : Color.white;
        int limit = fadeSpeed > 0 ? 255 : 0;
        while (color.r != limit || color.g != limit || color.b != limit) {
            if(fadeSpeed > 0) {
                if (color.b != limit) {
                    color.b = (byte)Mathf.Max(0, Mathf.Min(color.b + fadeSpeed, 255));
                } else if (color.g != limit) {
                    color.g = (byte)Mathf.Max(0, Mathf.Min(color.g + fadeSpeed, 255));
                } else if (color.r != limit) {
                    color.r = (byte)Mathf.Max(0, Mathf.Min(color.r + fadeSpeed, 255));
                }
            }else {
                if (color.r != limit) {
                    color.r = (byte)Mathf.Max(0, Mathf.Min(color.r + fadeSpeed, 255));
                } else if (color.g != limit) {
                    color.g = (byte)Mathf.Max(0, Mathf.Min(color.g + fadeSpeed, 255));
                } else if (color.b != limit) {
                    color.b = (byte)Mathf.Max(0, Mathf.Min(color.b + fadeSpeed, 255));
                }
            }
            ColorAdjustments colorOverride;
            volume.profile.TryGet(out colorOverride);
            colorOverride.colorFilter.value = color;
            yield return null;
        }
    }
}
