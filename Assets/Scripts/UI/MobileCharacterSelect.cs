using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanicball;

public class MobileCharacterSelect : MonoBehaviour {

    public bool clicked;

	// Use this for initialization
	void Start () {
        if (!PlatformDetector.isMobile()) gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
       
	}

    private void LateUpdate()
    {
        clicked = false;
    }
    public void Click()
    {   
        clicked = true;
    }
}
