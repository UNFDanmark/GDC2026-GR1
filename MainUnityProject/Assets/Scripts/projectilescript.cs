using UnityEngine;

public class projectilescript : MonoBehaviour
{

    Rigidbody rb;
    public float ProjectileSpeed;
    public float InitialSpeed;
    public float LifeTime;
    GameObject target;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = FindAnyObjectByType<PlayerController>().gameObject;
        rb.AddForce(new Vector3(0, InitialSpeed*rb.linearDamping, 0), ForceMode.Impulse);
        Destroy(gameObject, LifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(Time.deltaTime*ProjectileSpeed*rb.linearDamping*(target.transform.position - transform.position));
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PlayerHit")) Destroy(gameObject);
    }
}
