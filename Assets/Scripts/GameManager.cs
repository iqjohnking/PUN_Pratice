using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Tanks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{

    string gameVersion = "1.0";
    public static GameManager instance;
    public static GameObject localPlayer;

    private GameObject defaultSpawnPoint;

    //抓取重生點資料
    public static List<GameObject> GetAllObjectsOfTypeInScene<T>()
    {
        var objectsInScene = new List<GameObject>();
        foreach (var go in (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject)))
        {
            //hideFlags，不能編輯的東西擋掉(如系統內建的gameObject)
            if (go.hideFlags == HideFlags.NotEditable ||
                go.hideFlags == HideFlags.HideAndDontSave)
                continue;
            if (go.GetComponent<T>() != null)
                objectsInScene.Add(go);
        }
        return objectsInScene;
    }
    


    void Awake()
    {
        if (instance != null)
        {
            //不能有重複的GM
            Debug.LogErrorFormat(gameObject, $"Multiple instances of {GetType().Name} is not allow");
            DestroyImmediate(gameObject);
            return;
        }
        PhotonNetwork.AutomaticallySyncScene = true;
        //切換scene時不刪除GM，會繼續存在
        DontDestroyOnLoad(gameObject);
        defaultSpawnPoint = new GameObject("Default SpawnPoint");
        defaultSpawnPoint.transform.position = new Vector3(0, 0, 0);
        //SetParent，true則保持世界座標的，false則跟隨局部座標(母物件)的
        defaultSpawnPoint.transform.SetParent(transform, false);
        return;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Created room!!");
            PhotonNetwork.LoadLevel("GameScene");
        }
        else
        {
            Debug.Log("Joined room!!");
        }
    }

    //ver0.6隨機重生，這裡有改
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!PhotonNetwork.InRoom)
        {
            return;
        }
        var spawnPoint = GetRandomSpawnPoint();
        localPlayer = PhotonNetwork.Instantiate(
          "TankPlayer",
          spawnPoint.position,
          spawnPoint.rotation,
          0);
        Debug.Log("Player Instance ID: " + localPlayer.GetInstanceID());
    }

    public void JoinGameRoom()
    {
        var options = new RoomOptions
        {
            MaxPlayers = 0
        };


        PhotonNetwork.JoinOrCreateRoom("Gensoukyou", options, null);
    }
    private Transform GetRandomSpawnPoint()
    {
        var spawnPoints = GetAllObjectsOfTypeInScene<SpawnPoint>();
        return spawnPoints.Count == 0
        ? defaultSpawnPoint.transform
        : spawnPoints[Random.Range(0, spawnPoints.Count)].transform;
    }
}



