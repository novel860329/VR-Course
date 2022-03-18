using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon;

public class PlayerBullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxDistance = 200;
    public GameObject PrticlePrefab;

    public int power;
    Vector3 initPos;
    void Start()
    {
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float diffX = Math.Abs(initPos.x - transform.position.x);
        float diffY = Math.Abs(initPos.y - transform.position.y);
        float diffZ = Math.Abs(initPos.z - transform.position.z);

        // destroy if it's too far away
        if (diffX >= maxDistance || diffY >= maxDistance || diffZ >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // check if we hit an enemy
        if(collision.gameObject.tag == "mountain")
        {
            GameObject boostSparkles = PhotonNetwork.Instantiate(PrticlePrefab.name, this.transform.position, Quaternion.identity);
        }
        //GameObject boostSparkles = PhotonNetwork.Instantiate(PrticlePrefab.name, this.transform.position, Quaternion.identity);
        PhotonNetwork.Destroy(this.gameObject);
    }
}
