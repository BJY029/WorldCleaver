using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyEagleController : SingleTon<EnemyEagleController>
{
    public Transform target1;
    public Transform target2;
    public Transform target3;
    
    private Transform Target;


    private Vector3 InitPosition;

    public float speed = 7f;

    public bool EagleActive;

	private void Start()
	{
        InitPosition = transform.position;
        EagleActive = false;
		Target = target1;
	}

	// Update is called once per frame
	void Update()
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
        if (collision.CompareTag("Player"))
        {
            Target = target2;
        }
        else if (collision.CompareTag("Enemy"))
        {
            Target = target3;
        }
        else if (collision.CompareTag("Target"))
        {
            InitEnemyEagle();
        }
	}
}
