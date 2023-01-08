using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmland : MonoBehaviour
{
    public System.Action BeetHarvested;

    [SerializeField] float reloadInterval;
    [SerializeField] Beet turnipPrefab;
    [SerializeField] Collider trigger;
    [SerializeField] float randomRot;
    [SerializeField] float randomTilt;
    Beet turnip;
    float timer;

    void Start()
    {
        trigger.enabled = false;
        transform.rotation = Quaternion.Euler(Random.Range(-randomTilt, randomTilt), Random.Range(-randomRot, randomRot), Random.Range(-randomTilt, randomTilt));
        StartReload();
    }

    void Update()
    {
        if (timer > reloadInterval)
        {
            trigger.enabled = true;
            return;
        }

        timer += Time.deltaTime;

        float percentage = Mathf.Min(timer / reloadInterval, 1);

        turnip.transform.localScale = Vector3.one * percentage;
    }

    void Fire()
    {
        trigger.enabled = false;

        BeetHarvested?.Invoke();
        turnip.Fire();
        StartReload();

        timer = 0;
    }

    void StartReload()
    {
        turnip = Instantiate(turnipPrefab, transform.position, Quaternion.Euler(180, Random.Range(0, 360f), 0));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            Fire();
    }
}
