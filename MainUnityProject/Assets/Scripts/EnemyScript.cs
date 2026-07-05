using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>().gameObject;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(agent.isActiveAndEnabled)
            agent.SetDestination(player.transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHit"))
        {
            agent.enabled = false;
            // insert animation here
            GetComponent<Rigidbody>().AddRelativeForce(Vector3.back * 100, ForceMode.Impulse);
            Destroy(gameObject, 2f); // 2f er standin for animation.
        }
    }
}
