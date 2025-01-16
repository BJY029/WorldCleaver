using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningSceneUIManager : SingleTon<OpeningSceneUIManager>
{
	public GameObject SettingPanel;
	public Slider BGMSlider;
	public Slider EffectSlider;

	public GameObject RulesPanel;


	// Start is called before the first frame update
	void Start()
    {
        SettingPanel.transform.localScale = Vector3.zero;
		RulesPanel.transform.localScale = Vector3.zero;
		BGMSlider.value = BGMManager.Instance.SetVolume;
		EffectSlider.value = BGMManager.Instance.SetEVolume;
	}

	public void Set()
	{
		Time.timeScale = 0;
		BGMManager.Instance.OpeningBGM.Pause();
		SettingPanel.transform.localScale = Vector3.one;
	}

	public void Close()
	{
		float volum = BGMSlider.value;
		float Evolum = EffectSlider.value;
		BGMManager.Instance.SetVolume = volum;
		BGMManager.Instance.SetEVolume = Evolum;

		SettingPanel.transform.localScale = Vector3.zero;
		Time.timeScale = 1;
		BGMManager.Instance.MainBGM.volume = volum;
		BGMManager.Instance.HeartBeat.volume = volum;
		BGMManager.Instance.FightBGM.volume = volum;
		BGMManager.Instance.OpeningBGM.volume = volum;
		BGMManager.Instance.LoadingBGM.volume = volum;
		BGMManager.Instance.OpeningBGM.Play();
	}

	public void RuleOpen()
	{
		RulesPanel.transform.localScale = Vector3.one;
	}

	public void RuleClose()
	{
		RulesPanel.transform.localScale = Vector3.zero;
	}
}
