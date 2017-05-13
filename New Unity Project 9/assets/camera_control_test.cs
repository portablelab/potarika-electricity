using UnityEngine;
using System.Collections;

public class camera_control_test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("focus");
        CameraDevice.Instance.Start();
        bool focusModeSet = CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        if (!focusModeSet)
        {
            Debug.Log("no");
        }
        else
        {
            Debug.Log("yes");
        }
    }

	// Update is called once per frame
	void Update () {

	}
    void OnApplicationPause (bool pauseStatus) { 
        if (pauseStatus) { 
            //ホームボタンを押してアプリがバックグランドに移行した時 
            Debug.Log("バックグランドに移行したよ");  
        }
        else {    //アプリを終了しないでホーム画面からアプリを起動して復帰した時
            CameraDevice.Instance.Start();
            bool focusModeSet = CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
            if (!focusModeSet)
            {
                Debug.Log("no");
            }
            else
            {
                Debug.Log("yes");
            }
            Debug.Log("バックグランドから復帰したよ");
        }
    }
}
