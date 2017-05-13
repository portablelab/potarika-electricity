using UnityEngine;
using System.Collections;

public class KeyEvent : MonoBehaviour {
	static AndroidJavaObject    m_plugin = null;
	static GameObject           m_instance;
	
	public void Awake () {
		// gameObject変数はstaticでないのでstatic関数から呼ぶことが出来ない.
		// そのためstaticの変数にあらかじめコピーしておく.
		m_instance = gameObject;
		#if UNITY_ANDROID && !UNITY_EDITOR
		// プラグイン名をパッケージ名+クラス名で指定する。
		m_plugin = new AndroidJavaObject( "org.portable_lab.unitytest.Test" );
#endif
        print ("Awake!!");
	}

	// Use this for initialization
	void Start () {
		print ("Start!!");
		CallStartUnity();
	}
	
	// Update is called once per frame
	void Update () {
		//Androidで戻るキーでアプリを終了させる
		if(Application.platform == RuntimePlatform.Android && Input.GetKey(KeyCode.Escape)){
			print ("BackKey!!");
			CallEndUnity();
			Application.Quit();
		}
	}

	public static void CallStartUnity()
	{
		print ("CallStartUnity!!");
		#if UNITY_ANDROID && !UNITY_EDITOR
		if (m_plugin != null){
			m_plugin.Call("StartUnity");
		}
		#endif
	}
	
	public static void CallEndUnity(){
		print ("CallEndUnity!!");
		#if UNITY_ANDROID && !UNITY_EDITOR
		if (m_plugin != null){
			m_plugin.Call("EndUnity");
		}
		#endif
	}
}
