using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse1Controller : MonoBehaviour
{
	//�Է¹޴� �ӵ� ��
	public float speed;
	//�ش� �ӵ� ���� �����ϴ� ����
	private float Speed;
	Animator animator;

	public float RunSoundInterval = 0.5f;
	private float RunSoundTimer;

	//���� �ʱ�ȭ �� �Ҵ�
	private void Start()
	{
		Speed = speed;
		animator = GetComponent<Animator>();
	}

	//���� ��ġ �� �ӵ� �� �ʱ�ȭ
	public void initAll()
	{
		GameManager.Instance.HorseManager.Horse1_rbPrefab.transform.position = GameManager.Instance.HorseManager.Horse1_InitPos;
		Speed = speed;
		GameManager.Instance.HorseManager.Horse1_Run = false;
		GameManager.Instance.HorseManager.Horse1_End = false;
	}

	//���� �޸��� ������ ��, �̵��� �����ϴ� �Լ�
	private void FixedUpdate()
	{
		if (!GameManager.Instance.HorseManager.Horse1_Run) return;
		animator.SetFloat("Speed", Speed);

		Vector3 nextVec = Speed * new Vector3(1f, 0f) * Time.fixedDeltaTime;
		GameManager.Instance.HorseManager.Horse1_rb.MovePosition(GameManager.Instance.HorseManager.Horse1_rbPrefab.transform.position + nextVec);
	}

	private void Update()
	{
		if (GameManager.Instance.HorseManager.Horse1_Run)
		{
			RunSoundTimer -= Time.deltaTime;
			if(RunSoundTimer <= 0f)
			{
				GameManager.Instance.EffectAudioManager.PlayHorseGalloping1();
				RunSoundTimer = RunSoundInterval;
			}
		}
		else
		{
			RunSoundTimer = 0f;
		}
	}

	//�� ����Ʈ�� ������ ��, ����Ǵ� ���
	private void OnTriggerEnter2D(Collider2D collision)
	{
		//point 1, 2, 3�� �ӵ� ���� ��ȭ��Ų��.
		if (collision.CompareTag("Point1"))
		{
			float RandomSpeed = Random.Range(speed - 1f, speed + 1f);
			Speed = RandomSpeed;
		}
		else if (collision.CompareTag("Point2"))
		{
			float RandomSpeed = Random.Range(speed - 1f, speed + 1f);
			Speed = RandomSpeed;
		}
		else if (collision.CompareTag("Point3"))
		{
			float RandomSpeed = Random.Range(speed - 1f, speed + 1f);
			Speed = RandomSpeed;
		}
		//Goal Line�� �����ϸ�, ���� ó������ ������ ���, �÷��� ���� 0�� �����̹Ƿ�, �÷��� ���� �ڽ���
		//��ȣ�� �����Ѵ�. ���� 0�� �ƴ� �����̸�, �̹� �ٸ� ���� ������ �����̹Ƿ� �ƹ��͵� ���� �ʴ´�.
		else if (collision.CompareTag("GoalLine"))
		{
			if(GameManager.Instance.HorseManager.WinnerFlag == 0)
			{
				GameManager.Instance.HorseManager.WinnerFlag = 1;
			}
			Debug.Log(GameManager.Instance.HorseManager.WinnerFlag);
		}
		//���� ���ο� �����ϸ�, ���� �ӵ��� 0���� �����ϰ�, �ִϸ��̼� ���� �� �¸� ���� ������ �����Ѵ�.
		else if (collision.CompareTag("StopLine"))
		{
			GameManager.Instance.HorseManager.Horse1_Run = false;
			Speed = 0f;
			animator.SetFloat("Speed", Speed);
			GameManager.Instance.HorseManager.Horse1_End = true;

			GameManager.Instance.HorseManager.EndGame();
		}
	}
}
