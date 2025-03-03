using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Volume Mixers")]
    public AudioMixer mixer;

    [Header("Audio Sources")]
    public AudioSource musicSource;

    [Header("Audio Settings")]
    [Range(0.0001f, 1f)] public float musicVolume = 0.5f;
    [Range(0.0001f, 1f)] public float masterVolume = 0.5f;
    [Range(0.0001f, 1f)] public float sfxVolume = 0.5f;

    [Header("SFX Pool")]
    public GameObject audioSourcePrefab;
    public int poolSize = 10;
    private Queue<AudioSource> sfxPool;

    private void Awake()
    {
        Instance = this;

        InitializeSFXPool();
    }

    private void Start()
    {
        UpdateVolumes();
    }

    private void InitializeSFXPool()
    {
        sfxPool = new Queue<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newAudioSource = Instantiate(audioSourcePrefab, transform);
            AudioSource source = newAudioSource.GetComponent<AudioSource>();
            source.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
            newAudioSource.SetActive(false);
            sfxPool.Enqueue(source);
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        if (sfxPool.Count > 0)
        {
            AudioSource source = sfxPool.Dequeue();
            source.gameObject.SetActive(true);
            return source;
        }
        return null;
    }

    private void ReturnAudioSourceToPool(AudioSource source)
    {
        source.Stop();
        source.clip = null;
        source.gameObject.SetActive(false);
        sfxPool.Enqueue(source);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip, float pitch = 1f, float volume = 1f)
    {
        AudioSource source = GetAvailableAudioSource();
        if (source != null)
        {
            source.pitch = pitch;
            source.volume = volume;
            source.clip = clip;
            source.Play();

            StartCoroutine(ReturnSourceToPoolAfterDuration(source, clip.length / Mathf.Abs(pitch)));
        }
        else
        {
            Debug.LogWarning("No available AudioSource in pool!");
        }
    }

    private IEnumerator ReturnSourceToPoolAfterDuration(AudioSource source, float duration)
    {
        //print(duration);
        yield return new WaitForSeconds(duration);
        ReturnAudioSourceToPool(source);
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        //print("okay");
        UpdateVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        //print("okay1");
        UpdateVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        //print("okay2");
        UpdateVolumes();
    }

    private void UpdateVolumes()
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
        mixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        mixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
    }
}
