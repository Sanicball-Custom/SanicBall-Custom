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
        public Material material;
        public Sprite icon;
        public Color color = Color.white;
        public Sprite minimapIcon;
        public Material trail;
        public float ballSize = 1;
        public Mesh alternativeMesh = null;
        public Mesh collisionMesh = null;
		public CharacterTier tier = CharacterTier.Normal;
        public bool hidden = false;
    }
}