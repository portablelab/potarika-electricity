using UnityEngine;
using System.Collections;
using System;

public class meter : MonoBehaviour {
    public GameObject a;
    public float x, y, z;
    public float xa, ya, za;
    public float xb, yb, zb;
    public double th,r;
    float si = -1.0f;
    parts meters;
	// Use this for initialization
	void Start () {
        a = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        r = Vector3.Distance(new Vector3(-3.6299f,4.5429f,1.028f), new Vector3(-0.7878f,0.9523f,1.028f));
        xa = this.transform.TransformPoint(-0.787f, 0.9523f, 1.028f).x;
        ya = this.transform.TransformPoint(-0.787f, 0.9523f, 1.028f).y;
        za = this.transform.TransformPoint(-0.787f, 0.9523f, 1.028f).z;
        a.gameObject.GetComponent<Renderer>().material.color = new Color(150.0f / 255.0f, 14.0f / 255.0f, 10.0f / 255.0f, 250.0f / 255.0f);
    }
	
	// Update is called once per frame
    void Update()
    {
/*        if (meters = GetComponentInParent<parts>())
        {
            if (meters.amperage > 0 && this.gameObject.transform.FindChild("minus1").GetComponent<output_point>().wire_attached_flag)
            {
                if (meters.amperage < 0.050f)
                {
                    th = (th + ((meters.amperage/0.050f) * 90.0f) - 45.0f)/2.0f;
                }
                else
                {
                    th = (th+50.0f)/2.0f;
                }
            }
            else if (meters.amperage > 0 && this.gameObject.transform.FindChild("minus2").GetComponent<output_point>().wire_attached_flag)
            {
                if (meters.amperage < 0.50f)
                {
                    th = (th + ((meters.amperage / 0.50f) * 90.0f) - 45.0f) / 2.0f;
                }
                else
                {
                    th = (th + 50.0f) / 2.0f;
                }
            }
            else if (meters.amperage > 0 && this.gameObject.transform.FindChild("minus3").GetComponent<output_point>().wire_attached_flag)
            {
                if (meters.amperage < 5.0f)
                {
                    th = (th + ((meters.amperage / 5.0f) * 90.0f) - 45.0f) / 2.0f;
                }
                else
                {
                    th = (th + 50.0f) / 2.0f;
                }
            }
            else
            {
                th = (th - 50.0f) / 2.0f;
            }
        }
   */     
        r = Vector3.Distance(new Vector3(-3.6299f, 4.5429f, 1.028f), new Vector3(-0.7878f, 0.9523f, 1.028f));
       if (th >= 45 || th <= -45) { si *= -1.0f; }
        th += si *0.1f;
        xb = this.transform.TransformPoint(-0.787f - (float)(r * Math.Cos(th * Math.PI / 180.0) * Math.Cos(50.50 * Math.PI / 180.0)), 0.9523f + (float)(r * Math.Cos(th * Math.PI / 180.0) * Math.Sin(50.50 * Math.PI / 180.0)), 1.028f + (float)(r * Math.Sin(th * Math.PI / 180.0))).x;
        yb = this.transform.TransformPoint(-0.787f - (float)(r * Math.Cos(th * Math.PI / 180.0) * Math.Cos(50.50 * Math.PI / 180.0)), 0.9523f + (float)(r * Math.Cos(th * Math.PI / 180.0) * Math.Sin(50.50 * Math.PI / 180.0)), 1.028f + (float)(r * Math.Sin(th * Math.PI / 180.0))).y;
        zb = this.transform.TransformPoint(-0.787f - (float)(r * Math.Cos(th * Math.PI / 180.0) * Math.Cos(50.50 * Math.PI / 180.0)), 0.9523f + (float)(r * Math.Cos(th * Math.PI / 180.0) * Math.Sin(50.50 * Math.PI / 180.0)), 1.028f + (float)(r * Math.Sin(th * Math.PI / 180.0))).z;
        a.transform.position = new Vector3((xa + xb) / 2.0f, (ya + yb) / 2.0f, (za + zb) / 2.0f);	//ベクトルがわからない……orz
        a.transform.eulerAngles = new Vector3(0, 0, 90f);//xz平面上での変換
        a.transform.Rotate(new Vector3(-(float)(Math.Atan2((zb - za), (xb - xa)) * (180.0 / 3.14)), 0, 0));
        a.transform.Rotate(new Vector3(0, 0, 90f - (float)(Math.Atan2(Vector3.Distance(new Vector3(xa, 0, za), new Vector3(xb, 0, zb)), (yb - ya)) * (180.0 / 3.14))));
        r = Vector3.Distance(this.transform.TransformPoint(new Vector3(-3.6299f, 4.5429f, 1.028f)), this.transform.TransformPoint(new Vector3(-0.7878f, 0.9523f, 1.028f)));

        a.transform.localScale = new Vector3((float)r * 0.05f, (float)r * 0.5f, (float)r * 0.05f);
    }
}
