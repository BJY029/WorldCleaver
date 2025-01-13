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
            if(instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if(instance == null)
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
	//	// "DDOL" 태그가 설정된 경우만 DontDestroyOnLoad 적용
	//	if (gameObject.CompareTag("BGM"))
	//	{
	//		Debug.Log($"DontDestroyOnLoad applied to: {gameObject.name}");
	//		// 부모가 있으면 루트 오브젝트를 유지
	//		if (transform.parent != null)
	//		{
	//			DontDestroyOnLoad(transform.root.gameObject);
	//		}
	//		// 부모가 없으면 현재 오브젝트를 유지
	//		else
	//		{
	//			DontDestroyOnLoad(gameObject);
	//		}
	//	}
	//	else if (gameObject.CompareTag("DDOL"))
	//	{
	//		Debug.Log($"DontDestroyOnLoad applied to: {gameObject.name}");
	//		// 부모가 있으면 루트 오브젝트를 유지
	//		if (transform.parent != null)
	//		{
	//			DontDestroyOnLoad(transform.root.gameObject);
	//		}
	//		// 부모가 없으면 현재 오브젝트를 유지
	//		else
	//		{
	//			DontDestroyOnLoad(gameObject);
	//		}
	//	}
	//}

	private void Awake()
	{
		//Debug.Log($"Single Tone : {gameObject.name}");
		if (transform.parent != null && transform.root != null)
		{
			DontDestroyOnLoad(this.transform.root.gameObject);
		}
		else
		{
			DontDestroyOnLoad(this.gameObject);
		}

	}
}
