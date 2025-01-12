using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : SingleTon<BGMManager>
{
    public AudioSource MainBGM;
    public AudioSource HeartBeat;
    public AudioSource FightBGM;

    public float durationTime = 3.0f;
    public bool flag;
    public bool flag2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //플레이어의 상태를 확인해서, 가슴뛰는 효과음을 재생할지의 여부를 결정하는 함수
    public void CheckState()
    {
        int state = GameManager.Instance.PlayerController.ReturnCurrentDanger();

        if (state == 0)
        {
            if (HeartBeat.isPlaying) HeartBeat.Stop();
        }
        else if (state == 1)
        {
            if (!HeartBeat.isPlaying)
            {
                HeartBeat.Play();
            }
            HeartBeat.pitch = 0.8f;
        }
        else if (state == 2)
        {
            HeartBeat.pitch = 1.0f;
        }
        else if (state == 3)
        {
            HeartBeat.pitch = 1.2f;
        }
    }


    //플레이어 사망시 호출되는 함수
    public void PlayerDead()
    {
        HeartBeat.Stop();
        StartCoroutine(SlowDownBGM());
		if (EffectAudioManager.Instance.LoopAudioSource.isPlaying)
			StartCoroutine(FadeOutBGM(EffectAudioManager.Instance.LoopAudioSource));
	}

    //재생되고 있는 브금의 속도를 0으로 천천히 줄인다.
    IEnumerator SlowDownBGM()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < durationTime)
        {
            elapsedTime += Time.deltaTime;

            float newPitch = Mathf.Lerp(1f, 0f, elapsedTime/ durationTime);
            MainBGM.pitch = newPitch;

            yield return null;
        }

        MainBGM.pitch = 0f;
        MainBGM.Stop();
    }

    //결투신청 아이템을 사용했을 때 호출되는 함수
    public void MoveToFight()
    {
        //메인 브금을 Fadeout하고
        StartCoroutine(FadeOutBGM(MainBGM));
        //만약 현재 두근두근 ㅋ bgm이  재생되고 있으면, 플레그를 체크하고, 같이 Fadeout
        if (HeartBeat.isPlaying)
        {
            flag = true;
            StartCoroutine( FadeOutBGM(HeartBeat));
        }
        else flag = false;

        //효과음 오디오 소스에서, Loop용 오디오 소스가 재생되어있으면 이것도 같이 Fadeout
        if (EffectAudioManager.Instance.LoopAudioSource.isPlaying)
        {
            flag2 = true;
            StartCoroutine(FadeOutBGM(EffectAudioManager.Instance.LoopAudioSource));
        }
        else flag2 = false;

        //결투신청 전용 브금을 재생한다.
		StartCoroutine(FadeInBGM(FightBGM));
    }

    //결투가 끝나고 다시 돌아오는 함수
    public void BackToMain()
    {
        //각 브금들을 조건에 맞게 FadeIn한다
		StartCoroutine(FadeOutBGM(FightBGM));
        StartCoroutine(FadeInBGM(MainBGM));
        if (flag) StartCoroutine(FadeInBGM(HeartBeat));
        if (flag2) StartCoroutine(FadeInBGM(EffectAudioManager.Instance.LoopAudioSource));
    }

    //전달받은 소스를 FadeOut하는 코루틴
    public IEnumerator FadeOutBGM(AudioSource source)
    {
		float elapsedTime = 0.0f;
		while (elapsedTime < durationTime)
		{
			elapsedTime += Time.deltaTime;

			float newVolume = Mathf.Lerp(1f, 0f, elapsedTime / durationTime);
			source.volume = newVolume;

			yield return null;
		}

		source.volume = 0f;
        //만약 전달받은 소스가 전투씬 전용 브금이면, 그냥 브금 재생위치를 초기화시킨다.
        if(source == FightBGM) source.Stop();
        else source.Pause();
	}

    //전달받은 소스를 Fadein하는 코루틴
    public IEnumerator FadeInBGM(AudioSource source)
    {
        source.Play();

		float elapsedTime = 0.0f;
		while (elapsedTime < durationTime)
		{
			elapsedTime += Time.deltaTime;

			float newVolume = Mathf.Lerp(0f, 1f, elapsedTime / durationTime);
			source.volume = newVolume;

			yield return null;
		}

		source.volume = 1f;
	}

    // Update is called once per frame
    void Update()
    {
    }
}
