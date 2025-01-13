using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance = null;

	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (T)FindObjectOfType(typeof(T));

				if (instance == null)
				{
					GameObject obj = new GameObject(typeof(T).Name, typeof(T));
					instance = obj.GetComponent<T>();
				}
			}
			return instance;
		}
	}

	//private void Start()
	//{
	//	Debug.Log($"Single Tone : {gameObject.name}");
	//	// "DDOL" �±װ� ������ ��츸 DontDestroyOnLoad ����
	//	if (gameObject.CompareTag("BGM"))
	//	{
	//		Debug.Log($"DontDestroyOnLoad applied to: {gameObject.name}");
	//		// �θ� ������ ��Ʈ ������Ʈ�� ����
	//		if (transform.parent != null)
	//		{
	//			DontDestroyOnLoad(transform.root.gameObject);
	//		}
	//		// �θ� ������ ���� ������Ʈ�� ����
	//		else
	//		{
	//			DontDestroyOnLoad(gameObject);
	//		}
	//	}
	//	else if (gameObject.CompareTag("DDOL"))
	//	{
	//		Debug.Log($"DontDestroyOnLoad applied to: {gameObject.name}");
	//		// �θ� ������ ��Ʈ ������Ʈ�� ����
	//		if (transform.parent != null)
	//		{
	//			DontDestroyOnLoad(transform.root.gameObject);
	//		}
	//		// �θ� ������ ���� ������Ʈ�� ����
	//		else
	//		{
	//			DontDestroyOnLoad(gameObject);
	//		}
	//	}
	//}

	private void OnEnable()
	{

		if (instance != null && instance != this)
		{
			Destroy(this.gameObject); // �ߺ� �ν��Ͻ� ����
			return; // ���� �ڵ� ���� ����
		}

		//Debug.Log($"Single Tone : {gameObject.name}");
		if (transform.parent != null && transform.root != null)
		{
			if(gameObject.CompareTag("DDOL") || gameObject.CompareTag("BGM"))
				DontDestroyOnLoad(this.transform.root.gameObject);
		}
		else
		{
			if (gameObject.CompareTag("DDOL") || gameObject.CompareTag("BGM"))
				DontDestroyOnLoad(this.gameObject);
		}
	}
	//}

	//private void Awake()
	//{
	//	if (Instance != this)
	//	{
	//		Destroy(gameObject);
	//		return;
	//	}

	//	DontDestroyOnLoad(gameObject);
	//}

}
