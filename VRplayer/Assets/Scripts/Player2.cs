using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Player2 : MonoBehaviour
{

    private GameObject ball;
    public float speed = 0.5f;
    private float windx;
    private float windy;
    public float high;
    public GameManager GM;
    public Text TimeText;
    protected Joystick joystick;
    private bool start = false;
    private float windTime = 0;
    private float WindInterval;
    private float timer_f;
    private float timer_ss;
    private int timer_s;
    private int timer_m;
   // private int toatl_time;

    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        this.transform.position = new Vector3(4532f, 1150f, 2982f);
        this.transform.position += new Vector3(0, high, 0);
        this.transform.Rotate(90, 0, 0);
        Random.InitState(System.Guid.NewGuid().GetHashCode());
        WindInterval = 10.0f;
        timer_f = 90;
        timer_ss = 0;
        timer_s = 0;
        timer_m = 3;
    }



    // Update is called once per frame
    void Update()
    {
        if (GM.GetStart())
        {
            windTime += Time.deltaTime;

            timer_f -= Time.deltaTime;
           
            timer_s = ((int)timer_f) % 60;
            timer_m = (int)timer_f / 60;
            timer_ss = timer_f - (timer_s+timer_m*60);

        

            //if (timer_m < 10)TimeText.text = "Time: " + timer_m .ToString() + ":" + timer_s + ":" + timer_ss.ToString()[2] + timer_ss.ToString()[3];
            
            TimeText.text = "Time: " + timer_m.ToString() + ":" + timer_s + ":" + timer_ss.ToString()[2] + timer_ss.ToString()[3];
            
            if (timer_f <= 0)
            {
                timer_f = 90;
                GM.LevelUp();
            }

            if (windTime > WindInterval)
            {
                windx = Random.Range(-2, 2);
                windy = Random.Range(-1, 1);
                Debug.Log(windx);
                Debug.Log(windy);
                windTime = 0;
            }
            
            var rigidbody = GetComponent<Rigidbody>();
            rigidbody.velocity = new Vector3(joystick.Horizontal * speed, rigidbody.velocity.y, joystick.Vertical * speed);

            this.transform.position += new Vector3(windx, 0, windy);

        }
    }
}
