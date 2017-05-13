using UnityEngine;
using System.Collections.Generic;

public class Eclipse: MonoBehaviour {
	static AndroidJavaObject    m_plugin = null;
	static GameObject           m_instance;
	
	public void Awake () {
		// gameObject変数はstaticでないのでstatic関数から呼ぶことが出来ない.
		// そのためstaticの変数にあらかじめコピーしておく.
		m_instance = gameObject;
		#if UNITY_ANDROID && !UNITY_EDITOR
		// プラグイン名をパッケージ名+クラス名で指定する。
		m_plugin = new AndroidJavaObject( "org.portable_lab.unitytest.UnityState" );
#endif
    }
	
	// NativeコードのFuncB 関数を呼び出す.
	public static int CallFuncB()
	{
		int modoriValue = -3;
		#if UNITY_ANDROID && !UNITY_EDITOR
		if (m_plugin != null){
			modoriValue = m_plugin.Call<int>("FuncB");
		}
		#endif
		return modoriValue;
	}
	
	// ネイティブコードから呼ばれる関数
	// publicでかつ、非static関数でないと呼ばれない.
	public void onCallBack(string str)
	{
		Debug.Log("Call From Native. (" + str + ")");
	}
	
}