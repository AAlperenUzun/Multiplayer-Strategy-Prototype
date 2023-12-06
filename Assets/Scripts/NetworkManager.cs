using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject[] player1Prefabs;
    public Transform[] spawn1Points; 
    public GameObject[] player2Prefabs;
    public Transform[] spawn2Points; 

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogError("connecting..");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.LogError("connected");
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.LogError("spawn");
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < spawn1Points.Length; i++)
            {
                PhotonNetwork.Instantiate(player1Prefabs[0].name, spawn1Points[i].position, Quaternion.identity);
            }
        }
        else
        {
            for (int i = 0; i < spawn2Points.Length; i++)
            {
                PhotonNetwork.Instantiate(player2Prefabs[0].name, spawn2Points[i].position, Quaternion.identity);
            }
        }
    }

}