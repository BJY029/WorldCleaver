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

	//ũ���� �ؽ�Ʈ
    public Text creditsText;
	//ũ������ ��ġ����
    public RectTransform creditsRectTransform;
	//��ŵ ��ư
    public Button skipButton;
	//��ũ�� �ӵ�
    public float creditsScrollSpeed = 200f;
	//ũ������ ���� �� ��ġ
    public float creitsEndY = 1000f;
	//�ʱ� ��ġ ����
	private float initialY;

	//���� �÷���
    private bool isSceneLoaded = false;
    private bool creditsFinished = false;
	//�ڷ�ƾ�� �����ϴ� ����
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

	//�ε��� �ڷ�ƾ
	private IEnumerator LoadScene(string sceneName)
	{
		//�ε����� Ȱ��ȭ �ϰ�
		loadingScene.SetActive(true);
		// ��ŵ ��ư�� ��Ȱ��ȭ�Ѵ�. (�ε� �߿��� ��ŵ �Ұ�)
		skipButton.gameObject.SetActive(false);
		// ũ������ ���� ��ũ���ϴ� �ڷ�ƾ�� �����ϰ� �ش� �ڷ�ƾ�� ������ �����Ѵ�.
		scrollCreditsCoroutine =  StartCoroutine(ScrollCredits());

		//�񵿱�� ���� �ҷ��´�.
		operation = SceneManager.LoadSceneAsync(sceneName);
		// ���� �ε�� �� �ڵ����� Ȱ��ȭ���� �ʵ��� �����Ѵ�.
		operation.allowSceneActivation = false;

		// �񵿱� �� �ε尡 �Ϸ�� ������ �ݺ��Ѵ�.
		while (!operation.isDone)
		{
			// �ε� ���൵�� 0 ~ 1 ������ ����Ѵ�.
			float progress = Mathf.Clamp01(operation.progress / 0.9f);
			// ���൵�� ���α׷��� �ٿ� �ݿ��Ѵ�
			progressBar.value = progress;

			//�� �ε尡 90% �̻� �Ϸ�� ���
			if(operation.progress >= 0.9f)
			{
				//�� �ε� �Ϸ� ���¸� �����Ѵ�.
				isSceneLoaded = true;
				//��ŵ ��ư�� Ȱ��ȭ�Ѵ�.
				skipButton.gameObject.SetActive(true);

				//ũ���� �ִϸ��̼��� ���� ���
				if (creditsFinished)
				{
					//�� Ȱ��ȭ�� ����Ѵ�.
					operation.allowSceneActivation = true;
					//ũ���� ���� �÷��׸� �ʱ�ȭ�Ѵ�.
					creditsFinished = false;
					//�������� ũ���� ��ũ�� �ڷ�ƾ�� �����Ѵ�.
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

	//ũ������ ��ũ�� �ϴ� �ڷ�ƾ
	IEnumerator ScrollCredits()
	{
		//�ʱ� ��ġ ����
		initialY = creditsRectTransform.anchoredPosition.y;

		//ũ������ ������ �ö󰥶����� �ݺ�
		while (!creditsFinished)
		{
			//ũ������ ��ġ�� �ӵ��� �°� �ʱ�ȭ�Ѵ�.
			creditsRectTransform.anchoredPosition = new Vector2(
				creditsRectTransform.anchoredPosition.x,
				creditsRectTransform.anchoredPosition.y + creditsScrollSpeed * Time.deltaTime
				);

			//���� ũ������ y�� ��ġ�� ���� ��ġ���� �Ѿ ���
			if (creditsRectTransform.anchoredPosition.y > creitsEndY)
			{
				// ũ������ ��ġ�� �ʱ�ȭ �ϰ�
				creditsRectTransform.anchoredPosition = new Vector2(
					creditsRectTransform.anchoredPosition.x,
					initialY // �ʱ� Y��
				);

				//ũ������ �����ٴ� ������ �����Ѵ�.
				creditsFinished = true;
			}

			yield return null;
		}


	}

	//��ŵ ��ư�� �Ҵ�Ǵ� �Լ�
	public void SkipLoading()
	{
		//���� ȣ��� ���, ���� ��� �ε��� �Ǿ���, �񵿱�� �ε��Ǵ� ���� null�� �ƴϸ�
		if (isSceneLoaded && operation != null)
		{
			//ũ������ ��ũ�� �ϴ� �ڷ�ƾ�� ����� ���
			if(scrollCreditsCoroutine != null)
			{
				//�ش� �ڷ�ƾ�� ���� �� �ʱ�ȭ
				StopCoroutine(scrollCreditsCoroutine);
			}

			// ũ���� ��ġ �ʱ�ȭ
			creditsRectTransform.anchoredPosition = new Vector2(
				creditsRectTransform.anchoredPosition.x,
				initialY // �ʱ� Y��
			);

			//ũ������ �����ٰ� �˸�
			creditsFinished = true;

			//���� ������ �Ѿ���� ���
			operation.allowSceneActivation = true;
		}
	}
}
