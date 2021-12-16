using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
//using Photon.Realtime;

public class RoomManager : MonoBehaviour //MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    /*public static RoomManager Instance { get; private set; }
    private PhotonView PV;

    private GameObject gopv;
    private bool check = true;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;

        AddPhotonView();
    }

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        

        if(SceneManager.GetActiveScene().handle == 0 && check)
        {
            AddPhotonView();
            check = false;
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        //PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    public override void OnDisable()
    {
        base.OnDisable();
        //PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.buildIndex == 1) // Instantiate PlayerManager Prefab
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
        Debug.Log("Scene Loaded");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + " has left the game");
    }

    public void AddPhotonView()
    {
        gopv = new GameObject("gopv");
        gopv.AddComponent<PhotonView>();
        gopv.transform.parent = transform;
    }*/

}
