using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse2Controller : MonoBehaviour
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
		GameManager.Instance.HorseManager.Horse2_rbPrefab.transform.position = GameManager.Instance.HorseManager.Horse2_InitPos;
		Speed = speed;
		GameManager.Instance.HorseManager.Horse2_Run = false;
		GameManager.Instance.HorseManager.Horse2_End = false;
	}

	private void FixedUpdate()
	{
		if (!GameManager.Instance.HorseManager.Horse2_Run) return;
		animator.SetFloat("Speed", Speed);

		Vector3 nextVec = Speed * new Vector3(1f, 0f) * Time.fixedDeltaTime;
		GameManager.Instance.HorseManager.Horse2_rb.MovePosition(GameManager.Instance.HorseManager.Horse2_rbPrefab.transform.position + nextVec);
	}

	private void Update()
	{
		if (GameManager.Instance.HorseManager.Horse1_Run)
		{
			RunSoundTimer -= Time.deltaTime;
			if (RunSoundTimer <= 0f)
			{
				GameManager.Instance.EffectAudioManager.PlayHorseGalloping2();
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
			if (GameManager.Instance.HorseManager.WinnerFlag == 0)
			{
				GameManager.Instance.HorseManager.WinnerFlag = 2;
			}
			Debug.Log(GameManager.Instance.HorseManager.WinnerFlag);
		}
		else if (collision.CompareTag("StopLine"))
		{
			GameManager.Instance.HorseManager.Horse2_Run = false;
			Speed = 0f;
			animator.SetFloat("Speed", Speed);
			GameManager.Instance.HorseManager.Horse2_End = true;

			GameManager.Instance.HorseManager.EndGame();
		}
	}
}
