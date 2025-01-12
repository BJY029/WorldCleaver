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

    //�÷��̾��� ���¸� Ȯ���ؼ�, �����ٴ� ȿ������ ��������� ���θ� �����ϴ� �Լ�
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
		if (EffectAudioManager.Instance.LoopAudioSource.isPlaying)
			StartCoroutine(FadeOutBGM(EffectAudioManager.Instance.LoopAudioSource));
	}

    //����ǰ� �ִ� ����� �ӵ��� 0���� õõ�� ���δ�.
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

    //������û �������� ������� �� ȣ��Ǵ� �Լ�
    public void MoveToFight()
    {
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
        if (EffectAudioManager.Instance.LoopAudioSource.isPlaying)
        {
            flag2 = true;
            StartCoroutine(FadeOutBGM(EffectAudioManager.Instance.LoopAudioSource));
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
        if (flag2) StartCoroutine(FadeInBGM(EffectAudioManager.Instance.LoopAudioSource));
    }

    //���޹��� �ҽ��� FadeOut�ϴ� �ڷ�ƾ
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
