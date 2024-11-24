using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private SoundCollection soundCollection;
    private List<Sound> sounds;
    #region Singleton
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }
    #endregion
    void Start()
    {
        DataMananger.instance.LoadData();
        sounds = soundCollection.sounds;
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.isLoop;
        }
    }

    public void Play(int id)
    {
        if (!DataMananger.instance.gameSave.sounds) return;
        List<Sound> sounds = new List<Sound>();
        foreach (Sound s in this.sounds)
        {
            if (s.id == id) sounds.Add(s);
        }
        Sound sound = sounds[Random.Range(0, sounds.Count)];
        sound.source.Play();
    }
    public void Stop()
    {
        if (!DataMananger.instance.gameSave.sounds) return;
        foreach (Sound sound in sounds)
        {
            sound.source.Stop();
        }
    }
}
