using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damageAmount = 25;

    // Destroi a flexa após 10 segundos do lançamento.
    private void Start()
    {
        Destroy(gameObject, 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(transform.GetComponent<Rigidbody>());

        // Quando a flexa colide com um Inimigo, ele recebe dano. 
        if(other.tag =="Enemy")
        {
            transform.parent = other.transform;
            other.GetComponent<Enemy>().TakeDamage(damageAmount);
        }
    }
}
