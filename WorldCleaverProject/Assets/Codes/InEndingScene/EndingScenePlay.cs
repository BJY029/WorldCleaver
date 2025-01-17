using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingScenePlay : MonoBehaviour
{
    public Button SeeDiaryButton;
    public GameObject MoveObj;

    public float WaitSec = 1.5f;
    public float MoveSpeed = 0.1f;
    public float EndPosition = 20f;

    public bool isMovementFinished;

    // Start is called before the first frame update
    void Start()
    {
        isMovementFinished = false;
		SeeDiaryButton.gameObject.SetActive(false);
		StartCoroutine(PlayScene());
    }

	IEnumerator PlayScene()
	{
		yield return new WaitForSeconds(WaitSec);

		Vector2 startPosition = MoveObj.transform.position; // �ʱ� ��ġ ����
		Vector2 targetPosition = new Vector2(EndPosition, startPosition.y); // ��ǥ ��ġ ����
		float distance = Vector2.Distance(startPosition, targetPosition);
		float duration = distance / MoveSpeed;
		float elapsedTime = 0f; // ��� �ð�

		while (elapsedTime < duration) // 10�� ���� �̵�
		{
			elapsedTime += Time.deltaTime; // �̵� �ӵ� ����
			float t = elapsedTime / duration;
			MoveObj.transform.position = Vector2.Lerp(startPosition, targetPosition, t);
			yield return null;
		}

		// �̵��� �Ϸ�� �� ó��
		isMovementFinished = true;
		SeeDiaryButton.gameObject.SetActive(true);
	}
}
