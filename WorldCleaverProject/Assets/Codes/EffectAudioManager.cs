using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAudioManager : SingleTon<EffectAudioManager>
{
    public AudioSource audioSource;
    public AudioSource LoopAudioSource;
    public AudioSource AudioSourceForWalk_0;
	public AudioSource AudioSourceForWalk_1;
	public AudioClip Hit;
    public AudioClip Drink;
    public AudioClip Eat;
    public AudioClip Fight;
    public AudioClip Crunch;
    public AudioClip Eagle;
    public AudioClip FlareGun;
    public AudioClip Oil;
    public AudioClip Heal;
    public AudioClip Pop;
    public AudioClip Squeeze;
    public AudioClip Smoke;
    public AudioClip Shild;
    public AudioClip Punch;

    public AudioClip[] Wings;
    public AudioClip[] Horse;


    public float DurationTime = 2.0f;

    void Start()
    {
        
    }

	public void PlayDeerGalloping()
	{
		StartCoroutine(PlayDeerGallopings());
	}

	IEnumerator PlayDeerGallopings()
	{
		int randomIdx = Random.Range(0, Horse.Length);
		AudioSourceForWalk_0.clip = Horse[randomIdx];
		AudioSourceForWalk_0.Play();
		yield return null;
	}

	public void PlayEagleWing()
    {
        StartCoroutine(EagleWing());
    }

    IEnumerator EagleWing()
    {
        int randomIdx = Random.Range(0, Wings.Length);
        AudioSourceForWalk_0.clip = Wings[randomIdx];
        AudioSourceForWalk_0.Play();
        yield return null;
    }

    public void PlayHorseGalloping1()
    {
        StartCoroutine(PlayHorseGalloping_1());
    }

	public void PlayHorseGalloping2()
	{
		StartCoroutine(PlayHorseGalloping_2());
	}

	IEnumerator PlayHorseGalloping_1()
    {
        int randomIdx = Random.Range(0, Horse.Length);
        AudioSourceForWalk_0.clip = Horse[randomIdx];
        AudioSourceForWalk_0.Play();
        yield return null;
    }

	IEnumerator PlayHorseGalloping_2()
	{
		int randomIdx = Random.Range(0, Horse.Length);
		AudioSourceForWalk_1.clip = Horse[randomIdx];
		AudioSourceForWalk_1.Play();
		yield return null;
	}

    public void PlayPunch()
    {
        audioSource.clip = Punch;
        audioSource.Play();
    }

	public void PlayShild()
    {
        audioSource.clip = Shild;
        audioSource.Play();
    }

    public void PlaySmoke()
    {
        if (LoopAudioSource.isPlaying) return;
        StartCoroutine(PlaySmokes());
    }

    public void StopSmoke()
    {
        StartCoroutine (StopSmokes());
    }

    IEnumerator PlaySmokes()
    {
        LoopAudioSource.clip = Smoke;
        LoopAudioSource.volume = 0f;
        LoopAudioSource.Play();
        float elapsedTime = 0.0f;
        while (elapsedTime < DurationTime)
        {
            elapsedTime += Time.deltaTime;

            float newVolume = Mathf.Lerp(0f, 1f, elapsedTime / DurationTime);
            LoopAudioSource.volume = newVolume;

            yield return null;
        }
        LoopAudioSource.volume = 1.0f;
    }

    IEnumerator StopSmokes()
    {
		float elapsedTime = 0.0f;
		while (elapsedTime < DurationTime)
		{
			elapsedTime += Time.deltaTime;

			float newVolume = Mathf.Lerp(1f, 0f, elapsedTime / DurationTime);
			LoopAudioSource.volume = newVolume;

			yield return null;
		}
		LoopAudioSource.volume = 0.0f;
        LoopAudioSource.Stop();
	}

    public void PlayHit()
    {
        audioSource.clip = Hit;
        audioSource.Play();
    }

    public void PlayDrink()
    {
        audioSource.clip = Drink;
        audioSource.Play();
    }

    public void PlayEat()
    {
        audioSource.clip = Eat;
        audioSource.Play();
    }

    public void PlayFight()
    {
        audioSource.clip = Fight;
        audioSource.Play();
    }

    public void PlayCrunch()
    {
        audioSource.clip = Crunch;
        audioSource.Play();
    }

    public void PlayEagle()
    {
        audioSource.clip = Eagle;
        audioSource.Play();
    }

    public void PlayFlareGun()
    {
        audioSource.clip = FlareGun;
        audioSource.Play();
    }

    public void PlayHeal()
    {
        audioSource.clip = Heal;
        audioSource.Play();
    }

    public void PlayOil()
    {
        audioSource.clip = Oil;
        audioSource.Play();
    }

    public void playPop()
    {
        audioSource.clip = Pop;
        audioSource.Play();
    }

    public void playSqueeze()
    {
        audioSource.clip = Squeeze;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
