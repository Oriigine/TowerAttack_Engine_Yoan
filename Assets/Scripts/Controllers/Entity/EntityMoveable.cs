﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EntityMoveable : Entity
{
    [Range(1, 50)]
    public float moveSpeed = 1;

    // Range de detection
    public CapsuleCollider detectionRange;

    [Header("Target")]
    // Variable target
    public GameObject globalTarget;
    //public GameObject globalTarget;
    
    [Header("Stop Time")]
    // Variable de temps d'arret
    public float timeWaitBeforeMove = 1;
    private float m_CurrentTimeBeforeNextMove = 0;
    
    private NavMeshAgent m_NavMeshAgent;



    // Initialisation - Construction de l'entité
    public override void InitEntity()
    {
        base.InitEntity();        
        Debug.Log("Modif Tealrocks");
        Debug.Log("Coucou Modif Pedro");
        // Initialisation - Construction
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    public override void RestartEntity()
    {
        base.RestartEntity();

        // Set/Restart properties
        m_NavMeshAgent.speed = moveSpeed;
        SetDestination();
    }

    public void SetGlobalTarget(GameObject target)
    {
        globalTarget = target;
        SetDestination();
    }

    private void SetDestination()
    {
        if (globalTarget)
        {
            m_NavMeshAgent.SetDestination(globalTarget.transform.position);
        }
    }

    public override void Update()
    {
        
        if(m_NavMeshAgent.isStopped)
        {
            if (m_CurrentTimeBeforeNextMove < timeWaitBeforeMove)
            {
                m_CurrentTimeBeforeNextMove += Time.deltaTime;
            }
            else
            {
                m_NavMeshAgent.isStopped = false;
                SetDestination();
            }
        }
        //base.Update();
    }

    protected override bool DoAttack(Entity targetEntity)
    {
        if(base.DoAttack(targetEntity))
        {
            Debug.Log("Attack!");
            m_NavMeshAgent.isStopped = true;
            m_CurrentTimeBeforeNextMove = 0;
            return true;
        }
        return false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other != null)
        {
            Debug.Log("Enemy Detected");
            m_NavMeshAgent.SetDestination(other.transform.position);
        }
        else
        {
            m_NavMeshAgent.SetDestination(globalTarget.transform.position);
        }
    }
}