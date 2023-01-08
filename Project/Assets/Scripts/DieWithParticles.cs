using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieWithParticles : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] bool useParticleLifetime;
    [SerializeField] int emitCount;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] Sound deathSound;

    [Header("Other")]
    [SerializeField] MeshRenderer parentRenderer;
    [SerializeField] float destroyDelay;
    Color particleColor;

    private void Start()
    {
        particleColor = parentRenderer.material.color;
    }

    public void Begin()
    {
        if (transform.parent != null)
        {
            GameObject parent = transform.parent.gameObject;
            transform.SetParent(null);
            Destroy(parent);
        }

        ParticleSystem.MainModule settings = particleSystem.main;
        settings.startColor = new ParticleSystem.MinMaxGradient(particleColor);
        audioSource.PlayOneShot(deathSound.RandomSound);
            
        particleSystem.Emit(emitCount);
        StartCoroutine(DestroyTimer());
    }

    IEnumerator DestroyTimer()
    {
        float delay = destroyDelay;

        if (useParticleLifetime)
            delay = particleSystem.main.startLifetime.constant;

        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}
