using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmland : MonoBehaviour
{
    [SerializeField] float reloadInterval;
    [SerializeField] Beet turnipPrefab;
    [SerializeField] float randomRot;
    [SerializeField] float randomTilt;
    Beet turnip;
    float timer;

    void Start()
    {
        transform.rotation = Quaternion.Euler(Random.Range(-randomTilt, randomTilt), Random.Range(-randomRot, randomRot), Random.Range(-randomTilt, randomTilt));
        StartReload();
    }

    void Update()
    {
        if (timer > reloadInterval) return;

        timer += Time.deltaTime;

        float percentage = Mathf.Min(timer / reloadInterval, 1);

        turnip.transform.localScale = Vector3.one * percentage;
    }

    public void TryShoot()
    {
        if (timer > reloadInterval)
        {
            turnip.Fire();
            StartReload();

            timer = 0;
        }

    }

    void StartReload()
    {
        turnip = Instantiate(turnipPrefab, transform.position, Quaternion.Euler(180, Random.Range(0, 360f), 0));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            TryShoot();
    }
}
