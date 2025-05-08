using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentAI : MonoBehaviour
{
    [Header("Opponent Movement")]
    public float movementSpeed = 1f;
    public float rotationSpeed = 10f;
    public CharacterController characterController;
    public Animator animator;

    [Header("Opponent Fight")]
    public float attackCooldown = 0.5f;
    public int attackDamages = 5;
    public string[] attackAnimations = { "Attack1Animation", "Attack2Animation", "Attack3Animation", "Attack4Animation" };
    public float dodgeDistance = 2f;
    public int attackCount = 0;
    public int randomNumber;
    public float attackRadius = 2f;
    public FightingController[] fightingControllers;
    public Transform[] players;
    public bool isTakingDamage;
    private float lastAttackTime;

    [Header("Effect and Sound")]
    public ParticleSystem attack1Effect;
    public ParticleSystem attack2Effect;
    public ParticleSystem attack3Effect;
    public ParticleSystem attack4Effect;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        createRandomNumber();
    }

    void Update()
    {
        Transform closestPlayer = FindClosestPlayer();

        if (closestPlayer != null)
        {
            MoveTowardsPlayer(closestPlayer);

            if (Vector3.Distance(transform.position, closestPlayer.position) < attackRadius)
            {
                PerformAttack(Random.Range(0, attackAnimations.Length));
            }
        }
    }

    Transform FindClosestPlayer()
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform player in players)
        {
            if (player.gameObject.activeSelf)
            {
                float distance = Vector3.Distance(transform.position, player.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = player;
                }
            }
        }
        return closest;
    }

    void MoveTowardsPlayer(Transform targetPlayer)
    {
        Vector3 direction = (targetPlayer.position - transform.position).normalized;
        characterController.Move(direction * movementSpeed * Time.deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        animator.SetBool("Walking", true);
    }

    void PerformAttack(int attackIndex)
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            animator.Play(attackAnimations[attackIndex]);
            Debug.Log($"Performed attack {attackIndex + 1}, dealing {attackDamages} damage");
            lastAttackTime = Time.time;

            switch (attackIndex)
            {
                case 0: attack1Effect.Play(); break;
                case 1: attack2Effect.Play(); break;
                case 2: attack3Effect.Play(); break;
                case 3: attack4Effect.Play(); break;
            }
        }
    }

    void PerformDodgeFront()
    {
        animator.Play("DodgeFrontAnimation");

        Vector3 dodgeDirection = -transform.forward * dodgeDistance;
        characterController.SimpleMove(dodgeDirection);
    }

    void createRandomNumber()
    {
        randomNumber = Random.Range(1, 5);
    }
}
