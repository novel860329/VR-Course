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
    public int BulletNum;
    private int last_kind;
    private List<int> NumOfMissiles = new List<int>();
    public int totalNumOfMissiles;
    private int originNumOfMissiles;
    public GameObject playerGod;
    public GameObject playerPlane;
    private GameObject player_prefab;
    public GameObject[] enemy_prefab;
    public GameObject local_mask;
    public GameObject local_enemy;
    public float t1;
    private float t2;
    private GameObject[] particleSystems;
    public int level;
    Level_number L;
    PlaneMove plane_move;

    // Start is called before the first frame update
    void Start()
    {
        L = GameObject.FindObjectOfType<Level_number>();
        plane_move = GameObject.FindObjectOfType<PlaneMove>();
        BulletNum = 6;
        level = 1;
        t2 = t1;
        last_kind = 0;
        Random.InitState(System.Guid.NewGuid().GetHashCode());
        playerGod.GetComponent<Player2>().GM = this;
        playerPlane.GetComponent<PlaneMove>().GM = this;
        NumOfMissiles.Add(2);
        NumOfMissiles.Add(2);
        NumOfMissiles.Add(1);
        NumOfMissiles.Add(0);
        NumOfMissiles.Add(0);
        totalNumOfMissiles = 5;
        originNumOfMissiles = 5;
        L = GameObject.FindObjectOfType<Level_number>();
        L.RefreshUI();
    }

    public void LevelUp()
    {
        CleanMissil();
        t2 = t1;
        if (level % 2 == 0)
        {
            NumOfMissiles[3]++;
            NumOfMissiles[4]++;
            originNumOfMissiles += 2;
        }
        else
        {
            NumOfMissiles[0]++;
            NumOfMissiles[1]++;
            NumOfMissiles[2]++;
            originNumOfMissiles += 3;
        }
        level++;
        totalNumOfMissiles = originNumOfMissiles;
        BulletNum = 6;
        L = GameObject.FindObjectOfType<Level_number>();
        L.RefreshUI();
        plane_move = GameObject.FindObjectOfType<PlaneMove>();
        plane_move.FullBlood();
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
        if (totalNumOfMissiles > 0)
        {
            if (start)
            {
                while (NumOfMissiles[last_kind] < 0)
                {
                    last_kind++;
                    if (last_kind >= 5)
                        last_kind = 0;
                }
                t2 = t2 - Time.deltaTime;
                if (t2 <= 0)
                {
                    NewEnemy(last_kind + 1);
                    last_kind++;
                    totalNumOfMissiles--;
                    if (last_kind >= 5)
                        last_kind = 0;
                    t2 = t1;                                       //重複賦值，重複執行
                }
            }
            else
            {
                CleanMissil();
            }
        }
        
        CleanParticle();
    }

    public void NewEnemy(int missile_kind)
    {
        float x = player_prefab.transform.position.x;
        float z = player_prefab.transform.position.z;
        while (x == player_prefab.transform.position.x && z == player_prefab.transform.position.z)
        {
            x = Random.Range(X1, X2);
            z = Random.Range(Z1, Z2);
        }
        //float rMissil = Random.Range(1, 5);

        switch(missile_kind)
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
        local_enemy.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void CleanParticle()
    {
        particleSystems = GameObject.FindGameObjectsWithTag("Expos");
        foreach (GameObject ps in particleSystems)
        {
            if (ps.GetComponent<ParticleSystem>().isStopped)
            {
                PhotonNetwork.Destroy(ps.gameObject);
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
                    PhotonNetwork.Destroy(m.gameObject);
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
