using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, ILevarDano
{
    private NavMeshAgent agente;
    public GameObject player;
    public Animator anim;
    public float distanciaDoAtaque = 2.0f;
    private int life = 100;
    public Slider healthBar;
    private FieldOfView fov;
     private PatrulharAleatorio pal;
    
    
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        healthBar.gameObject.SetActive(false); // Barra de vida inicia invisível.
        fov = GetComponent<FieldOfView>();
        pal = GetComponent<PatrulharAleatorio>();
    }

    void Update()
    {
        //VaiAtrasJogador();
        //OlharParaJogador();
        UpdateLife();
    
        if (fov.podeVerPlayer)
        {
            VaiAtrasJogador();
        } else 
        {
            anim.SetBool("pararAtaque", true);
            CorrigirRigiSair();
            agente.isStopped = false;
            pal.Andar();
        }
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
            agente.isStopped = true;
            AudioManager.instance.Play("EnemyDeath");
            anim.SetTrigger("die");
            GetComponent<Collider>().enabled = false;
            healthBar.gameObject.SetActive(false);
            CorrigirRigiSair();
            Destroy(gameObject, 7); // inimigo desaparece do jogo após um tempo morto. 
        } else
        {   
            healthBar.gameObject.SetActive(true);
            AudioManager.instance.Play("EnemyDamage");
            anim.SetTrigger("damage");
        }
    }

    // Atualiza a barra de vida do inimigo.
    private void UpdateLife() 
    {
        healthBar.value = life; 
    }

    // Inimigo dá dano atualizando a barra de vida da Kate. Está com erro
    /*public void DarDano()
    {
       player.GetComponent<StarterAssets>().AtualizarVida(-10);
    }*/

   

}
