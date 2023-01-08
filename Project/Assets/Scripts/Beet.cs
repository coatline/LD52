using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beet : MonoBehaviour
{
    [SerializeField] DieWithParticles dieWithParticles;
    [SerializeField] float lateralPopoutRange;
    [SerializeField] MeshCollider col;
    [SerializeField] float tiltRange;
    [SerializeField] float speed;
    Vector3 rotVelocity;
    Vector3 velocity;
    Enemy target;
    bool popping;
    bool shooting;

    private void Awake()
    {
        transform.rotation = Quaternion.Euler(Random.Range(-tiltRange, tiltRange), Random.Range(0, 360f), Random.Range(-tiltRange, tiltRange));
    }

    private void Update()
    {
        if (popping)
        {
            velocity.y -= .35f;
            velocity.x = Mathf.Lerp(velocity.x, 0, Time.deltaTime * 2f);

            if (velocity.y < -1f)
            {
                velocity.y = 0;
                popping = false;
                StartCoroutine(DelayShoot());
            }
        }

        if (shooting)
        {
            if (target == null)
            {
                transform.localScale -= Vector3.one / 10;

                if (transform.localScale.x <= 0f)
                    Destroy(gameObject);
            }

            if (transform.position.magnitude > 200)
                Destroy(gameObject);
        }

        transform.Translate(velocity * Time.deltaTime, Space.World);
        transform.rotation = Quaternion.Euler(transform.rotation.x + rotVelocity.x, transform.rotation.y + rotVelocity.y, transform.rotation.z + rotVelocity.z);
    }

    void Die()
    {
        shooting = false;
        target = null;
    }

    IEnumerator DelayShoot()
    {
        yield return new WaitForSeconds(.4f);

        //target = EnemySpawner.I.GetClosestEnemy(transform.position);
        target = EnemySpawner.I.GetRandomEnemy();

        if (target == null)
        {
            Die();
            yield return null;
        }
        else
        {
            Vector3 randomFactor = new Vector3(Random.Range(-.5f, -.5f), Random.Range(-.5f, -.5f), Random.Range(-.5f, -.5f));
            Vector3 predictedPos = ((speed * Time.deltaTime) / Vector3.Distance(transform.position, target.transform.position) * target.Velocity) + target.transform.position + randomFactor;
            velocity = (predictedPos - transform.position).normalized * speed;
        }

        shooting = true;
        col.enabled = true;


        //velocity.y = ((target.transform.position.y / time) - (transform.position.y / time) - (target.transform.position.x / time) + (gravity) + (transform.position.x * (xSpeed)));
    }

    public void Fire()
    {
        popping = true;
        velocity.y = 12.5f + Random.Range(-.5f, .5f);
        velocity.x = Random.Range(-lateralPopoutRange, lateralPopoutRange);
        rotVelocity = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), Random.Range(-1, 1f)) * 15;
    }

    public void HitSomething()
    {
        target = null;
        dieWithParticles.Begin();
    }
}
