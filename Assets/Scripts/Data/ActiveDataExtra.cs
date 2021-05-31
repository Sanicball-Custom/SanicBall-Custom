using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SanicballCore;
using UnityEngine;

namespace Sanicball.Data {

    // Just an ScriptableObject for holding the sub-assets needed for the abilities
    [CreateAssetMenu(fileName = "ActiveDataExtra", menuName = "Sanicball/Extra ActiveData", order = 100)]
    public class ActiveDataExtra : ScriptableObject { }
}
