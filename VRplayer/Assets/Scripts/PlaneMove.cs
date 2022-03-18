using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon;
using Valve.VR;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;
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
    private bool ranIntoSomething = false;
    public GameObject leftController;
    private Vector3 vector, planeNormal;
    private float angle;
    private float lastAngle = 0;
    private float lastAngle2 = 0;
    private Vector3 cross;

    //public GameObject testCube;
    private Vector3 lastForward;
    private float lastcross = 0;
    public SteamVR_Behaviour_Boolean steamVR_Behaviour_Boolean;
    public SteamVR_Action_Vibration hapticAction;
    void Start()
    {
        BulletInterval = 0.5f;
        LoadingTime = 5.0f;
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
            Vector3 planeForward = this.transform.forward;
            planeNormal = transform.up;
            Vector3 planeOnPlane = Vector3.ProjectOnPlane(planeForward, planeNormal);
            Vector3 controllerForward = leftController.transform.forward;
            Vector3 controllerOnPlane = Vector3.ProjectOnPlane(controllerForward, planeNormal);
            angle = Mathf.Acos(Vector3.Dot(planeOnPlane.normalized, controllerOnPlane.normalized)) * Mathf.Rad2Deg;
            cross = Vector3.Cross(planeOnPlane.normalized, controllerOnPlane.normalized);
            //Debug.Log(angle);
            if (cross.y > 0)
                angle *= -1;
            if (angle < 30 && angle > -30)
                turnSpeed = 0;
            else
            {
                if ((lastAngle - angle) < 320 && (lastAngle - angle) > -320)
                    turnSpeed = angle * -0.5f;
                else
                {
                    if (lastAngle > 0)
                        turnSpeed = 30;
                    else
                        turnSpeed = -30;
                }
                if (turnSpeed > 50)
                    turnSpeed = 50;
                else if (turnSpeed < -50)
                    turnSpeed = -50;
            }
            lastAngle = angle;

            float dot = Vector3.Dot(transform.up, controllerForward);
            float turnSpeed2 = dot * 87;
            /*if (dot < 0.6 && dot > -0.6)
                turnSpeed2 = dot * 100;
            else if (dot >= 0.6)
                turnSpeed2 = 60;
            else
                turnSpeed2 = -60;*/
            float planeAngle = Vector3.SignedAngle(planeOnPlane, planeForward, transform.right);
            if (planeAngle > 20 && dot < 0)
                turnSpeed2 = 0;
            else if (planeAngle < -20 && dot > 0)
                turnSpeed2 = 0;
            print("angle: " + planeAngle);
            //print(turnSpeed2);
            bool isTriggeredr = SteamVR_Input.GetState("default", "interactUI", SteamVR_Input_Sources.RightHand);
            bool isTriggeredl = SteamVR_Input.GetState("default", "interactUI", SteamVR_Input_Sources.LeftHand);

            this.transform.Find("Cockpit3_WithInterior/Cockpit3_InteriorDetails/Screen2/number_of_bullets/Text").gameObject.GetComponent<Text>().text = "" + GM.BulletNum;
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
                if (GM.BulletNum <= 0 && BulletLoading > LoadingTime)
                {
                    GM.BulletNum = 6;
                }
                else if (isTriggeredr && time > BulletInterval && GM.BulletNum > 0)
                {
                    GM.BulletNum--;
                    newBullet = PhotonNetwork.Instantiate(bullet.name, camera.transform.position + camera.transform.forward * 20 - new Vector3(0, 5, 0), Quaternion.identity);
                    Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
                    newBullet.transform.forward = transform.forward;
                    //bulletRb.AddForce(transform.forward * 5000);
                    bulletRb.velocity = this.transform.forward * 500;
                    time = 0;
                    BulletLoading = 0;
                }
                if (isTriggeredl | Input.GetKey(KeyCode.UpArrow)) //前
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
                this.transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
                this.transform.Rotate(Vector3.left, turnSpeed2 * Time.deltaTime);
                //this.transform.Rotate(lastForward, turnSpeed * -0.5f * Time.deltaTime);
                lastForward = Vector3.forward;
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
        Pulse(1, 150, 60, steamVR_Behaviour_Boolean.inputSource);
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
            GameObject boostSparkles = PhotonNetwork.Instantiate(PrticlePrefab.name, this.transform.position, Quaternion.identity);
        }
        Debug.Log("G");
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
        for (i = 0; i < count; i++)
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
        if (i >= 5)
        {
            GM.SetStart(false);

        }
    }

    public void FullBlood()
    {
        GameObject health = this.transform.Find("Health_bar").gameObject;
        for (int i = 0; i < 5; i++)
        {
            health.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    private void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        hapticAction.Execute(0, duration, frequency, amplitude, source);
        print("In Pluse");
    }
}


