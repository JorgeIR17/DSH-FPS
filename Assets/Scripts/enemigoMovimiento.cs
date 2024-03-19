using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemigoMovimiento : LivingEntity
{
    UnityEngine.AI.NavMeshAgent pathFinder;
    Transform target;
    // Start is called before the first frame update
    float myCollisionradius;
    float TargetCollisionradius;

    float DistanciadeAtaque = 1.5f;
    float NextAttackTime = 0;
    float TimeBetweenAttack = 0.5f;
    bool Atacando = false;

    LivingEntity targetEntity;
    float damage = 1;
    bool bFinalPartida = false;

    public override void Start()
    {
        base.Start();
        pathFinder = GetComponent<UnityEngine.AI.NavMeshAgent>();
        // Encuentra el objeto que tenga el tag "Player" y asigna su transformada como objetivo
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // Obtiene el radio de colisión del enemigo y del objetivo
        myCollisionradius = GetComponent<CapsuleCollider>().radius;
        if (target.GetComponent<CapsuleCollider>() != null)
            TargetCollisionradius = target.GetComponent<CapsuleCollider>().radius;
        else
            TargetCollisionradius = 0.5f; // Valor predeterminado si el objetivo no tiene un CapsuleCollider
        targetEntity = target.GetComponent<LivingEntity>();
        jugador.OnDeathPlayer += FinalPartida;
    }

    void FinalPartida()
    {
        bFinalPartida = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!bFinalPartida){
            if(!Atacando){
                Vector3 dirtoTarget = (target.position - transform.position).normalized;
                Vector3 TargetPosition = target.position - dirtoTarget * (myCollisionradius + TargetCollisionradius + DistanciadeAtaque);
                pathFinder.SetDestination(TargetPosition);

                if(Time.time > NextAttackTime)
                {
                    NextAttackTime = Time.time + TimeBetweenAttack;
                    float sqrDsttoTarget = (target.position - transform.position).sqrMagnitude;
                    if (sqrDsttoTarget <= Mathf.Pow(myCollisionradius + TargetCollisionradius + DistanciadeAtaque, 2))
                    {
                        Debug.Log("estoy al lado");
                        StartCoroutine(Attack());
                    }
                }
            }
        }
    }
    IEnumerator Attack()
    {
        if(!bFinalPartida){
            pathFinder.enabled = false;
            Atacando = true;

            Vector3 originalPosition = transform.position;
            Vector3 dirtoTarget = (target.position - transform.position).normalized;
            Vector3 AttackPosition = target.position - dirtoTarget * (myCollisionradius + TargetCollisionradius);

            float percent = 0;
            float attackSpeed = 1;

            bool hasAppliedDamage = false;
            while(percent <= 1)
            {
                if(percent >= 0.5f && !hasAppliedDamage)
                {
                    targetEntity.TakeDamage(damage);
                    hasAppliedDamage = true;
                }
                percent += Time.deltaTime * attackSpeed;
                float interpolacion = (-Mathf.Pow(percent,2)+percent)*4;
                transform.position = Vector3.Lerp(originalPosition, AttackPosition, interpolacion);
                yield return null;
                
            }
            pathFinder.enabled = true;
            Atacando = false;
        }
    }
}
