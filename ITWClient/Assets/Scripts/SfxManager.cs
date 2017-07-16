using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum SfxType
{
    // UI
    UI_Cancel,
    UI_Click,
    UI_Move,
    // InGame-Character

    Character_Charge,
    Character_Crush, // 일단 보류
    Character_Hit,
    Character_Landing,

    Doctor_Dead,
    Doctor_Evade,
    Doctor_Fly,
    Doctor_Skill,

    Engineer_Dead,
    Engineer_Evade,
    Engineer_Fly,
    Engineer_Skill,

    Heavy_Counter,
    Heavy_Dead,
    Heavy_Fly,
    Heavy_Skill,
    Heavy_Walk,

    Rocketeer_Dead,
    Rocketeer_Evade,
    Rocketeer_Fly,
    Rocketeer_Skill,

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
    private ObjectPool<AudioSource> loopSourcesPool = new ObjectPool<AudioSource>();
    private List<KeyValuePair<SfxType, AudioSource>> loopingSources = new List<KeyValuePair<SfxType, AudioSource>>();
    private GameObject loopSourcesObject = null;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        source = gameObject.AddComponent<AudioSource>();
        SoundManager.SfxVolume = 1; // 임시
        source.volume = SoundManager.SfxVolume;

        loopSourcesObject = new GameObject("Loop Sources");
        loopSourcesObject.transform.SetParent(this.transform);
        for(int i = 0; i < 10; ++i)
        {
            AddLoopSource();
        }
    }

    private void AddLoopSource()
    {
        AudioSource newSource = loopSourcesObject.AddComponent<AudioSource>();
        newSource.volume = SoundManager.SfxVolume;
        loopSourcesPool.Add(newSource);
    }

    public void Initialize()
    {
        if(IsInitialized == true)
        {
            return;
        }
        IsInitialized = true;

        clips.Add(SfxType.UI_Cancel,            Resources.Load<AudioClip>("Sounds/SFX_UI/SFX_UI_Cancel"));
        clips.Add(SfxType.UI_Click,             Resources.Load<AudioClip>("Sounds/SFX_UI/SFX_UI_Click"));
        clips.Add(SfxType.UI_Move,              Resources.Load<AudioClip>("Sounds/SFX_UI/SFX_UI_Move"));

        clips.Add(SfxType.Character_Charge,     Resources.Load<AudioClip>("Sounds/SFX_Character/SFX_Character_Charge"));
        clips.Add(SfxType.Character_Crush,      Resources.Load<AudioClip>("Sounds/SFX_Character/SFX_Character_Crush"));
        clips.Add(SfxType.Character_Hit,        Resources.Load<AudioClip>("Sounds/SFX_Character/SFX_Character_Hit"));
        clips.Add(SfxType.Character_Landing,    Resources.Load<AudioClip>("Sounds/SFX_Character/SFX_Character_Landing"));

        clips.Add(SfxType.Doctor_Dead,          Resources.Load<AudioClip>("Sounds/SFX_Character/Doctor/SFX_Doctor_Dead"));
        clips.Add(SfxType.Doctor_Evade,         Resources.Load<AudioClip>("Sounds/SFX_Character/Doctor/SFX_Doctor_Evade"));
        clips.Add(SfxType.Doctor_Fly,           Resources.Load<AudioClip>("Sounds/SFX_Character/Doctor/SFX_Doctor_Fly"));
        clips.Add(SfxType.Doctor_Skill,         Resources.Load<AudioClip>("Sounds/SFX_Character/Doctor/SFX_Doctor_Skill"));

        clips.Add(SfxType.Engineer_Dead,        Resources.Load<AudioClip>("Sounds/SFX_Character/Engineer/SFX_Engineer_Dead"));
        clips.Add(SfxType.Engineer_Evade,       Resources.Load<AudioClip>("Sounds/SFX_Character/Engineer/SFX_Engineer_Evade"));
        clips.Add(SfxType.Engineer_Fly,         Resources.Load<AudioClip>("Sounds/SFX_Character/Engineer/SFX_Engineer_Fly"));
        clips.Add(SfxType.Engineer_Skill,       Resources.Load<AudioClip>("Sounds/SFX_Character/Engineer/SFX_Engineer_Skill"));

        clips.Add(SfxType.Heavy_Counter,        Resources.Load<AudioClip>("Sounds/SFX_Character/Heavy/SFX_Heavy_Counter"));
        clips.Add(SfxType.Heavy_Dead,           Resources.Load<AudioClip>("Sounds/SFX_Character/Heavy/SFX_Heavy_Dead"));
        clips.Add(SfxType.Heavy_Fly,            Resources.Load<AudioClip>("Sounds/SFX_Character/Heavy/SFX_Heavy_Fly"));
        clips.Add(SfxType.Heavy_Skill,          Resources.Load<AudioClip>("Sounds/SFX_Character/Heavy/SFX_Heavy_Skill"));
        clips.Add(SfxType.Heavy_Walk,           Resources.Load<AudioClip>("Sounds/SFX_Character/Heavy/SFX_Heavy_Walk"));

        clips.Add(SfxType.Rocketeer_Dead,       Resources.Load<AudioClip>("Sounds/SFX_Character/Rocketeer/SFX_Rocketeer_Dead"));
        clips.Add(SfxType.Rocketeer_Evade,      Resources.Load<AudioClip>("Sounds/SFX_Character/Rocketeer/SFX_Rocketeer_Evade"));
        clips.Add(SfxType.Rocketeer_Fly,        Resources.Load<AudioClip>("Sounds/SFX_Character/Rocketeer/SFX_Rocketeer_Fly"));
        clips.Add(SfxType.Rocketeer_Skill,      Resources.Load<AudioClip>("Sounds/SFX_Character/Rocketeer/SFX_Rocketeer_Skill"));

        clips.Add(SfxType.Item_Create,          Resources.Load<AudioClip>("Sounds/SFX_Object/SFX_Object_Item_Create"));
        clips.Add(SfxType.Item_GetHp,           Resources.Load<AudioClip>("Sounds/SFX_Object/SFX_Object_Item_GetHp"));
        clips.Add(SfxType.Item_GetMp,           Resources.Load<AudioClip>("Sounds/SFX_Object/SFX_Object_Item_GetMp"));
        clips.Add(SfxType.Item_GetExtreme,      Resources.Load<AudioClip>("Sounds/SFX_Object/SFX_Object_Item_GetExtreme"));

        Debug.Log("SfxManager Load end.");
    }

    public void Play(SfxType type)
    {
        if(clips.ContainsKey(type) == false)
        {
            Debug.LogError("Sfx not loaded, type : " + type.ToString());
            return;
        }

        source.PlayOneShot(clips[type], source.volume);
    }

    public void PlayLoop(SfxType type)
    {
        if (clips.ContainsKey(type) == false)
        {
            Debug.LogError("Sfx not loaded, type : " + type.ToString());
            return;
        }
        if (loopSourcesPool.RemainCount == 0)
        {
            AddLoopSource();
        }

        AudioSource loopSource = loopSourcesPool.Get();
        loopSource.clip = clips[type];
        loopSource.loop = true;
        loopSource.Play();

        loopingSources.Add(new KeyValuePair<SfxType, AudioSource>(type, loopSource));
    }

    public void StopLoop(SfxType type)
    {
        int findIndex = int.MinValue;
        for(int i = 0; i < loopingSources.Count; ++i)
        {
            if(loopingSources[i].Key == type)
            {
                findIndex = i;
                break;
            }
        }

        if(findIndex != int.MinValue)
        {
            AudioSource endedSource = loopingSources[findIndex].Value;
            endedSource.Stop();
            endedSource.clip = null;

            loopingSources.RemoveAt(findIndex);

            loopSourcesPool.Add(endedSource);
        }
    }

    public void StopAll()
    {
        for(int i = 0; i < loopingSources.Count; ++i)
        {
            loopingSources[i].Value.Stop();
            loopingSources[i].Value.clip = null;
            loopSourcesPool.Add(loopingSources[i].Value);
        }
        loopingSources.Clear();
    }
}