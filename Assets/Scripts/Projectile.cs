using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] private float speed = 10f;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _rigidbody.velocity = transform.forward * speed;
        StartCoroutine(DestroyProjectile());
    }

    private void OnTriggerEnter(Collider other) //other tiene que ser una variable? Enemy?
    {
        Destroy(gameObject);
    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
