using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event System.Action<Enemy> Died;

    [SerializeField] DieWithParticles dieWithParticles;

    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Vector2 jumpRange;
    [SerializeField] Rigidbody rb;
    [SerializeField] Color color;
    [SerializeField] float health;
    [SerializeField] float speed;
    Vector3 scale;
    Player player;
    float jump;

    void Start()
    {
        meshRenderer.material.color = color;
        scale = transform.localScale;
        transform.localScale = Vector3.one * 0;
        StartCoroutine(Jump());
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (transform.localScale != scale)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, scale, Time.deltaTime);
        }


        Vector3 lateralVel = (player.transform.position - transform.position).normalized * speed;
        rb.velocity = new Vector3(lateralVel.x, rb.velocity.y + jump, lateralVel.z);
        jump = 0;
    }

    IEnumerator Jump()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        jump = Random.Range(jumpRange.x, jumpRange.y);
        StartCoroutine(Jump());
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            player.Hurt();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plant"))
        {
            collision.gameObject.GetComponent<Beet>().HitSomething();
            TakeDamage(1);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health < 0)
        {
            Die();
        }
    }

    void Die()
    {
        Died?.Invoke(this);
        dieWithParticles.Begin();
    }

    public Vector3 Velocity => rb.velocity;
}
