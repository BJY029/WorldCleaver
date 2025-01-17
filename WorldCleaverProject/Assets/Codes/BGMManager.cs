using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : SingleTon<BGMManager>
{
    public AudioSource MainBGM;
    public AudioSource HeartBeat;
    public AudioSource FightBGM;
    public AudioSource OpeningBGM;
    public AudioSource LoadingBGM;
    public AudioSource EndingBGM;

    public float SetVolume = 1.0f;
    public float SetEVolume = 1.0f;

    public float durationTime = 3.0f;
    public bool flag;
    public bool flag2;

    public bool StallFlag;

    // Start is called before the first frame update
    void Awake()
    {
		SetVolume = 1.0f;
        StallFlag = false;
    }

    public void PlayOpeningBGM()
    {
        Debug.Log(SetVolume);
        OpeningBGM.volume = SetVolume;
        SetEVolume = 1.0f;

		OpeningBGM.Play();
    }

    public void ChangeBGM(string sceneName)
    {
        if(sceneName == "SampleScene")
        {
            if (OpeningBGM.isPlaying)
                StartCoroutine(FadeOutBGM(OpeningBGM));

			MainBGM.volume = SetVolume;
			HeartBeat.volume = SetVolume;
			FightBGM.volume = SetVolume;
			OpeningBGM.volume = SetVolume;
            LoadingBGM.volume = SetVolume;
            EndingBGM.volume = SetVolume;
		}
        else if(sceneName == "OpeningScene")
        {
            MainBGM.Stop();
            HeartBeat.Stop();
            EndingBGM.Stop();
            //MainBGM.pitch = 1.0f;
			MainBGM.volume = SetVolume;
			HeartBeat.volume = SetVolume;
			FightBGM.volume = SetVolume;
			OpeningBGM.volume = SetVolume;
            LoadingBGM.volume = SetVolume;
            EndingBGM.volume = SetVolume;
			OpeningBGM.Play();

			OpeningSceneUIManager.Instance. BGMSlider.value = BGMManager.Instance.SetVolume;
			OpeningSceneUIManager.Instance.EffectSlider.value = BGMManager.Instance.SetEVolume;
		}
        else if(sceneName == "EndingScene")
        {
            MainBGM.Stop();
            HeartBeat.Stop();
			//MainBGM.pitch = 1.0f;

			EndingBGM.volume = SetVolume;
			EndingBGM.Play();
        }
    }

    //�÷��̾��� ���¸� Ȯ���ؼ�, �����ٴ� ȿ������ ��������� ���θ� �����ϴ� �Լ�
    //GameManager�� DamageHitTree ������
    //ItmeManager�� ������
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


    //�÷��̾� ����� ȣ��Ǵ� �Լ�
    public void PlayerDead()
    {
        HeartBeat.Stop();
        StartCoroutine(SlowDownBGM());
		if (GameManager.Instance.EffectAudioManager.LoopAudioSource.isPlaying)
			StartCoroutine(FadeOutBGM(GameManager.Instance.EffectAudioManager.LoopAudioSource));
	}

    IEnumerator Stall()
    {
        StallFlag = true; 
        yield return new WaitForSeconds(durationTime);
        StallFlag = false;
    }

    //����ǰ� �ִ� ����� �ӵ��� 0���� õõ�� ���δ�.
    IEnumerator SlowDownBGM()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < durationTime)
        {
            elapsedTime += Time.deltaTime;

            float newPitch = Mathf.Lerp(SetVolume, 0f, elapsedTime/ durationTime);
            MainBGM.pitch = newPitch;

            yield return null;
        }

        MainBGM.pitch = 0f;
        MainBGM.Stop();
		MainBGM.pitch = 1f;
	}

    //������û �������� ������� �� ȣ��Ǵ� �Լ�
    public void MoveToFight()
    {
        if (GameManager.Instance.Turn == 44) return;
        //���� ����� Fadeout�ϰ�
        StartCoroutine(FadeOutBGM(MainBGM));
        //���� ���� �αٵα� �� bgm��  ����ǰ� ������, �÷��׸� üũ�ϰ�, ���� Fadeout
        if (HeartBeat.isPlaying)
        {
            flag = true;
            StartCoroutine( FadeOutBGM(HeartBeat));
        }
        else flag = false;

        //ȿ���� ����� �ҽ�����, Loop�� ����� �ҽ��� ����Ǿ������� �̰͵� ���� Fadeout
        if (GameManager.Instance.EffectAudioManager.LoopAudioSource.isPlaying)
        {
            flag2 = true;
            StartCoroutine(FadeOutBGM(GameManager.Instance.EffectAudioManager.LoopAudioSource));
        }
        else flag2 = false;

        //������û ���� ����� ����Ѵ�.
		StartCoroutine(FadeInBGM(FightBGM));
    }

    //������ ������ �ٽ� ���ƿ��� �Լ�
    public void BackToMain()
    {
        //�� ��ݵ��� ���ǿ� �°� FadeIn�Ѵ�
		StartCoroutine(FadeOutBGM(FightBGM));
        StartCoroutine(FadeInBGM(MainBGM));
        if (flag) StartCoroutine(FadeInBGM(HeartBeat));
        if (flag2) StartCoroutine(FadeInBGM(GameManager.Instance.EffectAudioManager.LoopAudioSource));
    }

    //���޹��� �ҽ��� FadeOut�ϴ� �ڷ�ƾ
    public IEnumerator FadeOutBGM(AudioSource source)
    {
		float elapsedTime = 0.0f;
		while (elapsedTime < durationTime)
		{
			elapsedTime += Time.deltaTime;

			float newVolume = Mathf.Lerp(SetVolume, 0f, elapsedTime / durationTime);
			source.volume = newVolume;

			yield return null;
		}

		source.volume = 0f;
        //���� ���޹��� �ҽ��� ������ ���� ����̸�, �׳� ��� �����ġ�� �ʱ�ȭ��Ų��.
        if(source == FightBGM) source.Stop();
        else source.Pause();
	}

    //���޹��� �ҽ��� Fadein�ϴ� �ڷ�ƾ
    public IEnumerator FadeInBGM(AudioSource source)
    {
        source.Play();

		float elapsedTime = 0.0f;
		while (elapsedTime < durationTime)
		{
			elapsedTime += Time.deltaTime;

			float newVolume = Mathf.Lerp(0f, SetVolume, elapsedTime / durationTime);
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
