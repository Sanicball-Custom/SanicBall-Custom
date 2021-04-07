using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingBall : MonoBehaviour {
    void Start() {
        if(!Application.isEditor) {
            Destroy(gameObject);
        }
    }
}
