using UnityEngine;
using System.Collections;

public class enlight : MonoBehaviour
{
    GameObject[] lightbox;
    public GameObject spotlight;
    int i;
	// Use this for initialization
	void Start () {
        lightbox = new GameObject[9];
        spotlight.light.intensity = 0.3f;
        for (i = 0; i < 9; i++)
        {
            lightbox[i] = (GameObject)GameObject.Instantiate(spotlight);
            if (i == 5)
                lightbox[i].transform.position = this.transform.position + new Vector3(i / 3 - 1, 3, i % 3 - 1);
            else
                lightbox[i].transform.position = this.transform.position + new Vector3(i / 3 - 1, 1, i % 3 - 1);
        }
	}
	
	// Update is called once per frame
    void Update()
    {
 //       this.transform.Rotate(0,1f, 0);
 //       this.transform.Translate(0,0,1f);
        for (i = 0; i < 9; i++)
            {
                lightbox[i].light.intensity += 0.01f;
                if (i == 5)
                    lightbox[i].transform.position = this.transform.position + new Vector3(i / 3 - 1, 3, i % 3 - 1);
                else
                    lightbox[i].transform.position = this.transform.position + new Vector3(i / 3 - 1, 1, i % 3 - 1);
            }
	}
}
