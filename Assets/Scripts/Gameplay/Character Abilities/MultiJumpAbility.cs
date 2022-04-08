using Sanicball.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiJumpAbility : MonoBehaviour, IAbilityJumpOverride {
    [ExposeToCharacter]
    public int extraJumps = 2;
    private int remainingJumps = 2;

    public void Awake() {
        remainingJumps = extraJumps;
    }

    public void Jump(Ball ball, bool hold) {
        if (ball.canMove) {
            if ((ball.grounded || remainingJumps > 0) && !hold) {
                float scalar = Vector3.Dot((-ball.gravDir).normalized, ball.rb.velocity.normalized);
                if (scalar < 0)
                    ball.rb.velocity -= (-ball.gravDir) * scalar * ball.rb.velocity.magnitude;

                ball.rb.AddForce(ball.Up * ball.characterStats.jumpHeight, ForceMode.Impulse);
                if (ball.sounds.Jump != null) {
                    ball.sounds.Jump.Play();
                }
                if (!ball.grounded) {
                    remainingJumps--;
                }
                ball.grounded = false;
            }
        }
    }

    public void TouchGround(Ball ball) {
        remainingJumps = extraJumps;
    }
}
