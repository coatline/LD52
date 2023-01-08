using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform parent;
    [SerializeField] bool generate;
    [SerializeField] bool delete;
    [SerializeField] int size;
    [SerializeField] bool f;

    private void OnValidate()
    {
        //if (f)
        //    print(EnemySpawner.I);

        if (delete)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        if (generate)
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity, parent);
                }
            }
        }
    }
}
