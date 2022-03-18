using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon;


public class Enemymove : MonoBehaviour
{
    private bool start = false;
    private GameObject player_prefab;
    public float SpeedUp = 5.0f;
    public GameObject PrticlePrefab;
    private GameObject boostSparkles;
    Vector3 Goal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player_prefab = GameObject.FindGameObjectsWithTag("Player")[0];
        Goal = player_prefab.transform.position;
        this.transform.position = Vector3.MoveTowards(transform.position, Goal, Time.deltaTime * SpeedUp);
        transform.LookAt(player_prefab.transform.position);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Missil_mask")
        {
            boostSparkles = PhotonNetwork.Instantiate(PrticlePrefab.name, this.transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    /*public Vector3 KillEnemy()
    {

    }*/
}
