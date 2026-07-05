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
            // <insert animation here>
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,30,-80), ForceMode.Impulse);
            // </insert animation here>
            Destroy(gameObject, 2f); // 2f er standin for animation.
        }
    }
}
