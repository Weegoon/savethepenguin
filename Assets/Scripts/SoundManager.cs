using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Space(10)]
    public List<AudioClip> listMusicGamePlay;
    public List<AudioClip> listMusicMainMenu;
    public List<AudioClip> listMusicSelectLevel;
    public List<AudioClip> listMusicRoom;

    [Space(10), SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource backgroundSound;
    
    private float volSound;
    private bool soundEnable;
    private bool musicEnable;

    public AudioClip winSound;
    public AudioClip loseSound;

    public AudioClip trapFall;
    public AudioClip trapHit;
    public AudioClip earnItem;

    [Space(15), Header("Test"), SerializeField] bool haveSound;

    public bool SoundEnable
    {
        get
        {
            return soundEnable;
        }
        set
        {
            soundEnable = value;
            //SectionSettings.SoundEnable = value;
        }
    }
    public bool MusicEnable
    {
        get
        {
            return musicEnable;
        }
        set
        {
            musicEnable = value;
            //SectionSettings.MusicEnable = value;
        }
    }
    // Use this for initialization
    void Awake()
    {
       instance = this;


        //soundEnable = SectionSettings.SoundEnable;
        //musicEnable = SectionSettings.MusicEnable;

        soundEnable = true;
        musicEnable = true;
    }

    void Start()
    {
        UpdateVolume();
    }

    public void UpdateVolume()
    {
        volSound = 1;
        SoundBackground();
    }
    public void UpdateSoundBG()
    {
        if (!haveSound) return;
        if (musicEnable)
        {
            if (UIController.instance.Gameplay.gameObject.activeInHierarchy)
            {
                PlayBackgroundSound(listMusicGamePlay);
            }
            else
            {
                PlayBackgroundSound(listMusicMainMenu);
            }
        }
        else
        {
            StopBackgroundSound();
        }
    }


    #region Control Sound UI

   
    public void PlayWin(bool final)
    {
        StopBackgroundSound();
        //ControlBGM(3f);
        PlayAudioClip(winSound);
    }
    public void PlayLose()
    {
        StopBackgroundSound();
        //ControlBGM(3f);
        PlayAudioClip(loseSound);
    }

    #endregion

    #region Control BGM
    public void ControlBGM(float time)
    {
        if (coVolume != null)
        {
            StopCoroutine(coVolume);
            coVolume = null;
        }

        coVolume = StartCoroutine(VolumeDown());
        Utility.Delay(this, () =>
        {
            coVolume = null;

            coVolume = StartCoroutine(VolumeUp());
        }, time);
    }

    Coroutine coVolume;

    IEnumerator VolumeDown()
    {
        float half = 1;
        float t = 0;
        while (t < half)
        {
            t += Time.deltaTime;
            backgroundSound.volume = Mathf.Lerp(volSound, 0, t / half);

            yield return null;
        }
    }
    IEnumerator VolumeUp()
    {
        float half = 1;
        float t = 0;
        while (t < half)
        {
            t += Time.deltaTime;
            backgroundSound.volume = Mathf.Lerp(0, volSound, t / half);

            yield return null;
        }
    }
    #endregion


    public void SoundBackground()
    {
        backgroundSound.volume = volSound;
    }

    public void PlayAudioClip(AudioClip audio)
    {
        if (!haveSound) return;
        if (soundEnable)
        {
            audioSource.PlayOneShot(audio, volSound);
        }
    }

    public void StopCurrentSound()
    {
        if (!haveSound) return;
        if (soundEnable && audioSource != null)
        {
            audioSource.Stop();
        }
    }

    #region BackgroundSound

    public void PlayBackgroundSound(List<AudioClip> listBg)
    {
        if (!haveSound) return;

        if (listBg.Count == 0) return;

        if (musicEnable)
        {
            if (backgroundSound.clip != null)
            {
                if (coFadeBackgroundSound != null)
                {
                    StopCoroutine(coFadeBackgroundSound);
                    coFadeBackgroundSound = null;
                }

                coFadeBackgroundSound = StartCoroutine(IEFadeInSound(backgroundSound, 2f, delegate
                {
                    backgroundSound.clip = listBg[UnityEngine.Random.Range(0, listBg.Count)];
                    backgroundSound.Play();
                }));
            }
            else
            {
                backgroundSound.clip = listBg[UnityEngine.Random.Range(0, listBg.Count)];
                backgroundSound.Play();
            }
        }
    }

    public void StopBackgroundSound()
    {
        if (coFadeBackgroundSound != null)
        {
            StopCoroutine(coFadeBackgroundSound);
            coFadeBackgroundSound = null;
        }

        coFadeBackgroundSound = StartCoroutine(IEFadeInSound(backgroundSound, 2f, delegate
        {
            backgroundSound.Stop();
            backgroundSound.clip = null;
        }));
    }

    Coroutine coFadeBackgroundSound;

    public void PauseMusicSound()
    {
        float a = 1;
        DOTween.To(() => a, (value) =>
        {
            a = value;
            backgroundSound.volume = a;
        }, 0, 2f/*time*/);
        Utility.Delay(this, () =>
        {
            float b = 0;
            DOTween.To(() => b, (value) =>
            {
                b = value;
                backgroundSound.volume = b;
            }, 1, 1f/*time*/);
        }, 4f);
    }

    #endregion

    IEnumerator PlaySoundDelay(float timeDelay, AudioClip audio)
    {
        yield return new WaitForSeconds(timeDelay);
        PlayAudioClip(audio);
    }

    IEnumerator IEFadeInSound(AudioSource audioSource, float duration, Action callback)
    {
        float half = duration / 2f;
        float t = 0;
        while (t < half)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(volSound, 0, t / half);
            yield return null;
        }

        audioSource.volume = 0;
        if (callback != null)
        {
            callback.Invoke();
        }

        t = 0;
        while (t < half)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, volSound, t / half);
            yield return null;
        }
        audioSource.volume = 1;
    }

    [ContextMenu("Mute Sound")]
    public void MuteSoundWhenAdsOpen()
    {
        if (soundEnable)
        {
            //if (coVolume != null)
            //{
            //    StopCoroutine(coVolume);
            //    coVolume = null;
            //}

            if (audioSource.isPlaying)
                audioSource.Stop();

            volSound = 0;
            SoundBackground();
        }
    }

    public void PlaySoundWhenAdsClose()
    {
        if (soundEnable)
        {
            volSound = 1;
            SoundBackground();
        }
    }    
}
