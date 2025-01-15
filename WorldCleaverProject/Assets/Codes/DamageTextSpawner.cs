using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
	public Vector2 InitialV;
	public Rigidbody2D rb;
	public float lifetime = 1.5f;

	private void Start()
	{
		rb.velocity = InitialV;
		Destroy(gameObject, lifetime);
	}
}
