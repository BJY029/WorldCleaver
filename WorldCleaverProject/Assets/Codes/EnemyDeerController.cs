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

		//맨 처음 위치를 따로 저장해둔다.
		InitPosition = Deer.transform.position;
	}

	private void FixedUpdate()
	{
		//사슴 아이템으로 인해 활성화가 된 경우에 이동을 진행한다.
		if (DeerActivated == false || deer == null) return;

		//앞으로 전진하다가, 나무(태그가 tree)와 부딪히면
		if (Hit == true)
		{
			//나무에게 데미지를 가하고
			GameManager.Instance.TreeController.DamageHitPlayer();
			//사슴을 비활성화한다.
			Deer.SetActive(false);
			//사슴 설정을 초기화한다.
			InitDeerPosition();
			return;
		}

		//지속적으로 앞으로 이동한다.
		Vector3 nextVec = speed * new Vector3(-1f, 0f) * Time.fixedDeltaTime;
		deer.MovePosition(Deer.transform.position + nextVec);
	}

	//trigger
	private void OnTriggerEnter2D(Collider2D collision)
	{
		//tree와 충돌시, Hit 플래그를 활성화시킨다.
		if (collision.CompareTag("tree"))
		{
			Hit = true;
			Debug.Log("collision");
		}
	}

	//사슴의 위치와 설정을 초기화한다.
	private void InitDeerPosition()
	{
		Deer.transform.position = InitPosition;
		DeerActivated = false;
		Hit = false;
	}
}
