using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] Sound takeDamageSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] Sound stepSound;
    bool isStepping;

    [Header("Effects")]
    [SerializeField] Volume volume;
    float initialVignetteIntensity;
    Color initialVignetteColor;
    Vignette vignette;
    float damageVisual;

    [Header("Properties")]
    [SerializeField] float maxHealth;
    [SerializeField] float speed;
    Vector3 currentVelocity;
    bool jumping;
    float health;

    [Header("Other")]
    [SerializeField] Collider beetTrigger;
    [SerializeField] Transform jumpPoint;
    [SerializeField] Rigidbody rb;
    [SerializeField] Camera cam;

    void Start()
    {
        rb.maxAngularVelocity = 0;
        health = maxHealth;

        volume.profile.TryGet<Vignette>(out vignette);
        initialVignetteIntensity = vignette.intensity.value;
        initialVignetteColor = (Color)vignette.color;
    }

    void Update()
    {
        currentVelocity = new Vector3(Input.GetAxisRaw("Horizontal"), currentVelocity.y, Input.GetAxisRaw("Vertical")) * Time.fixedDeltaTime;
        rb.angularVelocity = Vector3.zero;

        if (currentVelocity.magnitude > 0 && Grounded)
            if (isStepping == false)
            {
                audioSource.PlayOneShot(stepSound.RandomSound);
                StartCoroutine(StepSound());
            }

        damageVisual = Mathf.Lerp(damageVisual, 0, Time.deltaTime);

        if (jumping == false)
            if (Input.GetKeyDown(KeyCode.Space))
                if (Grounded)
                {
                    audioSource.PlayOneShot(jumpSound);
                    StartCoroutine(DoJump());
                    currentVelocity.y = 5;
                }

        vignette.color.SetValue(new ColorParameter(Color.Lerp(initialVignetteColor, Color.red, damageVisual), true));
        vignette.intensity.SetValue(new FloatParameter(Mathf.Lerp(initialVignetteIntensity, .35f, damageVisual), true));
    }

    bool Grounded => Physics.Raycast(jumpPoint.position, Vector3.down, .1f);

    private void FixedUpdate()
    {
        Vector3 direction = ((transform.forward * currentVelocity.z) + (transform.right * currentVelocity.x)).normalized * speed;
        rb.velocity = new Vector3(direction.x, rb.velocity.y + currentVelocity.y, direction.z);
        currentVelocity.y = 0;
    }

    bool hurtSoundPlaying;

    public void Hurt()
    {
        health--;

        if (!hurtSoundPlaying)
        {
            audioSource.PlayOneShot(takeDamageSound.RandomSound);
            StartCoroutine(HurtSound());
        }

        if (health <= 0)
        {
            GameEnder.I.Lose();
            beetTrigger.enabled = false;
            enabled = false;
            health = float.MaxValue;
        }

        ShowDamage();
    }

    IEnumerator DoJump()
    {
        jumping = true;
        yield return new WaitForSeconds(.25f);
        jumping = false;
    }

    IEnumerator StepSound()
    {
        isStepping = true;
        yield return new WaitForSeconds(.35f);
        isStepping = false;
    }

    IEnumerator HurtSound()
    {
        hurtSoundPlaying = true;
        yield return new WaitForSeconds(.085f);
        hurtSoundPlaying = false;
    }

    void ShowDamage()
    {
        //bloom.tint.SetValue(new ColorParameter(Color.Lerp(Color.red, initialBloomColor, health / maxHealth), true));
        damageVisual = 1;
        //vignette.color = new ColorParameter(Color.Lerp(Color.red, initialVignetteColor, health / maxHealth), true);
    }
}
