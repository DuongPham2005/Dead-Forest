using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform target;

    public float radius = 10f;
    public Vector3 originalePosition;
    public float maxDistance = 50f;

    public Animator animator;

    public float maxHP;
    public float currentHP;

    // Update is called once per frame

    public enum CharacterState
    {
        Normal,
        Attack,
        Die
    }
    public CharacterState currentState;
    void Start()
    {
        originalePosition = transform.position;
        currentHP = maxHP;
    }
    void Update()
    {
        Wander();
        if(target != null)
        {
            var lookPos = target.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5);
        }
        if(currentState == CharacterState.Die)
        {
            return;
        }
        var distanceToOriginal = Vector3.Distance(originalePosition, transform.position);
        var distance = Vector3.Distance(target.position, transform.position);
        if (distance <= radius && distanceToOriginal <= maxDistance)
        {
            navMeshAgent.SetDestination(target.position);
            animator.SetFloat("speed", navMeshAgent.velocity.magnitude);

            distance = Vector3.Distance(target.position, transform.position);
            if (distance < 2f)
            {
                ChangeState(CharacterState.Attack);
            }
        }
        if (distance > radius || distanceToOriginal > maxDistance)
        {
            navMeshAgent.SetDestination(originalePosition);
            animator.SetFloat("speed", navMeshAgent.velocity.magnitude);

            distance = Vector3.Distance(originalePosition, transform.position);
            if(distance < 1f)
            {
                animator.SetFloat("speed", 0);
            }
            ChangeState(CharacterState.Normal);
        }
    }
    private void ChangeState(CharacterState newState)
    {
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
            case CharacterState.Die:
                animator.SetTrigger("Die");
                Destroy(gameObject, 3f);
                break;
        }
        currentState = newState;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Math.Max(0 , currentHP);
        if(currentHP <= 0)
        {
            ChangeState(CharacterState.Die);
        }

    }
    public void Wander()
    {
        var randomDirection = Random.insideUnitSphere * radius;
        randomDirection += originalePosition;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, radius, 1);
        var finalPosition = hit.position;
        navMeshAgent.SetDestination(finalPosition);
        animator.SetFloat("speed", navMeshAgent.velocity.magnitude);
    }
}
