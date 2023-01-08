using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BOOM!", menuName = "Sound")]

public class Sound : ScriptableObject
{
    [SerializeField] AudioClip[] clips;

    public AudioClip RandomSound => clips[Random.Range(0, clips.Length)];
}
