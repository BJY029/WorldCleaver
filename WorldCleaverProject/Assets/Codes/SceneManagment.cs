using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagment :SingleTon<SceneManagment>
{
    public GameObject loadingScene;
    public Slider progressBar;
    public GameObject OpeningPanel;

	//크레딧 텍스트
    public Text creditsText;
	//크레딧의 위치정보
    public RectTransform creditsRectTransform;
	//스킵 버튼
    public Button skipButton;
	//스크롤 속도
    public float creditsScrollSpeed = 200f;
	//크레딧의 최종 끝 위치
    public float creitsEndY = 1000f;
	//초기 위치 정보
	private float initialY;

	//각종 플래그
    private bool isSceneLoaded = false;
    private bool creditsFinished = false;
	//코루틴을 저장하는 변수
	private Coroutine scrollCreditsCoroutine;

	private AsyncOperation operation;

	private void Awake()
	{
		loadingScene.SetActive(false);
	}

	// Start is called before the first frame update
	void Start()
    {
        
        if(SceneManager.GetActiveScene().name == "OpeningScene")
        {
            Debug.Log("Play");
            BGMManager.Instance.PlayOpeningBGM();
        }

		loadingScene.SetActive(false);
        
    }


    public void ChangeScene(string sceneName)
    {
        if (sceneName == "SampleScene")
        {
			BGMManager.Instance.OpeningBGM.Stop();
			BGMManager.Instance.LoadingBGM.Play();
			StartCoroutine(LoadScene(sceneName));
		}
        else if(sceneName == "OpeningScene"){ 
            SceneManager.LoadScene(sceneName);
            if (Time.timeScale == 0) Time.timeScale = 1;
			OpeningPanel.SetActive(true);
			BGMManager.Instance.ChangeBGM(sceneName);
        }
    }

	//로딩씬 코루틴
	private IEnumerator LoadScene(string sceneName)
	{
		//로딩씬을 활성화 하고
		loadingScene.SetActive(true);
		// 스킵 버튼을 비활성화한다. (로딩 중에는 스킵 불가)
		skipButton.gameObject.SetActive(false);
		// 크레딧을 위로 스크롤하는 코루틴을 실행하고 해당 코루틴의 참조를 저장한다.
		scrollCreditsCoroutine =  StartCoroutine(ScrollCredits());

		//비동기로 씬을 불러온다.
		operation = SceneManager.LoadSceneAsync(sceneName);
		// 씬이 로드된 뒤 자동으로 활성화되지 않도록 설정한다.
		operation.allowSceneActivation = false;

		// 비동기 씬 로드가 완료될 때까지 반복한다.
		while (!operation.isDone)
		{
			// 로드 진행도를 0 ~ 1 범위로 계산한다.
			float progress = Mathf.Clamp01(operation.progress / 0.9f);
			// 진행도를 프로그레스 바에 반영한다
			progressBar.value = progress;

			//씬 로드가 90% 이상 완료된 경우
			if(operation.progress >= 0.9f)
			{
				//씬 로드 완료 상태를 저장한다.
				isSceneLoaded = true;
				//스킵 버튼을 활성화한다.
				skipButton.gameObject.SetActive(true);

				//크래딧 애니메이션이 끝난 경우
				if (creditsFinished)
				{
					//씬 활성화를 허용한다.
					operation.allowSceneActivation = true;
					//크레딧 종료 플래그를 초기화한다.
					creditsFinished = false;
					//실행중인 크레딧 스크롤 코루틴을 정지한다.
					StopCoroutine(scrollCreditsCoroutine);
				}
			}

			yield return null;
		}
		
		loadingScene.SetActive(false);
		OpeningPanel.SetActive(false);
		BGMManager.Instance.LoadingBGM.Stop();
		BGMManager.Instance.MainBGM.volume = BGMManager.Instance.SetVolume;
		BGMManager.Instance.MainBGM.pitch = 1.0f;
		BGMManager.Instance.MainBGM.Play();
	}

	//크레딧을 스크롤 하는 코루틴
	IEnumerator ScrollCredits()
	{
		//초기 위치 저장
		initialY = creditsRectTransform.anchoredPosition.y;

		//크레딧이 끝까지 올라갈때까지 반복
		while (!creditsFinished)
		{
			//크레딧의 위치를 속도에 맞게 초기화한다.
			creditsRectTransform.anchoredPosition = new Vector2(
				creditsRectTransform.anchoredPosition.x,
				creditsRectTransform.anchoredPosition.y + creditsScrollSpeed * Time.deltaTime
				);

			//만약 크레딧의 y축 위치가 최종 위치보다 넘어선 경우
			if (creditsRectTransform.anchoredPosition.y > creitsEndY)
			{
				// 크레딧의 위치를 초기화 하고
				creditsRectTransform.anchoredPosition = new Vector2(
					creditsRectTransform.anchoredPosition.x,
					initialY // 초기 Y값
				);

				//크레딧이 끝났다는 정보를 저장한다.
				creditsFinished = true;
			}

			yield return null;
		}


	}

	//스킵 버튼에 할당되는 함수
	public void SkipLoading()
	{
		//만약 호출된 경우, 씬이 모두 로딩이 되었고, 비동기로 로딩되는 씬이 null이 아니면
		if (isSceneLoaded && operation != null)
		{
			//크래딧을 스크롤 하는 코루틴이 저장된 경우
			if(scrollCreditsCoroutine != null)
			{
				//해당 코루틴을 정지 후 초기화
				StopCoroutine(scrollCreditsCoroutine);
			}

			// 크레딧 위치 초기화
			creditsRectTransform.anchoredPosition = new Vector2(
				creditsRectTransform.anchoredPosition.x,
				initialY // 초기 Y값
			);

			//크레딧이 끝났다고 알림
			creditsFinished = true;

			//다음 씬으로 넘어가도록 허용
			operation.allowSceneActivation = true;
		}
	}
}
