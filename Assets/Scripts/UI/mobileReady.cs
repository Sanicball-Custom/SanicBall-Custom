using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobileReady : MonoBehaviour {

    public bool ready;

    public void Start() {
        if (!PlatformDetector.isMobile()) gameObject.SetActive(false);
    }

    public void LateUpdate()
    {
        ready = false;
    }

    public void Ready()
    {
        ready = true;
    }
}
