using UnityEngine;

namespace Sanicball
{
    public class MenuCamera : MonoBehaviour
    {
        public bool fadingEnabled = true;
        public bool movingEnabled = true;
        public StandardShaderFade fade;
        public MenuCameraPath[] paths;
        public Transform lookTarget;

        public float moveSpeed = 0.2f;
        public float fadeTime = 0.4f;

        private int currentPath = 0;
        private float changePathTimer;

        // Use this for initialization
        private void Start()
        {
            if(fadingEnabled)
                CameraFade.StartAlphaFade(Color.black, true, 5);

            if(movingEnabled)
                transform.position = paths[currentPath].startPoint.position;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!movingEnabled) return;
            float dist = Vector3.Distance(transform.position, paths[currentPath].endPoint.position);

            //Movement
            transform.position = Vector3.MoveTowards(transform.position, paths[currentPath].endPoint.position, moveSpeed * Time.deltaTime);

            //Calculate time before hitting end point
            float timeToChange = dist / moveSpeed;

            //Start fading in if close enough to end point
            if (timeToChange < fadeTime && changePathTimer <= 0f)
            {
                changePathTimer = fadeTime;
                fade.FadeIn(fadeTime);
            }

            //Change path and begin fading out when fadein is done
            if (changePathTimer > 0f)
            {
                changePathTimer -= Time.deltaTime;
                if (changePathTimer <= 0f)
                {
                    ChangePath();
                    fade.FadeOut(fadeTime);
                }
            }

            //Look at target
            transform.LookAt(lookTarget.position);
        }

        private void ChangePath()
        {
            currentPath++;
            if (currentPath >= paths.Length)
                currentPath = 0;
            transform.position = paths[currentPath].startPoint.position;

            //TODO: code betr
            lookTarget.GetComponent<CycleMaterial>().Switch();
        }
    }
}