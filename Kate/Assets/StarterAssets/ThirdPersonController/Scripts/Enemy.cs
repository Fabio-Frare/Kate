using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, ILevarDano
{
    private NavMeshAgent agente;
    private GameObject player;
    public Animator anim;
    public float distanciaDoAtaque = 2.0f;
    public int life = 100;
    
    
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        VaiAtrasJogador();
        OlharParaJogador();
    }

    private void VaiAtrasJogador()
    {
        float distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);
        if(distanciaDoPlayer < distanciaDoAtaque) 
        {
            agente.isStopped = true;
            
            anim.SetTrigger("ataque");
            anim.SetBool("podeAndar", false);
            anim.SetBool("pararAtaque", false);
            CorrigirRigiEntrar();
        } 
        // Player se afastou
        if(distanciaDoPlayer >= distanciaDoAtaque + 1)
        {
            anim.SetBool("pararAtaque", true);
            CorrigirRigiSair();
        }

        if(anim.GetBool("podeAndar"))
        {
            agente.isStopped = false;
            agente.SetDestination(player.transform.position);
            anim.ResetTrigger("ataque");
        }
    }

    private void OlharParaJogador()
    {
        Vector3 direcaoOlhar = player.transform.position - transform.position;
        Quaternion rotacao = Quaternion.LookRotation(direcaoOlhar);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacao, Time.deltaTime * 300);
    }

    private void CorrigirRigiEntrar()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private void CorrigirRigiSair()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void TakeDamage(int damageAmount)
    {
        life -= damageAmount;

        if(life <= 0)
        {
            AudioManager.instance.Play("EnemyDeath");
            anim.SetTrigger("die");
            GetComponent<Collider>().enabled = false;
        } else
        {   
            AudioManager.instance.Play("EnemyDamage");
            anim.SetTrigger("damage");
        }
    }

}
