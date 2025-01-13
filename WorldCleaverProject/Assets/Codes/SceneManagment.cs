using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagment :SingleTon<SceneManagment>
{

    public GameObject loadingScene;
    public Slider progressBar;

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

	// Update is called once per frame
	void Update()
    {
        
    }

    public void ChangeScene(string sceneName)
    {
        if (sceneName == "SampleScene")
        {
			StartCoroutine(LoadScene(sceneName));
        }
        else { 
            SceneManager.LoadScene(sceneName);
			loadingScene.SetActive(false);
			BGMManager.Instance.ChangeBGM(sceneName);
        }
    }

    private IEnumerator LoadScene(string sceneName)
    {

        loadingScene.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;
            yield return null;
        }

		//loadingScene.SetActive(false);
		BGMManager.Instance.OpeningBGM.Stop();
		BGMManager.Instance.MainBGM.volume = BGMManager.Instance.SetVolume;
		BGMManager.Instance.MainBGM.Play();
	}

}
