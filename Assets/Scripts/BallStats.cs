namespace Sanicball
{
    [System.Serializable]
    public class BallStats
    {
        public float rollSpeed = 100;
        public float airSpeed = 15;
        public float jumpHeight = 14;
        public float grip = 0;
		public bool multipleJump = false;
		public int extraJumps = 0;
		public bool slowFall = false;
    }
}