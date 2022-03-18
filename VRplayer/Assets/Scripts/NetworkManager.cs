using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    //The instance of the current NetworkManager
    public static NetworkManager Instance;

    /// <summary>Connect automatically? If false you can set this to true later on or call ConnectUsingSettings in your own scripts.</summary>
    public bool AutoConnect = true;

    /// <summary>Used as PhotonNetwork.GameVersion.</summary>
    public byte Version = 1;

    //The room name
    public string roomName = "IDVR";

    //public GameObject camera;
    //The prefab for the player's avatar
    public GameObject avatar_prefab;

    //The local player avatar
    public GameObject local_player;

    public GameObject player_mask;

    public GameObject local_mask;

    public static List<Player> playerList = new List<Player>();
    //If the player is the master client in this room
    //it would be the server as well
    //public GameObject Server;

    /*void Awake()
    {
        Instance = this;
    }

    void OnDestroy()
    {
        Instance = null;
    }*/

    public void Start()
    {
        Instance = this;
        if (this.AutoConnect)
        {
            this.ConnectNow();
        }

    }

    public void ConnectNow()
    {
        Debug.Log("ConnectAndJoinRandom.ConnectNow() will now call: PhotonNetwork.ConnectUsingSettings().");
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = this.Version + "." + SceneManagerHelper.ActiveSceneBuildIndex;
        print(PhotonNetwork.GameVersion);
    }

    // below, we implement some callbacks of the Photon Realtime API.
    // Being a MonoBehaviourPunCallbacks means, we can override the few methods which are needed here.

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby(). This client is connected and does get a room-list, which gets stored as PhotonNetwork.GetRoomList(). This script now calls: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRoomFailed() was called by PUN. The room is not available, so we create one. Calling: PhotonNetwork.CreateRoom(roomName, new RoomOptions() { CleanupCacheOnLeave = true });");
        PhotonNetwork.CreateRoom(roomName, new RoomOptions() { CleanupCacheOnLeave = true });
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 4 }, null);
    }

    // the following methods are implemented to give you some context. re-implement them as needed.
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected(" + cause + ")");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running.");
        local_player = PhotonNetwork.Instantiate(avatar_prefab.name, new Vector3(4532f, 1050f, 2982f), Quaternion.identity);
        //local_mask = Instantiate(player_mask, new Vector3(4532f, 1050f, 2982f), Quaternion.identity);
        //Check if the player is the master client
        if (PhotonNetwork.IsMasterClient)
        {
            //If he/she is, then activate the server gameobject
            //Which means that he/she will play as the server as well
            //Server.SetActive(true);
        }
    }

    void Update()
    {
        //add the non-local player to playerList when he joins
        /*while (PhotonNetwork.playerList.Length < 2) {
 
            Debug.Log ("Update Count of Players "+playerList.Count);
            Debug.Log ("1");
            Player otherPlayer = GameObject.Find ("Player").GetComponent<Player> ();
 
            otherPlayer.ID = pp.ID;
 
            playerList.Add (otherPlayer);
        }*/

    }

}