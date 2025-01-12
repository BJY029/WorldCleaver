using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEagleController : SingleTon<PlayerEagleController>
{
	public Transform target1;
	public Transform target2;
	public Transform target3;

	private Transform Target;

	public float EagIeInterval = 1.0f;
	private float EagleTimer;

	private Vector3 InitPosition;

	public float speed = 7f;

	public bool EagleActive;

	private void Start()
	{
		InitPosition = transform.position;
		EagleActive = false;
		Target = target1;
	}

	private void Update()
	{
		if (EagleActive)
		{
			EagleTimer -= Time.deltaTime;
			if(EagleTimer <= 0f)
			{
				EffectAudioManager.Instance.PlayEagleWing();
				EagleTimer = EagIeInterval;
			}
		}
		else
		{
			EagleTimer = 0f;
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (EagleActive == false) return;

		Vector3 direction = (Target.position - transform.position).normalized;
		transform.Translate(direction * speed * Time.deltaTime, Space.World);
	}

	public void InitEnemyEagle()
	{
		transform.position = InitPosition;
		EagleActive = false;
		Target = target1;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
		{
			Target = target2;
		}
		else if (collision.CompareTag("Player"))
		{
			Target = target3;
		}
		else if (collision.CompareTag("Target"))
		{
			InitEnemyEagle();
		}
	}
}
