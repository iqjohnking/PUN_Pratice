using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    string gameVersion = "1.0";


    void Awake()
    {
        if (instance != null)
        {
            //不能有重複的GM出現
            Debug.LogErrorFormat(gameObject, $"Multiple instances of {GetType().Name} is not allow");
            DestroyImmediate(gameObject);
            return;
        }
        PhotonNetwork.AutomaticallySyncScene = true;
        //切換scene時不刪除GM
        DontDestroyOnLoad(gameObject);
        return;

    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
    }
    public override void OnConnected()
    {
        Debug.Log("PUN Connected");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Connected to Master");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat($"PUN Disconnected was called by PUN with reason {cause}");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room!!");
    }
    public void JoinGameRoom()
    {
        var options = new RoomOptions
        {
            MaxPlayers = 0
        };
        PhotonNetwork.JoinOrCreateRoom("Gensoukyou", options, null);
    }


}
