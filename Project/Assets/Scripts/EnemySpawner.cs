using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] float interval;
    List<Enemy> enemies;

    private void Start()
    {
        enemies = new List<Enemy>();
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        Vector3 position = transform.GetChild(Random.Range(0, transform.childCount)).position;

        Enemy e = Instantiate(DataLibrary.I.Enemies.Rand, position, Quaternion.identity);
        e.Died += EnemyDied;
        enemies.Add(e);

        yield return new WaitForSeconds(interval);

        StartCoroutine(Spawn());
    }

    void EnemyDied(Enemy e) => enemies.Remove(e);

    public Enemy GetClosestEnemy(Vector3 position)
    {
        float closestVal = float.MaxValue;
        Enemy closest = null;

        foreach (Enemy e in enemies)
        {
            float dist = Vector2.Distance(position, e.transform.position);

            if (dist < closestVal)
            {
                closestVal = dist;
                closest = e;
            }
        }

        return closest;
    }

    public Enemy GetRandomEnemy()
    {
        if (enemies.Count == 0) return null;
        return enemies[Random.Range(0, enemies.Count)];
    }

    private void Update()
    {
        if (interval > .2f)
            interval -= Time.deltaTime / 45;
    }
}
