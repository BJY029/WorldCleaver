using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse1Controller : SingleTon<Horse1Controller>
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Point1"))
		{

		}
		else if (collision.CompareTag("Point2"))
		{

		}
		else if (collision.CompareTag("Point3"))
		{

		}
		else if (collision.CompareTag("GoalLine"))
		{

		}
		else if (collision.CompareTag("StopLine"))
		{
			HorseManager.Instance.Horse1_Speed = 0f;
		}
	}
}
