using System;

using UnityEngine;
using SanicballCore;

namespace Sanicball.Data
{
    [System.Serializable]
    public class CharacterInfo
    {
        public string name;
        public string artBy;
        public BallStats stats;
        public CharacterAbility[] abilities;
        public Material material;
        public Sprite icon;
        public Color color = Color.white;
        public Sprite minimapIcon;
        public Material trail;
        public float ballSize = 1;
        public Vector3 ballProportions = Vector3.one;
        public Quaternion ballRotation = Quaternion.identity;
        public bool cycleColors = false;
        public Color[] colorsToCycle;
        public float colorsPerSecond = 1;
        public Mesh alternativeMesh = null;
        public Mesh collisionMesh = null;
		public CharacterTier tier = CharacterTier.Normal;
        public bool hidden = false;
    }

    [System.Serializable]
    public class CharacterAbility {
        public UnityEngine.Object script;
        public UnityEngine.Object[] parameters;
        public string[] parameterTypes;
        public UnityEngine.Object[] dummyObjects;
    }
}