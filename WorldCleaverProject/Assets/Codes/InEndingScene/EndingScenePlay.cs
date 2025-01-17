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

		Vector2 startPosition = MoveObj.transform.position; // 초기 위치 저장
		Vector2 targetPosition = new Vector2(EndPosition, startPosition.y); // 목표 위치 설정
		float distance = Vector2.Distance(startPosition, targetPosition);
		float duration = distance / MoveSpeed;
		float elapsedTime = 0f; // 경과 시간

		while (elapsedTime < duration) // 10초 동안 이동
		{
			elapsedTime += Time.deltaTime; // 이동 속도 조절
			float t = elapsedTime / duration;
			MoveObj.transform.position = Vector2.Lerp(startPosition, targetPosition, t);
			yield return null;
		}

		// 이동이 완료된 후 처리
		isMovementFinished = true;
		SeeDiaryButton.gameObject.SetActive(true);
	}
}
