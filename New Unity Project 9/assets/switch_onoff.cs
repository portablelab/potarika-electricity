using UnityEngine;
using System.Collections;

public class switch_onoff : MonoBehaviour {
    bool switch_on_flag=false;
    public GameObject switchbar;
    float time=0,dt;
	// Use this for initialization
    void Start()
    {
        gameObject.transform.FindChild("switch_on").gameObject.renderer.enabled = false;
        gameObject.transform.FindChild("switch_on/Circle_001").gameObject.renderer.enabled = false;
        gameObject.transform.FindChild("switch_on/Sphere").gameObject.renderer.enabled = false;

    }
	
	// Update is called once per frame
    void Update()
    {
        dt=Time.deltaTime;
        time += dt;
        if(time>1){
            time = 0;
            switch_on_flag = !switch_on_flag;
        }

        if (switch_on_flag == false)
        {
            gameObject.transform.FindChild("switch_off").gameObject.renderer.enabled = false;
            gameObject.transform.FindChild("switch_off/Circle_001").gameObject.renderer.enabled = false;
            gameObject.transform.FindChild("switch_off/Sphere").gameObject.renderer.enabled = false;

            gameObject.transform.FindChild("switch_on").gameObject.renderer.enabled = true;
            gameObject.transform.FindChild("switch_on/Circle_001").gameObject.renderer.enabled = true;
            gameObject.transform.FindChild("switch_on/Sphere").gameObject.renderer.enabled = true;

        }
        else
        {
            gameObject.transform.FindChild("switch_on").gameObject.renderer.enabled = false;
            gameObject.transform.FindChild("switch_on/Circle_001").gameObject.renderer.enabled = false;
            gameObject.transform.FindChild("switch_on/Sphere").gameObject.renderer.enabled = false;

            gameObject.transform.FindChild("switch_off").gameObject.renderer.enabled = true;
            gameObject.transform.FindChild("switch_off/Circle_001").gameObject.renderer.enabled = true;
            gameObject.transform.FindChild("switch_off/Sphere").gameObject.renderer.enabled = true;
        }
    }

}
