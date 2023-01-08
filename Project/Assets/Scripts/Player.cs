using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    [SerializeField] Volume volume;

    Color initialBloomColor;
    Color initialVignetteColor;
    Vignette vignette;
    Bloom bloom;

    [SerializeField] float maxHealth;
    float health;

    [SerializeField] Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] Camera cam;
    Vector3 currentVelocity;

    bool canJump;

    void Start()
    {
        health = maxHealth;

        volume.profile.TryGet(out bloom);
        initialBloomColor = (Color)bloom.tint;

        volume.profile.TryGet(out vignette);
        initialVignetteColor = (Color)vignette.color;
    }

    void Update()
    {
        currentVelocity = new Vector3(Input.GetAxisRaw("Horizontal"), currentVelocity.y, Input.GetAxisRaw("Vertical")) * Time.fixedDeltaTime;
        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
            currentVelocity.y = 5;
    }

    private void FixedUpdate()
    {
        Vector3 direction = ((transform.forward * currentVelocity.z) + (transform.right * currentVelocity.x)).normalized * speed;
        rb.velocity = new Vector3(direction.x, rb.velocity.y + currentVelocity.y, direction.z);
    }

    public void Hurt()
    {
        health--;
        ShowDamage();
    }

    void ShowDamage()
    {
        bloom.tint = new ColorParameter(Color.Lerp(initialBloomColor, Color.red, 1 - (health / maxHealth)), true);
        vignette.color = new ColorParameter(Color.Lerp(initialVignetteColor, Color.red, 1 - (health / maxHealth)), true);
    }

    //bool CanJump()
    //{
    //return Physics.Raycast
    //}
}
