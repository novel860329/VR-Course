using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon;

public class PlaneMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float m_speed = 50f;
    public float turnSpeed = 100.0f;
    public GameObject bullet;
    private GameObject newBullet;
    public GameObject camera;
    public GameObject PrticlePrefab;
    public GameManager GM;
    private Rigidbody rb;   
    private bool collid = false;
    private bool first = true;
    private float time = 0;
    private float BulletLoading = 0;
    private float collidTime = 0;
    private float mountTime = 0;
    private float LoadingTime;
    private float BulletInterval;
    private float CollidInterval;
    private float MountInterval;
    private int BulletNum = 6;
    private bool ranIntoSomething = false;
     
    void Start()
    {
        BulletInterval = 0.5f;
        LoadingTime = 2.0f;
        CollidInterval = 1.0f;
        MountInterval = 3.0f;
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GM.GetStart())
        {
            first = false;
            time += Time.deltaTime;
            BulletLoading += Time.deltaTime;
            collidTime += Time.deltaTime;
            mountTime += Time.deltaTime;

            this.transform.Find("Cockpit3_WithInterior/Cockpit3_InteriorDetails/Screen2/number_of_bullets/Text").gameObject.GetComponent<Text>().text = "" + BulletNum;
            if (ranIntoSomething)
            {
                if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.LeftArrow)) //左
                {
                    this.transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.D) | Input.GetKey(KeyCode.RightArrow)) //右
                {
                    this.transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
                }
            }
            else
            {
                if (BulletNum <= 0 && BulletLoading > LoadingTime)
                {
                    BulletNum = 6;
                }
                else if (Input.GetKeyDown(KeyCode.Q) && time > BulletInterval && BulletNum > 0)
                {
                    BulletNum--;
                    //newBullet = PhotonNetwork.Instantiate(bullet.name, camera.transform.position + camera.transform.forward * 20 - new Vector3(0, 5, 0), Quaternion.identity);
                    Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
                    newBullet.transform.forward = transform.forward;
                    bulletRb.AddForce(transform.forward * 5000);
                    time = 0;
                    BulletLoading = 0;
                }
                if (Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.UpArrow)) //前
                {
                    this.transform.Translate(Vector3.forward * m_speed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.LeftArrow)) //左
                {
                    this.transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.D) | Input.GetKey(KeyCode.RightArrow)) //右
                {
                    this.transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
                }
            }
            if (collid)
            {
                this.transform.Find("Cockpit3_WithInterior/Cockpit3_Glass_attack").gameObject.SetActive(true);
                this.transform.Find("Cockpit3_WithInterior/Cockpit3_InteriorDetails/Screen2/Attack").gameObject.SetActive(true);
                this.transform.Find("Cockpit3_WithInterior/Cockpit3_InteriorDetails/Screen2/Safe").gameObject.SetActive(false);
                Debug.Log("Active");
                collidTime = 0;
                collid = false;
            }
            else if (collidTime > CollidInterval)
            {
                this.transform.Find("Cockpit3_WithInterior/Cockpit3_Glass_attack").gameObject.SetActive(false);
                this.transform.Find("Cockpit3_WithInterior/Cockpit3_InteriorDetails/Screen2/Attack").gameObject.SetActive(false);
                this.transform.Find("Cockpit3_WithInterior/Cockpit3_InteriorDetails/Screen2/Safe").gameObject.SetActive(true);
                collid = false;
            }
        }
        else
        {
            string txt = "";
            if (!first)
            {
                txt = "\nGame Over";
                this.transform.Find("Cockpit3_WithInterior/Cockpit3_Glass_attack").gameObject.SetActive(true);
                this.transform.Find("Cockpit3_WithInterior/Cockpit3_InteriorDetails/Screen2/Attack").gameObject.SetActive(true);
                this.transform.Find("Cockpit3_WithInterior/Cockpit3_InteriorDetails/Screen2/Attack/Text").gameObject.GetComponent<Text>().text = txt;
                this.transform.Find("Cockpit3_WithInterior/Cockpit3_InteriorDetails/Screen2/Safe").gameObject.SetActive(false);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Missil")
        {
            Debug.Log("-1 blood");
            HealthBar(1);
            //PhotonNetwork.Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Missil2")
        {
            Debug.Log("-1 blood");
            HealthBar(1);
            //PhotonNetwork.Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Missil3")
        {
            Debug.Log("-2 blood");
            HealthBar(2);
            //PhotonNetwork.Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Missil4")
        {
            Debug.Log("-2 blood");
            HealthBar(2);
            //PhotonNetwork.Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Missil5")
        {
            Debug.Log("-3 blood");
            HealthBar(3);
            //PhotonNetwork.Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "mountain")
        {
            mountTime = 0;
            ranIntoSomething = true;
            Debug.Log("-5 blood");
            HealthBar(5);
            //GameObject boostSparkles = PhotonNetwork.Instantiate(PrticlePrefab.name, this.transform.position, Quaternion.identity);            
        }
        collid = true;
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "mountain")
        {
            ranIntoSomething = false;
        }
    }

    public void HealthBar(int damage)
    {
        int i = 0;
        GameObject health = this.transform.Find("Health_bar").gameObject;
        int count = health.transform.childCount;
        for(i = 0; i < count; i++)
        {
            if (health.transform.GetChild(i).gameObject.activeSelf)
                break;
        }
        if (i < 5)
        {
            for (int j = 0; j < damage; j++)
            {
                if (i <= 4)
                {
                    health.transform.GetChild(i).gameObject.SetActive(false);
                    i++;
                }
            }
        }
        if(i >= 5)
        {
            GM.SetStart(false);
        }
    }
}
