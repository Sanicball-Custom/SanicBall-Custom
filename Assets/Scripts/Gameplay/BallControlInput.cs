using UnityEngine;
using SanicballCore;

namespace Sanicball.Gameplay
{
    [RequireComponent(typeof(Ball))]
    public class BallControlInput : MonoBehaviour
    {
        private Ball ball;
        private Vector3 rawDirection;
        private bool hasJumped = false;

        public Quaternion LookDirection { get; set; }

        private void Start()
        {
            LookDirection = Quaternion.Euler(Vector3.forward);
            ball = GetComponent<Ball>();
        }

        private void Update()
        { //Short circuit if paused
            if (UI.PauseMenu.GamePaused)
            {
                ball.DirectionVector = Vector3.zero;
                return;
            }

            //GO FAST
            const float weight = 0.5f;

            var targetVector = GameInput.MovementVector(ball.CtrlType);
            rawDirection = Vector3.MoveTowards(rawDirection, targetVector, weight);
            Vector3 directionVector = rawDirection;

            if (directionVector != Vector3.zero)
            {
                //Modify direction vector to be more controller-friendly (And normalize it)
                var directionLength = directionVector.magnitude;
                directionVector = directionVector / directionLength;
                directionLength = Mathf.Min(1, directionLength);
                directionLength = directionLength * directionLength;
                directionVector = directionVector * directionLength;
            }
            if (ball.mobile) //aqui tmbn xd 
                directionVector = new Vector3(ball.joystick.Horizontal, 0, ball.joystick.Vertical).normalized;
            directionVector = LookDirection * Quaternion.Euler(0f, 90f, 0) * directionVector; //Multiply vector by camera rotation
            ball.DirectionVector = directionVector;

            //BRAKE FAST
            ball.Brake = GameInput.IsBraking(ball.CtrlType);

            //JUMP FAST
            if (GameInput.IsJumping(ball.CtrlType, false)) {
                if (!hasJumped)
                {
                    ball.Jump(false);
                    hasJumped = true;
                }
            }else if (GameInput.IsJumping(ball.CtrlType, true)) {
                ball.Jump(true);
            }else {
                if (hasJumped)
                    hasJumped = false;
            }

            //RESPAWN FAST
            if (GameInput.IsRespawning(ball.CtrlType) && ball.CanMove)
            {
                ball.RequestRespawn();
            }
			
			//POWERUP FAST
			if (GameInput.IsUsingPowerupLeft(ball.CtrlType))
            {
                ball.UsePowerup(0);
            }
			
			if (GameInput.IsUsingPowerupRight(ball.CtrlType))
            {
                ball.UsePowerup(1);
            }
        }
    }
}