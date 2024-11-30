using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterController characterController;
    public float speed = 2f;
    public Vector3 movementVevocity;
    public PlayerInput playerInput; 

    public Animator animator;
    // Update is called once per frame
    public DamageZone damageZone;
    public enum CharacterState
    {
        Normal,
        Attack
    }
    public CharacterState currentState;
    void FixedUpdate()
    {
         switch (currentState)
        {
            case CharacterState.Normal:
                CalculateMovement();
                break;
            case CharacterState.Attack:
                break;
        }
        characterController.Move(movementVevocity);        
    }
    void CalculateMovement()
    {
        if(playerInput.attackInput)
        {
            animator.SetFloat("speed", 0);
            ChangeState(CharacterState.Attack);
            return;
        }
        movementVevocity.Set(playerInput.horizontalInput, 0, playerInput.verticalInput);
        movementVevocity.Normalize();
        movementVevocity = Quaternion.Euler(0, -45, 0) * movementVevocity;
        movementVevocity *= speed * Time.deltaTime;

        animator.SetFloat("speed", movementVevocity.magnitude);
        if(movementVevocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movementVevocity);
        }
    }
    private void ChangeState(CharacterState newState)
    {
        playerInput.attackInput = false;
        switch (currentState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attack:
                break;
        }

        switch (newState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attack:
                animator.SetTrigger("Attack");
                break;
        }
        currentState = newState;
    }

    public void OnAttack01End()
    {
        ChangeState(CharacterState.Normal);
    }

    public void BeginAttack()
    {
        damageZone.BeginAttack();
    }
    public void EndAttack()
    {
       damageZone.EndAttack();
    }
}
