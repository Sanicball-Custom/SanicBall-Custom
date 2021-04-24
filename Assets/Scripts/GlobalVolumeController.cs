using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Sanicball.Data;

public class GlobalVolumeController : MonoBehaviour {
    private Volume volume;
    private Bloom bloom;
    private MotionBlur motionBlur;

    void Start() {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out motionBlur);
        bloom.active = ActiveData.GameSettings.bloom;
        motionBlur.active = ActiveData.GameSettings.motionBlur;
        StartCoroutine(UpdateAntialiasing());
    }

    IEnumerator UpdateAntialiasing() {
        yield return new WaitForSecondsRealtime(1f);
        if (ActiveData.GameSettings.aa == 0) {
            FindObjectsOfType<UniversalAdditionalCameraData>().ToList().ForEach(camera => camera.antialiasing = AntialiasingMode.None);
        } else {
            FindObjectsOfType<UniversalAdditionalCameraData>().ToList().ForEach(camera => camera.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing);
            switch (ActiveData.GameSettings.aa) {
                case 2:
                    FindObjectsOfType<UniversalAdditionalCameraData>().ToList().ForEach(camera => camera.antialiasingQuality = AntialiasingQuality.Low);
                    break;
                case 4:
                    FindObjectsOfType<UniversalAdditionalCameraData>().ToList().ForEach(camera => camera.antialiasingQuality = AntialiasingQuality.Medium);
                    break;
                case 8:
                    FindObjectsOfType<UniversalAdditionalCameraData>().ToList().ForEach(camera => camera.antialiasingQuality = AntialiasingQuality.High);
                    break;
            }
        }
    }
}