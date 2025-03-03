using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneResources : MonoBehaviour
{
    public static SceneResources Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        foreach (var c in clips)
        {
            _clips.Add(c.name, c);
        }
        foreach (var s in sprites)
        {
            _sprites.Add(s.name, s);    
        }
    }

    [Header("Common used Audio")]
    public List<AudioClip> clips = new List<AudioClip>();
    private Dictionary<string, AudioClip> _clips = new Dictionary<string, AudioClip>();
    public AudioClip GetAudioClip(string name)
    {
        return _clips[name];
    }

    [Header("Common used Sprite")]
    public List<Sprite> sprites = new List<Sprite>();
    private Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
    public Sprite GetSprite(string name)
    {
        return _sprites[name];
    }
}
