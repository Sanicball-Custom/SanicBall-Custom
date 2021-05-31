using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanicball.Gameplay;

public interface IAbility { }

public interface IAbilityJumpOverride : IAbility {
    public void Jump(Ball ball, bool hold);
    public void TouchGround(Ball ball);
}
public interface IAbilityUpdate : IAbility {
    public void Update(Ball ball);
    public void FixedUpdate(Ball ball);
}

// Attribute to let the ball know this field has to be modified
[System.AttributeUsage(System.AttributeTargets.Field)]
public class ExposeToCharacterAttribute : System.Attribute {
    public ExposeToCharacterAttribute() { }
}