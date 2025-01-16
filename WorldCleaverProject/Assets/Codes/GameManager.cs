using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    public PlayerController PlayerController;
    public EnemeyController EnemeyController;
    public TreeController TreeController;
    public AnimationManager AnimationManager;
    public CameraManager CameraManager;
    public DeerController DeerController;
    public DisplayEmptyMessage DisplayEmptyMessage;
    public DisplayEnemyItems DisplayEnemyItems;
    //public DisplayItems DisplayItems;
    public DisplayPlayerItems DisplayPlayerItems;
    public DisplayWarningMessage DisplayWarningMessage;
    public EffectAudioManager EffectAudioManager;
    public EnemyAI EnemyAI;
    public EnemyDeerController EnemyDeerController;
    public EnemyEagleController EnemyEagleController;
    public Horse1Controller Horse1Controller;
    public Horse2Controller Horse2Controller;
    public HorseManager HorseManager;
    public ItemManager ItemManager;
    public VillageManager VillageManager;
    public OppositeVillageManager OppositeVillageManager;
    public PlayerEagleController PlayerEagleController;
    //public SelectHorse SelectHorse;
    public SmokeEffect SmokeEffect;
    //public ToolTipsManager ToolTipsManager;
    public UIManager UIManager;

    public int Turn;
    public int myTurn;
    public int WhoLose;
    //�罿�� ��Ÿ ģ ��� �ش� �÷��׷� ���п��θ� �Ǻ��Ѵ�.
    public int DeerLastHit;
    public int ReasonFlag; //���� ������ ������ ������ �����ϴ� �÷���
    //0 : ��Ÿ, 1 : ������ �� ������, 2 : ���� ü���� �� ������

    public int MyVillageWeight;
    public int OppositeVillageWeight;
    public int MyDamageTotalCnt;
    public int OppositeDamageTotalCnt;

	private void Awake()
	{
        //���� 0, 1���� �����ؼ� Hit�� ������ �ٲ��.
        //0�� �÷��̾�, 1�� ������ �ϸ�, �ϴ� �÷��̾���� ���� �����ϵ��� �Ѵ�.
        Turn = 0;
        WhoLose = -1;
        DeerLastHit = -1;

        MyVillageWeight = 1;
        OppositeVillageWeight = 1;

        MyDamageTotalCnt = 0;
        OppositeDamageTotalCnt = 0;

        //�罿 ������ ��Ȱ��ȭ
        GameManager.Instance.DeerController.DeerActivated = false;
        GameManager.Instance.DeerController.Deer.SetActive(false);
        GameManager.Instance.EnemyDeerController.DeerActivated = false;
        GameManager.Instance.EnemyDeerController.Deer.SetActive(false) ;
	}

    //���� Turn�� ���� �����ϴ� Hit��
    //�Լ��� �ٸ���.
    //�̸� �����ϴ� ���� �޴��� �Լ��̴�.
    public void Hit()
    {
        //���� ü���� �� �ϸ��� ���̰� �ȴ�.
        GameManager.Instance.VillageManager.VilageHelath = -20f * MyVillageWeight;
        GameManager.Instance.OppositeVillageManager.OppositeVillageHealth = -20f * OppositeVillageWeight;

        GameManager.Instance.VillageManager.ChangeBackGround();
        GameManager.Instance.OppositeVillageManager.ChangeBackGround();

        //0�� �ƴϸ�, 0�� �ɶ����� 1�� ���ҽ�Ų��.
        //0�� �ƴѰ��, ü�� �ٸ� �����. ��, ����ź �������� ���� �ð��̴�.
        if (GameManager.Instance.ItemManager.smokeFlag != 0) GameManager.Instance.ItemManager.smokeFlag -= 1;
        if (GameManager.Instance.ItemManager.smokeFlag == 0)
        {
            GameManager.Instance.EffectAudioManager.StopSmoke();
            GameManager.Instance.SmokeEffect.StopSmoke();
        }

        //���� �÷��̾� ���̸�
        if (Turn == 0)
        {
            //�켱 �� ���� ���� �� ��
            myTurn = 0;
            //�÷��̾� ��Ʈ�ѷ��� Hit �Լ��� �����Ѵ�.
            PlayerController.Hit();
        }
        else if (Turn == 1) //���� �� ���̸�
        {
            //���� ���� ������ ��
            myTurn = 1; 
            //�� ��Ʈ�ѷ��� Hit �Լ��� �����Ѵ�.
            EnemeyController.Hit();
        }

        //�� Hit�� ����� �°� �������� �ο��Ѵ�.
        StartCoroutine(DamageTree(myTurn));
        //      if (myTurn == 0)
        //      {
        //          GameManager.Instance.TreeController.DamageHitPlayer();
        //          //�׸��� Hit�� ���������� ���� ���� ���, ������ ���ϴ� �������� ������Ų��.
        //          checkingMyTotalDamage();
        //          GameManager.Instance.TreeController.MyDamageCoef = 1.0f;
        //      }
        //      else if (myTurn == 1)
        //      {
        //          GameManager.Instance.TreeController.DamageHitOppositePlayer();
        //          checkingOppositeTotalDamage();
        //	GameManager.Instance.TreeController.OppositeDamageCoef = 1.0f;
        //}

        
        Debug.Log("Turn : " + Turn);
	}

    IEnumerator DamageTree(int turn)
    {
        yield return new WaitForSeconds(Instance.AnimationManager.WaitHitTime);
        if(turn == 0)
        {
			GameManager.Instance.TreeController.DamageHitPlayer();
			//�׸��� Hit�� ���������� ���� ���� ���, ������ ���ϴ� �������� ������Ų��.
			checkingMyTotalDamage();
			GameManager.Instance.TreeController.MyDamageCoef = 1.0f;
		}
        else if(turn == 1)
        {
			GameManager.Instance.TreeController.DamageHitOppositePlayer();
			checkingOppositeTotalDamage();
			GameManager.Instance.TreeController.OppositeDamageCoef = 1.0f;
		}
        Debug.Log("Turn : " + Turn);

        //���� ��Ÿ�� ������ �������� ���
        if (Turn != 44)
        {
            GameManager.Instance.CameraManager.changeCamera();

            //���� ���� �� ���ʿ��ٸ�, ������ ���� �ѱ��.
            if (turn == 0)
            {
                GameManager.Instance.EnemyAI.EnemyTurnBehavior();
            }
            else if (turn == 1)
            {
                if (GameManager.Instance.DisplayPlayerItems.isFull() == false)
                {
                    GameManager.Instance.ItemManager.SetRandomItemsOnButtons(); //�� �Ͽ���, Hit�� �߻��Ǹ�, ������ ���� â�� ǥ���Ѵ�.
                }
                else //�������� �� �� ���
                {
                    GameManager.Instance.DisplayWarningMessage.itemIsFull(); //��� �޽��� ���
                }
            }
        }
		//Instance.AnimationManager.Hit();
		//�÷��� �ʱ�ȭ
		GameManager.Instance.ItemManager.Flag = -1;
        BGMManager.Instance.CheckState();
	}

    //Turn�� ��ü�ϴ� �Լ��̴�.
    //GameManager ���� �Ѱ��Ѵ�.
    public void ChangeTurn()
    {
        //���� Turn�� 0�̸�
        if (Turn == 0)
        {
            //������ ���� �ʱ�ȭ�Ѵ�.
            Turn = 1;
            GameManager.Instance.DisplayPlayerItems.disableButtons();
            //������ ���� ���� ��, �Լ��� ȣ���Ѵ�.
            //EnemyAI.Instance.EnemyTurnBehavior();
        }
        //���� Turn�� 1�̸�
        else if (Turn == 1)
        {
            //�÷��̾�� ���� �ʱ�ȭ�Ѵ�.
            Turn = 0;
            //DisplayPlayerItems.Instance.beableButtons();
        }
        //���� Trun�� 44�̸�
        else if (Turn == 44)
        {
            //� ������ ������ �������Ƿ� �ش� ó���� �����Ѵ�.
            Debug.Log("GameOver");
            return;
        }
	}
    
    //������ �������� �������� ������ �����ϴ� �Լ�
    //���� ���ϴ� �������� 3�� �̳��� 100�̻��� ���� ���� ���
    //������ �������� �������� �����ϰ� �ȴ�.(x2, x3, x4, ...)
    public void checkingMyTotalDamage()
    {
        int Damage = GameManager.Instance.TreeController.HitDamage;
        if(Damage > 100)
        {
            MyDamageTotalCnt = 0;
        }
        else
        {
            MyDamageTotalCnt++;
        }

        MyVillageWeight = MyDamageTotalCnt / 3 + 1;
    }


	//���� ������ �������� �������� ������ �����ϴ� �Լ�
	//���� ���ϴ� �������� 3�� �̳��� 100�̻��� ���� ���� ���
	//�� ������ �������� �������� �����ϰ� �ȴ�.(x2, x3, x4, ...)
	public void checkingOppositeTotalDamage()
    {
        int Damage = GameManager.Instance.TreeController.HitDamageFromOp;
        if(Damage > 100)
        {
            OppositeDamageTotalCnt = 0;
        }
        else
        {
            OppositeDamageTotalCnt++;
        }
        OppositeVillageWeight = OppositeDamageTotalCnt / 3 + 1;
    }

    //������ ���ڸ� �Ǻ��ϴ� �Լ�
    public void WhoLoseGame()
    {
        //���� ��Ÿ �ļ� ������ ����� ���
		if (GameManager.Instance.TreeController.treeHealth == 0)
		{
            ReasonFlag = 0;
            //Debug.Log("������ ü���� �� �Ǿ�����,");
            //�罿�� ��Ÿ�� ģ ���
            //�ش� �÷��״� TreeController���� �ο��ȴ�.
            if(DeerLastHit == 0)
            {
                //Debug.Log("�÷��̾ ��Ÿ�� �ƽ��ϴ�.");
                WhoLose = 0;
                return;
            }
            else if(DeerLastHit == 1)
            {
				//Debug.Log("���� ��Ÿ�� �ƽ��ϴ�.");
				WhoLose = 1;
                return;
            }
            //���� ��Ÿ ģ ���
            if (myTurn == 0)
            {
				//Debug.Log("�÷��̾ ��Ÿ�� �ƽ��ϴ�.");
				//���ڴ� ��
				WhoLose = 0;
            }
            else if(myTurn == 1)
            {
				//Debug.Log("���� ��Ÿ�� �ƽ��ϴ�.");
				//���ڴ� ���
				WhoLose = 1;
            }
            return;
		}

        //�������� ������ �� �������� ������ ����� ���
        //�� ������ �� ������ ���
		if (GameManager.Instance.PlayerController.Mana == 0)
        {
			//Debug.Log("���� ������ �� �������� ������ �����ϴ�.");
			ReasonFlag = 1;
            //�й�� ��
            WhoLose = 0;
            return;
        }
        //��� ������ �� ������ ���
        else if(GameManager.Instance.EnemeyController.Mana == 0)
        {
			//Debug.Log("���� ������ �� �������� ������ �����ϴ�.");
			ReasonFlag = 1;
            //���ڴ� �����
            WhoLose = 1;
            return;
        }

        //�� ������ ü���� �� ��Ƽ� ������ ������ ���
		if (GameManager.Instance.VillageManager.VilageHelath == 0)
		{
			//Debug.Log("���� ���� ü���� ��� �����Ǿ� ������ �����ϴ�..");
			ReasonFlag = 2;
			WhoLose = 0;
			return;
		}
        else if(GameManager.Instance.OppositeVillageManager.OppositeVillageHealth == 0)
        {
			//Debug.Log("���� ���� ü���� ��� �����Ǿ� ������ �����ϴ�..");
			ReasonFlag = 2;
            WhoLose = 1;
            return;
        }
	}
}
