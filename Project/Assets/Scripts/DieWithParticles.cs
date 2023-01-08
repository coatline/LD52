using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieWithParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] MeshRenderer parentRenderer;
    [SerializeField] bool useParticleLifetime;
    [SerializeField] float destroyDelay;
    [SerializeField] int emitCount;
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
