using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


//전반적인 UI를 관리하는 스크립트
//싱글톤이기에 GameManager에 바로 접근 가능하다.
public class UIManager : MonoBehaviour
{
	public Button PauseButton;
	public GameObject PausePanel;
	public Slider BGMSlider;
	public Slider EffectSlider;

	public Slider treeSlider; //나무 체력을 나타내주는 슬라이더 UI
	public Slider playerSlider; //플레이어 기력을 나타내주는 슬라이더 UI
	public Slider EnemySlider; //적 기력을 나타내주는 슬라이더 UI
	public Button HitButton; //HIT 버튼 UI

	public Canvas VillageCanvas;
	public Canvas OppositeVillageCanvas;
	public Button ToVillageButton;
	public Button ToOppositeVillageButton;
	public Slider VillageSlider;
	public Slider OppositeVillageSlider;
	public Slider MyVillageSliderInMain;
	public Slider OppositeVillageSliderInMain;
	public SpriteRenderer VillageSky;
	public SpriteRenderer VillageCloud;
	public SpriteRenderer OppositeVillageSky;
	public SpriteRenderer OppositeVillageCloud;

	public GameObject ItemListPanel;

	public GameObject YouWinPanel;
	public GameObject YouLosePanel;
	public GameObject HintPanel;
	public Text HintText;
	public Button BackToOpening;
	private bool hasExecuted;

	private float InitHealth; //나무 체력의 초기 값을 저장하기 위한 변수 
	private float InitPlayerPower; //플레이어 체력의 초깃값을 저장하기 위한 변수
	private float InitEnemyPower; //플레이어 체력의 초깃값을 저장하기 위한 변수
	private float InitVillageHealth;//마을 체력의 초깃값을 저장하기 위한 변수
	private float InitOppositeVillageHealth;
	private CanvasGroup SliderGroup;

	//마지막 연산을 위한 플래그 설정
	private int flag = 0;

	public bool MainBGMFlag;
	public bool FightBGMFlag;
	public bool EffectAudioFlag;
	public bool ItemSelecting;

	private string[] TreeHint;
	private string[] ManaHint;
	private string[] VillageHint;

	private void Awake()
	{
		HitButton.interactable = false;
	}

	private void Start()
	{
		YouLosePanel.SetActive(false);
		YouWinPanel.SetActive(false);
		HintPanel.SetActive(false);
		PausePanel.transform.localScale = Vector3.zero;
		BackToOpening.transform.localScale = Vector3.zero;
		hasExecuted = false;

		////멈춤 버튼 활성화
		PauseButton.interactable = true;

		BGMSlider.value = BGMManager.Instance.SetVolume;
		EffectSlider.value = BGMManager.Instance.SetEVolume;

		//초기화
		InitHealth = GameManager.Instance.TreeController.treeHealth;
		InitPlayerPower = GameManager.Instance.PlayerController.Mana;
		InitEnemyPower = GameManager.Instance.EnemeyController.Mana;
		InitVillageHealth = GameManager.Instance.VillageManager.VilageHelath;
		InitOppositeVillageHealth = GameManager.Instance.OppositeVillageManager.OppositeVillageHealth;

		ItemSelecting = true;
		VillageCanvas.enabled = false;
		OppositeVillageCanvas.enabled = false;
		SliderGroup = treeSlider.GetComponent<CanvasGroup>();

		playerSlider.value = InitPlayerPower;
		EnemySlider.value = InitEnemyPower;
		treeSlider.value = InitHealth;
		VillageSlider.value = InitVillageHealth;
		OppositeVillageSlider.value = InitOppositeVillageHealth;
		MyVillageSliderInMain.value = InitVillageHealth;
		OppositeVillageSliderInMain.value = InitOppositeVillageHealth;


		TreeHint = new string[] {
			"아이템을 적절히 활용해서 상대방의 막타를 유도해 보세요",
			"기름 아이템을 적절한 타이밍에 사용해 보세요",
			"미리미리 나무의 체력을 회복시켜보세요"
		};
		ManaHint = new string[] {
			"기력 회복 아이템의 기력 사용량은 0이랍니다",
			"특정 아이템의 기력 사용량은 매우 높아 사용에 유의해야 해요",
			"당신의 운을 결투 신청 아이템을 통해 시험해 보세요"
		};
		VillageHint = new string[] {
			"나무의 체력이 많을 때 많이 때려두세요",
			"특정 아이템을 적절히 활용하여 상대방이 나무를 충분히 때리지 못하게 막아보세요",
			"마을 체력 회복 아이템은 단 하나라는 사실을 잊지 마세요"
		};
	}

	private void Update()
	{
		//if(BGMManager.Instance.StallFlag == true)
		//{
		//	PauseButton.interactable = false;
		//}
		//else
		//{
		//	PauseButton.interactable = true;
		//}

		//중요! 만약 게임이 종료된 경우
		if (GameManager.Instance.Turn == 44)
		{
			//게임이 누구에 의해 종료되었는지 확인한다.
			GameManager.Instance.WhoLoseGame();
			GameManager.Instance.AnimationManager.Dead();
			//Hit 버튼을 비활성화 한다.
			HitButton.interactable = false;
			//마을 전용 Canvas 비활성화
			ToVillageButton.interactable = false;
			ToOppositeVillageButton.interactable = false;
			//멈춤 버튼 비활성화
			PauseButton.interactable = false;

			if (!hasExecuted)
			{
				//게임 승리자를 띄우는 UI를 활성화한다.
				if (GameManager.Instance.WhoLose == 0)
				{
					YouLosePanel.SetActive(true);


					int RandomIdx = Random.Range(0, 3);
					int reason = GameManager.Instance.ReasonFlag;
					if (reason == 0)
					{
						HintText.text = "Hint\n" + TreeHint[RandomIdx];
					}
					else if (reason == 1)
					{
						HintText.text = "Hint\n" + ManaHint[RandomIdx];
					}
					else if (reason == 2)
					{
						HintText.text = "Hint\n" + VillageHint[RandomIdx];
					}
					HintPanel.SetActive(true);


				}
				else
				{
					YouWinPanel.SetActive(true);
				}

				StartCoroutine(WaitAndActiveButton());
				hasExecuted = true;
			}

			//그리고 현재 플래그가 1이면 이미 기력 바가 초기화된 상태이므로 그대로 return한다.
			if (flag == 1) return;
			//현재 플래그가 0이면, 기력 바의 연산이 아직 이루어지기 전이다. 
			//따라서, 기력 바의 연산을 끝낸 후, 예외 처리를 하기 위해 flag 도입
			//이제 기력 바의 연산이 끝난 후에, flag 값이 1로 업데이트 되면서 return이 된다.
			else flag = 1;
		}

		//만약 현재 연막탄 아이템이 실행중이면
		if(GameManager.Instance.ItemManager.smokeFlag != 0)
		{
			//게임이 끝난 경우
			if (GameManager.Instance.Turn == 44)
			{
				//연막탄 효과를 해제한다.
				SliderGroup.alpha = 1f;
			}
			else
			{
				//아니면 유지한다.
				SliderGroup.alpha = 0f;
			}
		}
		else
		{
			SliderGroup.alpha = 1f;
		}

		//현재 나무 체력 받아오기
		float curHealth = GameManager.Instance.TreeController.treeHealth;
		//나무 체력 슬라이더 업데이트
		treeSlider.value = curHealth / InitHealth;
		treeSlider.GetComponentInChildren<Text>().text = curHealth.ToString() + "/" + InitHealth.ToString();

		//그냥 Turn 값을 불러오면, 이미 Turn이 교체된 상태에서 변경이 진행되므로 버그 발생
		//따라서, 기존 Turn을 저장한 myTurn 값을 불러와서 해당 값을 통해 현재 플레이어를 판단한다.
		int Turn = GameManager.Instance.myTurn;

		//그냥 상시에 업데이트 할 수 있도록 설정
		//현재 플레이어 기력 받아오기
		float curPower_p = GameManager.Instance.PlayerController.Mana;
		//플레이어 기력 슬리이더 업데이트
		playerSlider.value = curPower_p / InitPlayerPower;
		playerSlider.GetComponentInChildren<Text>().text = curPower_p.ToString() + "/" + InitPlayerPower.ToString();

		float curPower_e = GameManager.Instance.EnemeyController.Mana;
		EnemySlider.value = curPower_e / InitEnemyPower;
		EnemySlider.GetComponentInChildren<Text>().text = curPower_e.ToString() + "/" + InitEnemyPower.ToString();

		//마을 체력 슬라이더 설정
		float VillageHealth = GameManager.Instance.VillageManager.VilageHelath;
		VillageSlider.value = VillageHealth / InitVillageHealth;
		VillageSlider.GetComponentInChildren<Text>().text = VillageHealth.ToString() + "/" + InitVillageHealth.ToString();
		MyVillageSliderInMain.value = VillageSlider.value;



		//색상이 적용되지 않는다. 
		//아마도 해당 SpriteRender에 스프라이트가 이미 적용되어 있어서 그런듯 하다.
		//따라서 이미지 자체를 색상이 변경된 것으로 구해야 할 듯 하다.
		float OppositeVillageHealth = GameManager.Instance.OppositeVillageManager.OppositeVillageHealth;
		OppositeVillageSlider.value = OppositeVillageHealth / InitOppositeVillageHealth;
		OppositeVillageSlider.GetComponentInChildren<Text>().text = OppositeVillageHealth.ToString() + "/" + InitOppositeVillageHealth.ToString();
		OppositeVillageSliderInMain.value = OppositeVillageSlider.value;




		////플레이어 턴이면
		//if (Turn == 0)
		//{
		//	//현재 플레이어 기력 받아오기
		//	float curPower = GameManager.Instance.PlayerController.Mana;
		//	//플레이어 기력 슬리이더 업데이트
		//	playerSlider.value = curPower / InitPlayerPower;
		//}
		//else if (Turn == 1) //적 턴이면
		//{
		//	float curPower = GameManager.Instance.EnemeyController.Mana;
		//	EnemySlider.value = curPower / InitEnemyPower;
		//}

		//만약 적 턴이면 버튼을 비활성화 시킨다.
		if (GameManager.Instance.Turn == 1) ToVillageButton.interactable = false;
		else if(GameManager.Instance.Turn == 0) //만약 내 턴이면
		{
			//만약 현재 아이템 Full 경고문 혹은 Empty 경고문이 뜨고 있는 상태면
			if(GameManager.Instance.DisplayWarningMessage.WarningFlag == 1 || GameManager.Instance.DisplayEmptyMessage.WarningFlag == 1)
			{
				//비활성화
				ToVillageButton.interactable = false;
				ToOppositeVillageButton.interactable = false;
			}
			//else
			//{
			//	ToVillageButton.interactable = true;
			//	ToOppositeVillageButton.interactable = true;
			//}
			//만약 현재 아이템 선택 창이 떠 있는 경우
			else if (ItemListPanel.transform.localScale == Vector3.one)
			{
				//버튼 비활성화
				ToVillageButton.interactable = false;
				ToOppositeVillageButton.interactable = false;
			}
			else
			{
				if (!ItemSelecting)
				{
					//아니면 다시 활성화
					ToVillageButton.interactable = true;
					ToOppositeVillageButton.interactable = true;
				}
			}
		}

		//만약 플래그가 1이면 밑의 연산은 재끼고 바로 리턴한다.
		if (flag == 1) return;
		//만약 Hit 명령을 수행중이면 버튼을 비활성화 시킨다.
		if (GameManager.Instance.AnimationManager.isHitingTree == 1)
		{
			HitButton.interactable = false;
			//마을 버튼 또한 비활성화시킨다.
			ToVillageButton.interactable = false;
			ToOppositeVillageButton.interactable = false;
		}
		//적 턴인 경우 Hit 버튼을 비활성화 시킨다.
		else if(GameManager.Instance.Turn == 1)
		{
			HitButton.interactable = false;
		}
		else if(GameManager.Instance.ItemManager.FuncFlag == true)
		{
			HitButton.interactable = false;
		}
		else
		{
			//악간의 딜레이를 주기위해 코루틴에서 플래그를 설정해서 해당 플래그를 false로 처리
			//근데 게임을 시작할 때, itemSelecting이 false로 되어있으면, 아이템 선택창이 뜨지도 않았는데 Hit버튼이 상호작용 가능해서
			//버그 발생 가능, 따라서 일단 itemSelecting을 처음에 true로 설정
			if(!ItemSelecting)
				HitButton.interactable= true;
		}
	}

	public void IsPauesButtonClicked()
	{
		Time.timeScale = 0;
		BGMManager.Instance.MainBGM.Pause();
		if (GameManager.Instance.EffectAudioManager.LoopAudioSource.isPlaying)
		{
			GameManager.Instance.EffectAudioManager.LoopAudioSource.Pause();
			EffectAudioFlag = true;
		}
		else EffectAudioFlag = false;
		PausePanel.transform.localScale = Vector3.one;
	}

	public void BackToGame()
	{
		float volum = BGMSlider.value;
		float Evolum = EffectSlider.value;
		BGMManager.Instance.SetVolume = volum;
		BGMManager.Instance.SetEVolume = Evolum;

		PausePanel.transform.localScale = Vector3.zero;
		Time.timeScale = 1;
		BGMManager.Instance.MainBGM.volume = volum;
		BGMManager.Instance.HeartBeat.volume = volum;
		BGMManager.Instance.FightBGM.volume = volum;
		BGMManager.Instance.OpeningBGM.volume = volum;
		BGMManager.Instance.LoadingBGM.volume = volum;
		BGMManager.Instance.MainBGM.Play();

		GameManager.Instance.EffectAudioManager.audioSource.volume = Evolum;
		GameManager.Instance.EffectAudioManager.LoopAudioSource.volume = Evolum;
		GameManager.Instance.EffectAudioManager.AudioSourceForWalk_0.volume = Evolum;
		GameManager.Instance.EffectAudioManager.AudioSourceForWalk_1.volume = Evolum;
		if (EffectAudioFlag) GameManager.Instance.EffectAudioManager.LoopAudioSource.Play();
	}

	IEnumerator WaitAndActiveButton()
	{
		yield return new WaitForSeconds(6f);
		BackToOpening.transform.localScale = Vector3.one;
	}
}
