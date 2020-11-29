using System;
using Sanicball.Data;
using Sanicball.UI;
using Sanicball.Logic;
using SanicballCore;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Sanicball.Extra;

namespace Sanicball.Gameplay
{
    public enum BallType
    {
        Player,
        LobbyPlayer,
        AI
    }

    public class CheckpointPassArgs : System.EventArgs
    {
        public CheckpointPassArgs(Checkpoint c)
        {
            CheckpointPassed = c;
        }

        public Checkpoint CheckpointPassed { get; private set; }
    }

    public class CameraCreationArgs : System.EventArgs
    {
        public CameraCreationArgs(IBallCamera cameraCreated)
        {
            CameraCreated = cameraCreated;
        }

        public IBallCamera CameraCreated { get; private set; }
    }

    [System.Serializable]
    public class BallMotionSounds
    {
        [SerializeField]
        private AudioSource jump;
        [SerializeField]
        private AudioSource roll;
        [SerializeField]
        private AudioSource speedNoise;
        [SerializeField]
        private AudioSource brake;

        public AudioSource Jump { get { return jump; } }
        public AudioSource Roll { get { return roll; } }
        public AudioSource SpeedNoise { get { return speedNoise; } }
        public AudioSource Brake { get { return brake; } }
    }

    [System.Serializable]
    public class BallPrefabs
    {
        [SerializeField]
        private DriftySmoke smoke;
        [SerializeField]
        private OmniCamera camera;
        [SerializeField]
        private PivotCamera oldCamera;
        [SerializeField]
        private ParticleSystem removalParticles;
        [SerializeField]
        private SpeedFire speedFire;

        public DriftySmoke Smoke { get { return smoke; } }
        public OmniCamera Camera { get { return camera; } }
        public PivotCamera OldCamera { get { return oldCamera; } }
        public ParticleSystem RemovalParticles { get { return removalParticles; } }
        public SpeedFire SpeedFire{ get { return speedFire; } }
    }

    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        //These are set using Init() when balls are instantiated
        //But you can set them from the editor to quickly test out a track
        [Header("Initial stats")]
        [SerializeField]
        private BallType type;
        [SerializeField]
        private ControlType ctrlType;
        [SerializeField]
        private int characterId;
        [SerializeField]
        private string nickname;
        [SerializeField]
        private GameObject hatPrefab;

        public BallType Type { get { return type; } }
        public ControlType CtrlType { get { return ctrlType; } }
        public int CharacterId { get { return characterId; } }

        [Header("Subcategories")]
        [SerializeField]
        public BallPrefabs prefabs;
        [SerializeField]
        public BallMotionSounds sounds;

        //State
		[System.NonSerialized]
        public BallStats characterStats;
		[System.NonSerialized]
        public bool canMove = true;
        private BallControlInput input;
        private bool grounded = false;
        private float groundedTimer = 0;
        private float upResetTimer = 0;
        private DriftySmoke smoke;
		[System.NonSerialized]
        public SpeedFire speedFire;

        public bool CanMove { get { return canMove; } set { canMove = value; } }
        public bool AutoBrake { get; set; }
        public Vector3 DirectionVector { get; set; }
        public Vector3 Up { get; set; }
        public bool Brake { get; set; }
        public string Nickname { get { return nickname; } }
		
		public Powerup[] powerups = new Powerup[2];
		[System.NonSerialized]
		public bool changeUIPowerups = false;
		[System.NonSerialized]
		public bool canUsePowerups = true;
		[System.NonSerialized]
		public bool zapped = false;
		private float zapTimer = 0;
		private float timeZapped = 10.0f;
        private ParticleSystem zap_particles;
		private float originalSize = 1;
        [System.NonSerialized]
		public bool confused = false;
		private float confuseTimer = 0;
		private float timeConfused = 10.0f;
		
		public bool canMultiJump = false;
		public int extraJumps = 0;
		private int jumpsRemaining = 0;

        private Vector3 slowFallInitialSpeed = new Vector3();
        private bool usedSlowFall = false;

        public GameObject joystickCanvas;
        public bl_Joystick joystick;
        public mobileMovementManager mmm;
        public bool mobile;

        //public bool gravity;

        //Component caches
		[System.NonSerialized]
        public Rigidbody rb;
        public BallControlInput Input { get { return input; } }

        //Events
        public event System.EventHandler<CheckpointPassArgs> CheckpointPassed;
        public event System.EventHandler RespawnRequested;
        public event System.EventHandler<CameraCreationArgs> CameraCreated;

        public Vector3 gravDir;
		
		[System.NonSerialized]
		public string characterName;

        public void Jump(bool hold)
        {
            if(canMove) { //possible movement section
                if ((grounded || jumpsRemaining > 0) && !hold) {
                    float scalar = Vector3.Dot((-gravDir).normalized, rb.velocity.normalized);
                    if (scalar < 0)
                        rb.velocity -= (-gravDir) * scalar * rb.velocity.magnitude;
                    //if(rb.velocity.y < 0) rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);


                    //if (grounded)
                    //else
                    //            jumpDir = new Vector3(Up.x * (-gravDir.normalized.x), Up.y * (-gravDir.normalized.y), Up.z * (-gravDir.normalized.z));


                    //rb.AddForce((grounded ? Up : -gravDir.normalized) * characterStats.jumpHeight, ForceMode.Impulse);
                    rb.AddForce(Up * characterStats.jumpHeight, ForceMode.Impulse);
                    if (sounds.Jump != null)
                    {
                        sounds.Jump.Play();
                    }
                    if(!grounded){
                        jumpsRemaining--;
                    }
                    grounded = false;
                }
                /*if (characterStats.slowFall && hold) {
                    canMove = false;
                    usedSlowFall = true;
                    if (slowFallInitialSpeed.sqrMagnitude == 0) slowFallInitialSpeed = rb.velocity;
                    var rotatedVelocity = (Quaternion.Euler(gravDir) * rb.velocity);
                    //if (rotatedVelocity.magnitude > gravDir.magnitude && Vector3.Dot(rotatedVelocity.normalized, gravDir) > 0 && !rb.useGravity) {
                    if (Vector3.Scale(rotatedVelocity, gravDir).magnitude > gravDir.magnitude && !rb.useGravity) { 
                        var slowFallVelocity = new Vector3();
                        slowFallVelocity.x = -gravDir.x * slowFallInitialSpeed.x;
                        slowFallVelocity.y = -gravDir.y * slowFallInitialSpeed.y;
                        slowFallVelocity.z = -gravDir.z * slowFallInitialSpeed.z;
                        rb.velocity = slowFallVelocity;
                        //rb.velocity = gravDir * slowFallInitialSpeed.magnitude;
                    } else if (rb.velocity.y < -1.5f && rb.useGravity)
                        rb.velocity = new Vector3(rb.velocity.x, -1.5f, rb.velocity.z);
                }*/
            }
        }

		public void UsePowerup(int slot) {
            var powerup = powerups[slot];
			if(powerup != null && canUsePowerups) {
                PowerupMessages data = new PowerupMessages();
                data.input = gameObject;
                Component action = powerup.GetComponent(powerup.name + "Action");
                if(action != null) {
                    action.SendMessage("Use", data);
                }
                if(data.output) {
                    powerups[slot] = null;
                    if(!GetComponent<BallControlAI>()){
                        changeUIPowerups = true;
                    }
                }
			}
        }
		
        public void RequestRespawn()
        {
            if (RespawnRequested != null){
                RespawnRequested(this, System.EventArgs.Empty);
            }
            if(!CanMove){
                CanMove = true;
                rb.useGravity = true;   
            }
        }

        public void Init(BallType type, ControlType ctrlType, int characterId, string nickname)
        {
            this.type = type;
            this.ctrlType = ctrlType;
            this.characterId = characterId;
            this.nickname = nickname;
			
			
			prefabs.Camera.orbitHeight = 1;
			prefabs.Camera.orbitDistance = 6;

;        }

        private void Start()
        {
            Up = Vector3.up;

            zap_particles = transform.GetComponentInChildren<ParticleSystem>();
            zap_particles.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);

            customGravity cm = new GameObject("start custom gravity", typeof(customGravity)).GetComponent<customGravity>();
            cm.gravityType = GravityType.Default;
            cm.manageGrav(this);
            Destroy(cm.gameObject);
			
			//powerups[0] = GameObject.Find("PowerupLightningBolt").GetComponent(typeof(Powerup)) as Powerup;
			//powerups[1] = GameObject.Find("PowerupAdvance").GetComponent(typeof(Powerup)) as Powerup;

            //Set up drifty smoke
            smoke = Instantiate(prefabs.Smoke);
            smoke.target = this;
            smoke.DriftAudio = sounds.Brake;


            //Grab reference to Rigidbody
            rb = GetComponent<Rigidbody>();
            rb.useGravity = true;
            //Set angular velocity (This is necessary for fast)
            rb.maxAngularVelocity = 1000f;

            //Set object name
            gameObject.name = type.ToString() + " - " + nickname;

            //Set character
            if (CharacterId >= 0 && CharacterId < ActiveData.Characters.Length)
            {
                SetCharacter(ActiveData.Characters[CharacterId]);
				canMultiJump = characterStats.multipleJump;
				extraJumps = characterStats.extraJumps;
            }

            //Set up speed effect
            speedFire = Instantiate(prefabs.SpeedFire);
            speedFire.Init(this);

            //Crimbus
            DateTime now = DateTime.Now;
            if (now.Month == 12 && now.Day > 20 && now.Day <= 31)
            {
                hatPrefab = ActiveData.ChristmasHat;
            }
			
			if ((now.Month == 10 && now.Day > 20) || (now.Month == 11 && now.Day < 11))
            {
                hatPrefab = ActiveData.HalloweenHat;
            }

            var sceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (sceneIndex == 11 || sceneIndex == 12 || true)
            {
                hatPrefab = ActiveData.WaluigiHat;
            }

            if (ActiveData.GameSettings.eSportsReady && ActiveData.GameSettings.characterHats)
            {
                GameObject hatEsports = Instantiate(ActiveData.ESportsHat);
                hatEsports.transform.SetParent(transform, false);
            }

            //Spawn hat
            if (hatPrefab != null && ActiveData.GameSettings.characterHats)
            {
                GameObject hat = Instantiate(hatPrefab);
                hat.transform.SetParent(transform, false);
                hat.transform.rotation = Quaternion.Euler(-transform.rotation.eulerAngles)*hat.transform.rotation;
            }
			
			//Execute the Ball Modifying per Track
			GameObject modifierGO = GameObject.Find("StageModifier");
			if(modifierGO != null){
				StageModifier modifier = (StageModifier)modifierGO.GetComponent(typeof(StageModifier));
				modifier.ModifyBall(gameObject);
			}
			
            //Create objects and components based on ball type
            if (type == BallType.Player)
            {
                if (ctrlType != ControlType.None)
                {
                    IBallCamera camera;
                    //Create camera
                    if (ActiveData.GameSettings.useOldControls)
                    {
                        camera = Instantiate(prefabs.OldCamera);
                        ((PivotCamera)camera).UseMouse = ctrlType == ControlType.Keyboard;
                    }
                    else
                    {
                        camera = Instantiate(prefabs.Camera);
                    }
                    camera.Target = rb;
                    camera.CtrlType = ctrlType;

                    if (CameraCreated != null)
                        CameraCreated(this, new CameraCreationArgs(camera));
                }
            }
            if (type == BallType.LobbyPlayer)
            {
                //Make the lobby camera follow this ball
                var cam = FindObjectOfType<LobbyCamera>();
                if (cam)
                {
                    cam.AddBall(this);
                }
            }
            if ((type == BallType.Player || type == BallType.LobbyPlayer) && ctrlType != ControlType.None)
            {
                //Create input component
                input = gameObject.AddComponent<BallControlInput>();
            }
            if (type == BallType.AI)
            {
                //Create AI component
                gameObject.AddComponent<BallControlAI>();
            }
            if (ctrlType == ControlType.Mobile)  //change later :)
            {
                mobile = true;
                GameObject joystickIC = Instantiate(joystickCanvas, Vector3.zero, Quaternion.identity);
                joystick = joystickIC.GetComponentInChildren<bl_Joystick>();
                    
                if (type == BallType.Player)
                {
                    mmm = FindObjectOfType<mobileMovementManager>();
                    mmm.ball = this;
                }

            }
        }


        private void SetCharacter(Data.CharacterInfo c)
        {
            GetComponent<Renderer>().material = c.material;
            GetComponent<TrailRenderer>().material = c.trail;
            if (c.name == "Super Sanic" && ActiveData.GameSettings.eSportsReady) {
                GetComponent<TrailRenderer>().material = ActiveData.ESportsTrail;
            }
            transform.localScale = new Vector3(c.ballSize * c.ballProportions.x, c.ballSize * c.ballProportions.y, c.ballSize * c.ballProportions.z);
            if(c.ballRotation != Quaternion.identity)
                transform.rotation = c.ballRotation;
			originalSize = c.ballSize;
            if (c.alternativeMesh != null)
            {
                GetComponent<MeshFilter>().mesh = c.alternativeMesh;
            }
            //set collision mesh too
            if (c.collisionMesh != null)
            {
                if (c.collisionMesh.vertexCount <= 255)
                {
                    Destroy(GetComponent<Collider>());
                    MeshCollider mc = gameObject.AddComponent<MeshCollider>();
                    mc.sharedMesh = c.collisionMesh;
                    mc.convex = true;
                }
                else
                {
                    Debug.LogWarning("Vertex count for " + c.name + "'s collision mesh is bigger than 255!");
                }
            }
            characterStats = c.stats;
			characterName = c.name;
            SpriteRenderer minimapIcon = GetComponentInChildren<SpriteRenderer>();
            if(minimapIcon != null){
                minimapIcon.sprite = c.minimapIcon;
            }
            foreach(Transform child in transform){
                if(child.gameObject.name == "Minimap Icon"){
                    GameObject minimapCamera = GameObject.Find("MinimapCamera");
                    if(minimapCamera != null){
                        Camera MCamera = minimapCamera.GetComponent<Camera>();
                        if(MCamera != null){
                            child.localScale = new Vector3(1f,1f,1f)*(100f/855.6f) * MCamera.orthographicSize;
                        }
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if (!rb.useGravity && canMove)
                rb.AddForce(gravDir * rb.mass * Physics.gravity.magnitude, ForceMode.Acceleration);            

            if (CanMove)
            {   //movement 2
                //If grounded use torque
                if (DirectionVector != Vector3.zero)
                {

                    if(confused){
                        rb.AddTorque(DirectionVector * -characterStats.rollSpeed);
                    }else{
                        rb.AddTorque(DirectionVector * characterStats.rollSpeed);
                    }
                }
                //If not use both
                if (!grounded)
                {
                    if(confused){
                        rb.AddForce((Quaternion.Euler(0, -90, 0) * DirectionVector) * -characterStats.airSpeed);
                    }else{
                        rb.AddForce((Quaternion.Euler(0, -90, 0) * DirectionVector) * characterStats.airSpeed);
                    }
                }
            }

            if (canMove && CharacterId == 31 && !grounded) { // Big Chungus
                rb.AddForce(gravDir * rb.mass * Physics.gravity.magnitude*4, ForceMode.Acceleration);
            }

            if (AutoBrake)
            {
                //Always brake when AutoBrake is on
                Brake = true;
            }

            //Braking
            if (Brake)
            {
                //Force ball to brake by resetting angular velocity every update
                rb.angularVelocity = Vector3.zero;
            }

            // Downwards torque for extra grip - currently not used
            if (grounded)
            {
                //rb.AddForce(-Up*characterStats.grip * (rb.velocity.magnitude/400)); //Downwards gravity to increase grip
                //Debug.Log(stats.grip * Mathf.Pow(rigidbody.velocity.magnitude/100,2));
            }

            if (characterStats.slowFall && !GameInput.IsJumping(ctrlType, false)) {
                canMove = true;
                slowFallInitialSpeed = Vector3.zero;
            }
            if (grounded && usedSlowFall) usedSlowFall = false;
        }

        private void Update()
        {
            //Rolling sounds
            if (grounded)
            {
                float rollSpd = Mathf.Clamp(rb.angularVelocity.magnitude / 230, 0, 16);
                float vel = (-128f + rb.velocity.magnitude) / 256; //Start at 128 fph, end at 256

                vel = Mathf.Clamp(vel, 0, 1);
                if (sounds.Roll != null)
                {
                    sounds.Roll.pitch = Mathf.Max(rollSpd, 0.8f);
                    sounds.Roll.volume = Mathf.Min(rollSpd, 1);
                }
                if (sounds.SpeedNoise != null)
                {
                    sounds.SpeedNoise.pitch = 0.8f + vel;
                    sounds.SpeedNoise.volume = vel;
                }
            }
            else
            {
                //Fade sounds out when in the air
                if (sounds.Roll != null && sounds.Roll.volume > 0)
                {
                    sounds.Roll.volume = Mathf.Max(0, sounds.Roll.volume - 0.2f);
                }
                if (sounds.SpeedNoise != null && sounds.SpeedNoise.volume > 0)
                {
                    sounds.SpeedNoise.volume = Mathf.Max(0, sounds.SpeedNoise.volume - 0.01f);
                }
            }

            //Grounded timer
            if (groundedTimer > 0)
            {
                groundedTimer -= Time.deltaTime;
                if (groundedTimer <= 0)
                {
                    grounded = false;
                    upResetTimer = 1f;
                }
            }

            if (!grounded)
            {
                if (upResetTimer > 0)
                {
                    upResetTimer -= Time.deltaTime;
                }
                else
                {
                    Up = Vector3.MoveTowards(Up, -gravDir.normalized, Time.deltaTime * 10);
                }
            }

            //Smoke
            if (smoke != null)
            {
                smoke.grounded = grounded;
            }
			
			//Zapped
			if(zapped) {
				zapTimer += Time.deltaTime;
				if(zapTimer < timeZapped) {
					transform.localScale = new Vector3(originalSize, originalSize, originalSize)/2;
					AutoBrake = true;
                    zap_particles.Play();
                    
                    GameObject music = GameObject.Find("IngameMusic");
                    if(music != null){
                        AudioSource source = (AudioSource)music.GetComponent(typeof(AudioSource));
                        source.pitch = timeZapped-zapTimer+1;
                    }
				}else{
					transform.localScale = new Vector3(originalSize, originalSize, originalSize);
					AutoBrake = false;
                    zap_particles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
					
					zapTimer = 0;
					zapped = false;
				}
			}

            //Confused
			if(confused) {
				confuseTimer += Time.deltaTime;
                GameObject music = GameObject.Find("IngameMusic");
                if(music != null){
                    AudioSource source = (AudioSource)music.GetComponent(typeof(AudioSource));
                    source.pitch = confuseTimer/4;
                }
				if(confuseTimer > timeConfused) {				
					confuseTimer = 0;
					confused = false;
                    if(music != null){
                        AudioSource source = (AudioSource)music.GetComponent(typeof(AudioSource));
                        source.pitch = 1;
                    }
				}
			}
        }

        private void OnTriggerEnter(Collider other)
        {
            var c = other.GetComponent<Checkpoint>();

            if (c)
            {
                if (CheckpointPassed != null)
                    CheckpointPassed(this, new CheckpointPassArgs(c));
            }

            customGravity cm = other.GetComponent<customGravity>();
            if (cm)
                cm.manageGrav(this);

            if (other.GetComponent<TriggerRespawn>())
                RequestRespawn();
        }

        private void OnCollisionStay(Collision c)
        {
            //Enable grounded and reset timer
            grounded = true;
            groundedTimer = 0;
			jumpsRemaining = extraJumps;
            Up = c.contacts[0].normal;
        }

        private void OnCollisionExit(Collision c)
        {
            //Disable grounded when timer is done
            groundedTimer = 0.08f;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Up);
        }

        public void CreateRemovalParticles()
        {
            //TODO: Create a special version of the particle system for Super Sanic that has a cloud of pot leaves instead. No, really.
            Instantiate(prefabs.RemovalParticles, transform.position, transform.rotation);
        }
    }
}
