using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeEffect : MonoBehaviour
{
    private ParticleSystem smoke;

	private void Start()
	{
		smoke = GetComponent<ParticleSystem>();
	}

	public void PlaySmoke() { smoke.Play(); }

	public void StopSmoke() {  smoke.Stop(); }
}
