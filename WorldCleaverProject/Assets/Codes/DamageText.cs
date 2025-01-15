using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
	public float floatSpeed = 2.0f;  // 떠오르는 속도
	public float fadeDuration = 1.0f;  // 페이드 지속 시간

	private Text textMesh;
	private Color originalColor;
	private float elapsedTime = 0f;

	private void Start()
	{
		textMesh = GetComponent<Text>();
		if (textMesh != null)
		{
			originalColor = textMesh.color;
		}
		Debug.Log("start");
	}

	private void Update()
	{
		// 위로 이동
		transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);

		// 경과 시간 계산
		elapsedTime += Time.deltaTime;

		// 페이드아웃 효과
		if (textMesh != null)
		{
			float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
			textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
			//Debug.Log($"Elapsed Time: {elapsedTime}, Alpha: {Mathf.Lerp(1, 0, elapsedTime / fadeDuration)}");


			// 완전히 사라지면 오브젝트 제거
			if (alpha <= 0)
			{
				Destroy(gameObject);
				Debug.Log("End");
			}
		}
	}
}
