using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobileReady : MonoBehaviour {

    public bool ready;

    public void LateUpdate()
    {
        ready = false;
    }

    public void Ready()
    {
        ready = true;
    }
}
