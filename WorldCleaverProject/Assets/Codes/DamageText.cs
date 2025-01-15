using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
	public float floatSpeed = 2.0f;  // �������� �ӵ�
	public float fadeDuration = 1.0f;  // ���̵� ���� �ð�

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
		// ���� �̵�
		transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);

		// ��� �ð� ���
		elapsedTime += Time.deltaTime;

		// ���̵�ƿ� ȿ��
		if (textMesh != null)
		{
			float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
			textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
			//Debug.Log($"Elapsed Time: {elapsedTime}, Alpha: {Mathf.Lerp(1, 0, elapsedTime / fadeDuration)}");


			// ������ ������� ������Ʈ ����
			if (alpha <= 0)
			{
				Destroy(gameObject);
				Debug.Log("End");
			}
		}
	}
}
