using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse1Controller : MonoBehaviour
{
	//입력받는 속도 값
	public float speed;
	//해당 속도 값을 보존하는 변수
	private float Speed;
	Animator animator;

	public float RunSoundInterval = 0.5f;
	private float RunSoundTimer;

	//변수 초기화 및 할당
	private void Start()
	{
		Speed = speed;
		animator = GetComponent<Animator>();
	}

	//말의 위치 및 속도 값 초기화
	public void initAll()
	{
		GameManager.Instance.HorseManager.Horse1_rbPrefab.transform.position = GameManager.Instance.HorseManager.Horse1_InitPos;
		Speed = speed;
		GameManager.Instance.HorseManager.Horse1_Run = false;
		GameManager.Instance.HorseManager.Horse1_End = false;
	}

	//말이 달리기 상태일 때, 이동을 수행하는 함수
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

	//각 포인트에 도달할 때, 적용되는 기능
	private void OnTriggerEnter2D(Collider2D collision)
	{
		//point 1, 2, 3은 속도 값을 변화시킨다.
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
		//Goal Line에 도달하면, 만약 처음으로 도달한 경우, 플래그 값이 0인 상태이므로, 플래그 값을 자신의
		//번호로 변경한다. 만약 0이 아닌 상태이면, 이미 다른 말이 도착한 상태이므로 아무것도 하지 않는다.
		else if (collision.CompareTag("GoalLine"))
		{
			if(GameManager.Instance.HorseManager.WinnerFlag == 0)
			{
				GameManager.Instance.HorseManager.WinnerFlag = 1;
			}
			Debug.Log(GameManager.Instance.HorseManager.WinnerFlag);
		}
		//정지 라인에 도달하면, 말의 속도를 0으로 변경하고, 애니메이션 변경 및 승리 판정 연산을 수행한다.
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
