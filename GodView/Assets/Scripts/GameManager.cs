using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon;

public class GameManager : MonoBehaviour
{
    private bool start = false;
    public float X1;
    public float X2;
    public float Z1;
    public float Z2;
    public GameObject playerGod;
    public GameObject playerPlane;
    private GameObject player_prefab;
    public GameObject[] enemy_prefab;
    public GameObject local_mask;
    public GameObject local_enemy;
    public float t1;
    private float t2;
    private GameObject[] particleSystems;
    // Start is called before the first frame update
    void Start()
    {
        t2 = t1;
        Random.InitState(System.Guid.NewGuid().GetHashCode());
        playerGod.GetComponent<Player2>().GM = this;
        playerPlane.GetComponent<PlaneMove>().GM = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            start = true;
            player_prefab = GameObject.FindGameObjectsWithTag("Player")[0];
            Debug.Log("Game Start!!");
        }
        if (start)
        {
            t2 = t2 - Time.deltaTime;
            if (t2 <= 0)
            {
                //NewEnemy();
                t2 = t1;                                       //重複賦值，重複執行
            }
        }
        else
        {
           // CleanMissil();
        }
        //CleanParticle();
    }

    public void NewEnemy()
    {
        float x = player_prefab.transform.position.x;
        float z = player_prefab.transform.position.z;
        while (x == player_prefab.transform.position.x && z == player_prefab.transform.position.z)
        {
            x = Random.Range(X1, X2);
            z = Random.Range(Z1, Z2);
        }
        float rMissil = Random.Range(1, 5);
        switch(rMissil)
        {
            case 1:
                local_enemy = PhotonNetwork.Instantiate(enemy_prefab[0].name, new Vector3(x, player_prefab.transform.position.y, z), Quaternion.identity);
                break;
            case 2:
                local_enemy = PhotonNetwork.Instantiate(enemy_prefab[1].name, new Vector3(x, player_prefab.transform.position.y, z), Quaternion.identity);
                break;
            case 3:
                local_enemy = PhotonNetwork.Instantiate(enemy_prefab[2].name, new Vector3(x, player_prefab.transform.position.y, z), Quaternion.identity);
                break;
            case 4:
                local_enemy = PhotonNetwork.Instantiate(enemy_prefab[3].name, new Vector3(x, player_prefab.transform.position.y, z), Quaternion.identity);
                break;
            case 5:
                local_enemy = PhotonNetwork.Instantiate(enemy_prefab[4].name, new Vector3(x, player_prefab.transform.position.y, z), Quaternion.identity);
                break;
        }
        Debug.Log(local_enemy);
        local_enemy.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void CleanParticle()
    {
        particleSystems = GameObject.FindGameObjectsWithTag("Expos");
        foreach (GameObject ps in particleSystems)
        {
            if (ps.GetComponent<ParticleSystem>().isStopped)
            {
                //PhotonNetwork.Destroy(ps.gameObject);
            }
        }
    }
    public void CleanMissil()
    {
        GameObject[][] Missil = { GameObject.FindGameObjectsWithTag("Missil") , GameObject.FindGameObjectsWithTag("Missil2"),
                                  GameObject.FindGameObjectsWithTag("Missil3"), GameObject.FindGameObjectsWithTag("Missil4"),
                                  GameObject.FindGameObjectsWithTag("Missil5")};
        for (int i = 0; i < 5; i++)
        {
            foreach (GameObject m in Missil[i])
            {
                if (m != null) {
                    Debug.Log("Destroy missil");
                    //PhotonNetwork.Destroy(m.gameObject);
                }
            }
        }
    }
    public bool GetStart()
    {
        return start;
    }
    public void SetStart(bool s)
    {
        this.start = s;
    }
}
