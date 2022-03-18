using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon;

public class MaskMove : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player_prefab;
    public float SpeedUp = 5.0f;
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
        /*
        if(player_prefab.transform.position.x == this.transform.position.x && player_prefab.transform.position.z == this.transform.position.z)
        {
            Destroy(this.gameObject);
        }*/
    }

    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Missil")
        {
            Destroy(this.gameObject);
        }
    }
}
