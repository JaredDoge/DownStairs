using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using UnityEngine.UI;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    //遊戲版本的編碼, 可讓 Photon Server 做同款遊戲不同版本的區隔.
    const string GAME_VERSION = "1";

    [Tooltip("遊戲室玩家人數上限. 當遊戲室玩家人數已滿額, 新玩家只能新開遊戲室來進行遊戲.")]
    [SerializeField]
    private byte maxPlayersPerRoom = 4;

    [SerializeField]
    private GameObject joinObj;

    private Join join;

    private PlayState state;

    private string roomName;






    void Awake()
    {
        // 確保所有連線的玩家均載入相同的遊戲場景
        PhotonNetwork.AutomaticallySyncScene = true;
    }


    void Start()
    {

        state = SceneData.Get<PlayState>("state");

        join = joinObj.GetComponent<Join>();

        join.setState(state);

        join.backButton.onClick.AddListener(delegate
        {
            SceneManager.LoadScene(ScenceBuildIndex.SCENCE_START);
        });

        join.roomButton.onClick.AddListener(delegate
            {
                Connect();
            }
        );




    }

    void Update()
    {


    }



    // 與 Photon Cloud 連線
    public void Connect()
    {
        // 檢查是否與 Photon Cloud 連線
        if (PhotonNetwork.IsConnected)
        {

        }
        else
        {
            // 未連線, 嚐試與 Photon Cloud 連線
            PhotonNetwork.GameVersion = GAME_VERSION;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN 呼叫 OnConnectedToMaster(), 已連上 Photon Cloud.");


        // 已連線, 創立或加入一個遊戲室
        switch (state)
        {
            case PlayState.Create:
                // 設定遊戲玩家的名稱
                PhotonNetwork.NickName = join.nameInput.text;
                //創亂數room name
                roomName = Random.Range(0, 100000).ToString().PadLeft(6, '0');


                Debug.Log("創立" + roomName + "遊戲室");
                PhotonNetwork.CreateRoom(roomName, new RoomOptions
                {
                    MaxPlayers = maxPlayersPerRoom
                });

                break;

            case PlayState.Join:
                // 設定遊戲玩家的名稱
                PhotonNetwork.NickName = join.nameInput.text;

                roomName = join.roomIdInput.text;

                Debug.Log("加入" + roomName + "遊戲室");

                PhotonNetwork.JoinRoom(roomName);

                break;


        }



    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN 呼叫 OnDisconnected() {0}.", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN 呼叫 OnJoinRandomFailed(), 隨機加入遊戲室失敗.");

        // 隨機加入遊戲室失敗. 可能原因是 1. 沒有遊戲室 或 2. 有的都滿了.    
        // 好吧, 我們自己開一個遊戲室.
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN 呼叫 OnJoinedRoom(), 已成功進入遊戲室中.");
    }


    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("{0} 進入遊戲室", other.NickName);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("我是 Master Client 嗎? {0}",
                PhotonNetwork.IsMasterClient);
        }
    }
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("{0} 離開遊戲室", other.NickName);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("我是 Master Client 嗎? {0}",
                PhotonNetwork.IsMasterClient);
        }
    }

 

}


