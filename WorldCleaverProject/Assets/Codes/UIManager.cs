using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//�������� UI�� �����ϴ� ��ũ��Ʈ
//�̱����̱⿡ GameManager�� �ٷ� ���� �����ϴ�.
public class UIManager : SingleTon<UIManager>
{
	public Slider treeSlider; //���� ü���� ��Ÿ���ִ� �����̴� UI
	public Slider playerSlider; //�÷��̾� ����� ��Ÿ���ִ� �����̴� UI
	public Slider EnemySlider; //�� ����� ��Ÿ���ִ� �����̴� UI
	public Button HitButton; //HIT ��ư UI

	public Canvas VillageCanvas;
	public Canvas OppositeVillageCanvas;
	public Button ToVillageButton;
	public Button ToOppositeVillageButton;
	public Slider VillageSlider;
	public Slider OppositeVillageSlider;
	public Slider MyVillageSliderInMain;
	public Slider OppositeVillageSliderInMain;
	public GameObject ItemListPanel;

	public GameObject YouWinPanel;
	public GameObject YouLosePanel;

	private float InitHealth; //���� ü���� �ʱ� ���� �����ϱ� ���� ���� 
	private float InitPlayerPower; //�÷��̾� ü���� �ʱ갪�� �����ϱ� ���� ����
	private float InitEnemyPower; //�÷��̾� ü���� �ʱ갪�� �����ϱ� ���� ����
	private float InitVillageHealth;//���� ü���� �ʱ갪�� �����ϱ� ���� ����
	private float InitOppositeVillageHealth;
	private CanvasGroup SliderGroup;

	//������ ������ ���� �÷��� ����
	private int flag = 0;

	private void Start()
	{
		YouLosePanel.SetActive(false);
		YouWinPanel.SetActive(false);

		//�ʱ�ȭ
		InitHealth = GameManager.Instance.TreeController.treeHealth;
		InitPlayerPower = GameManager.Instance.PlayerController.Mana;
		InitEnemyPower = GameManager.Instance.EnemeyController.Mana;
		InitVillageHealth = VillageManager.Instance.VilageHelath;
		InitOppositeVillageHealth = OppositeVillageManager.Instance.OppositeVillageHealth;

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
	}

	private void Update()
	{
		//�߿�! ���� ������ ����� ���
		if (GameManager.Instance.Turn == 44)
		{
			//������ ������ ���� ����Ǿ����� Ȯ���Ѵ�.
			GameManager.Instance.WhoLoseGame();
			GameManager.Instance.AnimationManager.Dead();
			//Hit ��ư�� ��Ȱ��ȭ �Ѵ�.
			HitButton.interactable = false;
			//���� ���� Canvas ��Ȱ��ȭ
			ToVillageButton.interactable = false;
			ToOppositeVillageButton.interactable = false;

			//���� �¸��ڸ� ���� UI�� Ȱ��ȭ�Ѵ�.
			if(GameManager.Instance.WhoLose == 0)
			{
				YouLosePanel.SetActive(true);
			}
			else
			{
				YouWinPanel.SetActive(true);
			}

			//�׸��� ���� �÷��װ� 1�̸� �̹� ��� �ٰ� �ʱ�ȭ�� �����̹Ƿ� �״�� return�Ѵ�.
			if (flag == 1) return;
			//���� �÷��װ� 0�̸�, ��� ���� ������ ���� �̷������ ���̴�. 
			//����, ��� ���� ������ ���� ��, ���� ó���� �ϱ� ���� flag ����
			//���� ��� ���� ������ ���� �Ŀ�, flag ���� 1�� ������Ʈ �Ǹ鼭 return�� �ȴ�.
			else flag = 1;
		}

		//���� ���� ����ź �������� �������̸�
		if(ItemManager.Instance.smokeFlag != 0)
		{
			//������ ���� ���
			if (GameManager.Instance.Turn == 44)
			{
				//����ź ȿ���� �����Ѵ�.
				SliderGroup.alpha = 1f;
			}
			else
			{
				//�ƴϸ� �����Ѵ�.
				SliderGroup.alpha = 0f;
			}
		}
		else
		{
			SliderGroup.alpha = 1f;
		}

		//���� ���� ü�� �޾ƿ���
		float curHealth = GameManager.Instance.TreeController.treeHealth;
		//���� ü�� �����̴� ������Ʈ
		treeSlider.value = curHealth / InitHealth;

		//�׳� Turn ���� �ҷ�����, �̹� Turn�� ��ü�� ���¿��� ������ ����ǹǷ� ���� �߻�
		//����, ���� Turn�� ������ myTurn ���� �ҷ��ͼ� �ش� ���� ���� ���� �÷��̾ �Ǵ��Ѵ�.
		int Turn = GameManager.Instance.myTurn;

		//�׳� ��ÿ� ������Ʈ �� �� �ֵ��� ����
		//���� �÷��̾� ��� �޾ƿ���
		float curPower_p = GameManager.Instance.PlayerController.Mana;
		//�÷��̾� ��� �����̴� ������Ʈ
		playerSlider.value = curPower_p / InitPlayerPower;

		float curPower_e = GameManager.Instance.EnemeyController.Mana;
		EnemySlider.value = curPower_e / InitEnemyPower;

		//���� ü�� �����̴� ����
		float VillageHealth = VillageManager.Instance.VilageHelath;
		VillageSlider.value = VillageHealth / InitVillageHealth;
		MyVillageSliderInMain.value = VillageSlider.value;

		float OppositeVillageHealth = OppositeVillageManager.Instance.OppositeVillageHealth;
		OppositeVillageSlider.value = OppositeVillageHealth / InitOppositeVillageHealth;
		OppositeVillageSliderInMain.value = OppositeVillageSlider.value;


		////�÷��̾� ���̸�
		//if (Turn == 0)
		//{
		//	//���� �÷��̾� ��� �޾ƿ���
		//	float curPower = GameManager.Instance.PlayerController.Mana;
		//	//�÷��̾� ��� �����̴� ������Ʈ
		//	playerSlider.value = curPower / InitPlayerPower;
		//}
		//else if (Turn == 1) //�� ���̸�
		//{
		//	float curPower = GameManager.Instance.EnemeyController.Mana;
		//	EnemySlider.value = curPower / InitEnemyPower;
		//}

		//���� �� ���̸� ��ư�� ��Ȱ��ȭ ��Ų��.
		if(GameManager.Instance.Turn == 1) ToVillageButton.interactable = false;
		else if(GameManager.Instance.Turn == 0) //���� �� ���̸�
		{
			//���� ���� ������ Full ��� Ȥ�� Empty ����� �߰� �ִ� ���¸�
			if(DisplayWarningMessage.Instance.WarningFlag == 1 || DisplayEmptyMessage.Instance.WarningFlag == 1)
			{
				//��Ȱ��ȭ
				ToVillageButton.interactable = false;
				ToOppositeVillageButton.interactable = false;
			}
			else
			{
				ToVillageButton.interactable = true;
				ToOppositeVillageButton.interactable = true;
			}

			//���� ���� ������ ���� â�� �� �ִ� ���
			if (ItemListPanel.transform.localScale == Vector3.one)
			{
				//��ư ��Ȱ��ȭ
				ToVillageButton.interactable = false;
				ToOppositeVillageButton.interactable = false;
			}
			else
			{
				//�ƴϸ� �ٽ� Ȱ��ȭ
				ToVillageButton.interactable = true;
				ToOppositeVillageButton.interactable = true;
			}
		}

		//���� �÷��װ� 1�̸� ���� ������ �糢�� �ٷ� �����Ѵ�.
		if (flag == 1) return;
		//���� Hit ����� �������̸� ��ư�� ��Ȱ��ȭ ��Ų��.
		if (GameManager.Instance.AnimationManager.isHitingTree == 1)
		{
			HitButton.interactable = false;
			//���� ��ư ���� ��Ȱ��ȭ��Ų��.
			ToVillageButton.interactable = false;
			ToOppositeVillageButton.interactable = false;
		}
		//�� ���� ��� Hit ��ư�� ��Ȱ��ȭ ��Ų��.
		else if(GameManager.Instance.Turn == 1)
		{
			HitButton.interactable = false;
		}
		else if(ItemManager.Instance.FuncFlag == true)
		{
			HitButton.interactable = false;
		}
		else
		{
			HitButton.interactable= true;
		}
	}
}
