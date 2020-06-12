using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlatformDetector
{
    public static bool isMobile()
    {
        return Application.isMobilePlatform;
    }
}
