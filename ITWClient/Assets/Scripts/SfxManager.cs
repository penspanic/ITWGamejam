using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SfxType
{
    // UI

    // InGame-Character

    // InGame-Item
    Item_Create,
    Item_GetHp,
    Item_GetMp,
    Item_GetExtreme,
}

public class SfxManager : Singleton<SfxManager>
{
    public bool IsInitialized { get; private set; }

    private Dictionary<SfxType, AudioClip> clips = new Dictionary<SfxType, AudioClip>();
    private AudioSource source = null;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        source = gameObject.AddComponent<AudioSource>();
        SoundManager.SfxVolume = 1; // 임시
        source.volume = SoundManager.SfxVolume;
    }

    public void Initialize()
    {
        if(IsInitialized == true)
        {
            return;
        }
        IsInitialized = true;

        clips.Add(SfxType.Item_Create, Resources.Load<AudioClip>("Sounds/SFX_Object/SFX_Object_Item_Create"));
        clips.Add(SfxType.Item_GetHp, Resources.Load<AudioClip>("Sounds/SFX_Object/SFX_Object_Item_GetHp"));
        clips.Add(SfxType.Item_GetMp, Resources.Load<AudioClip>("Sounds/SFX_Object/SFX_Object_Item_GetMp"));
        clips.Add(SfxType.Item_GetExtreme, Resources.Load<AudioClip>("Sounds/SFX_Object/SFX_Object_Item_GetExtreme"));

    }

    public void Play(SfxType type)
    {
        if(clips.ContainsKey(type) == false)
        {
            Debug.LogError("Sfx not loaded, type : " + type.ToString());
            return;
        }
        Debug.Log("Sfx Play : " + type.ToString() + " Volume : " + source.volume);
        source.PlayOneShot(clips[type]);
    }
}