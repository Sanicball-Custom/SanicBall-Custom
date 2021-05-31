using Sanicball.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyHopAbility : MonoBehaviour, IAbilityJumpOverride {
    private bool jumped = false;

    public void Jump(Ball ball, bool hold) {
        if (ball.canMove) {
            if (ball.grounded && !jumped) {
                ball.rb.AddForce(ball.Up * ball.characterStats.jumpHeight/2f, ForceMode.Impulse);
                if (ball.sounds.Jump != null) {
                    ball.sounds.Jump.Play();
                }
                jumped = true;
                ball.grounded = false;
            }
        }
    }

    public void TouchGround(Ball ball) {
        jumped = false;
    }
}
