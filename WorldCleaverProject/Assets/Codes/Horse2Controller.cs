using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse2Controller : SingleTon<Horse2Controller>
{
	public float speed;
	private float Speed;
	Animator animator;

	public float RunSoundInterval = 0.5f;
	private float RunSoundTimer;

	private void Start()
	{
		Speed = speed;
		animator = GetComponent<Animator>();
	}

	public void initAll()
	{
		HorseManager.Instance.Horse2_rbPrefab.transform.position = HorseManager.Instance.Horse2_InitPos;
		Speed = speed;
		HorseManager.Instance.Horse2_Run = false;
		HorseManager.Instance.Horse2_End = false;
	}

	private void FixedUpdate()
	{
		if (!HorseManager.Instance.Horse2_Run) return;
		animator.SetFloat("Speed", Speed);

		Vector3 nextVec = Speed * new Vector3(1f, 0f) * Time.fixedDeltaTime;
		HorseManager.Instance.Horse2_rb.MovePosition(HorseManager.Instance.Horse2_rbPrefab.transform.position + nextVec);
	}

	private void Update()
	{
		if (HorseManager.Instance.Horse1_Run)
		{
			RunSoundTimer -= Time.deltaTime;
			if (RunSoundTimer <= 0f)
			{
				EffectAudioManager.Instance.PlayHorseGalloping2();
				RunSoundTimer = RunSoundInterval;
			}
		}
		else
		{
			RunSoundTimer = 0f;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
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
		else if (collision.CompareTag("GoalLine"))
		{
			if (HorseManager.Instance.WinnerFlag == 0)
			{
				HorseManager.Instance.WinnerFlag = 2;
			}
			Debug.Log(HorseManager.Instance.WinnerFlag);
		}
		else if (collision.CompareTag("StopLine"))
		{
			HorseManager.Instance.Horse2_Run = false;
			Speed = 0f;
			animator.SetFloat("Speed", Speed);
			HorseManager.Instance.Horse2_End = true;

			HorseManager.Instance.EndGame();
		}
	}
}
