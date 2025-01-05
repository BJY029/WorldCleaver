using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse1Controller : SingleTon<Horse1Controller>
{
	//�Է¹޴� �ӵ� ��
	public float speed;
	//�ش� �ӵ� ���� �����ϴ� ����
	private float Speed;
	Animator animator;

	//���� �ʱ�ȭ �� �Ҵ�
	private void Start()
	{
		Speed = speed;
		animator = GetComponent<Animator>();
	}

	//���� ��ġ �� �ӵ� �� �ʱ�ȭ
	public void initAll()
	{
		HorseManager.Instance.Horse1_rbPrefab.transform.position = HorseManager.Instance.Horse1_InitPos;
		Speed = speed;
		HorseManager.Instance.Horse1_Run = false;
		HorseManager.Instance.Horse1_End = false;
	}

	//���� �޸��� ������ ��, �̵��� �����ϴ� �Լ�
	private void FixedUpdate()
	{
		if (!HorseManager.Instance.Horse1_Run) return;
		animator.SetFloat("Speed", Speed);

		Vector3 nextVec = Speed * new Vector3(1f, 0f) * Time.fixedDeltaTime;
		HorseManager.Instance.Horse1_rb.MovePosition(HorseManager.Instance.Horse1_rbPrefab.transform.position + nextVec);
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
			if(HorseManager.Instance.WinnerFlag == 0)
			{
				HorseManager.Instance.WinnerFlag = 1;
			}
			Debug.Log(HorseManager.Instance.WinnerFlag);
		}
		//���� ���ο� �����ϸ�, ���� �ӵ��� 0���� �����ϰ�, �ִϸ��̼� ���� �� �¸� ���� ������ �����Ѵ�.
		else if (collision.CompareTag("StopLine"))
		{
			HorseManager.Instance.Horse1_Run = false;
			Speed = 0f;
			animator.SetFloat("Speed", Speed);
			HorseManager.Instance.Horse1_End = true;

			HorseManager.Instance.EndGame();
		}
	}
}
