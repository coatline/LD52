using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-2)]
public class DataLibrary : Singleton<DataLibrary>
{
    public Getter<Enemy> Enemies { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Enemies = new Getter<Enemy>(Resources.LoadAll<Enemy>("Prefabs/Enemies"));
    }
}
