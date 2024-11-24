using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "Collection Menu/Sound", order = 0)]
public class SoundCollection : ScriptableObject
{
    public List<Sound> sounds;
}

[Serializable]
public class Sound
{
    public int id;
    public string name;
    public AudioClip clip;
    [HideInInspector] public AudioSource source;
    [HideInInspector] public float volume = 1;
    [HideInInspector] public float pitch = 1;
    [HideInInspector] public float delay = 0;
    [HideInInspector] public bool isLoop = false;
}
