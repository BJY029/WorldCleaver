using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeerController : SingleTon<EnemyDeerController>
{
	private Rigidbody2D deer;
	private BoxCollider2D boxCollider;
	public GameObject Deer;

	public Boolean DeerActivated;
	public Boolean Hit;
	public float speed;

	private Vector2 InitPosition;

	// Start is called before the first frame update
	void Start()
	{
		deer = GetComponent<Rigidbody2D>();
		boxCollider = GetComponent<BoxCollider2D>();

		Hit = false;

		//�� ó�� ��ġ�� ���� �����صд�.
		InitPosition = Deer.transform.position;
	}

	private void FixedUpdate()
	{
		//�罿 ���������� ���� Ȱ��ȭ�� �� ��쿡 �̵��� �����Ѵ�.
		if (DeerActivated == false || deer == null) return;

		//������ �����ϴٰ�, ����(�±װ� tree)�� �ε�����
		if (Hit == true)
		{
			//�������� �������� ���ϰ�
			GameManager.Instance.TreeController.DamageHitPlayer();
			//�罿�� ��Ȱ��ȭ�Ѵ�.
			Deer.SetActive(false);
			//�罿 ������ �ʱ�ȭ�Ѵ�.
			InitDeerPosition();
			return;
		}

		//���������� ������ �̵��Ѵ�.
		Vector3 nextVec = speed * new Vector3(-1f, 0f) * Time.fixedDeltaTime;
		deer.MovePosition(Deer.transform.position + nextVec);
	}

	//trigger
	private void OnTriggerEnter2D(Collider2D collision)
	{
		//tree�� �浹��, Hit �÷��׸� Ȱ��ȭ��Ų��.
		if (collision.CompareTag("tree"))
		{
			Hit = true;
			Debug.Log("collision");
		}
	}

	//�罿�� ��ġ�� ������ �ʱ�ȭ�Ѵ�.
	private void InitDeerPosition()
	{
		Deer.transform.position = InitPosition;
		DeerActivated = false;
		Hit = false;
	}
}
