using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMask : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player_prefab;
    // Start is called before the first frame update
    void Start()
    {
        player_prefab = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = player_prefab.transform.position;
    }

}
