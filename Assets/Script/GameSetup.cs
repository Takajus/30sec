//using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour
{
    /*public static GameSetup Instance;

    private void OnEnable()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void DisconnectPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        //PhotonNetwork.Disconnect();
        PhotonNetwork.LeaveRoom();
        //while (PhotonNetwork.IsConnected)
        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene(0);
    }*/
}
