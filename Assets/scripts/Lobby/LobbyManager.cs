using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    //遊戲版本的編碼, 可讓 Photon Server 做同款遊戲不同版本的區隔.
    const string GAME_VERSION = "1";

    [Tooltip("遊戲室玩家人數上限. 當遊戲室玩家人數已滿額, 新玩家只能新開遊戲室來進行遊戲.")]
    [SerializeField]
    private byte maxPlayersPerRoom = 4;

    [SerializeField]
    private GameObject joinObj;

    [SerializeField]
    private GameObject roomUIObj;

    private Join join;

    private RoomUI roomUI;


    private PlayState state;

    private string roomName;

    private bool connectRequest = false;



    void RoomUI()
    {
        joinObj.SetActive(false);
        roomUIObj.SetActive(true);
    }

    void JoinUI()
    {
        joinObj.SetActive(true);
        roomUIObj.SetActive(false);
    }


    void Awake()
    {
        // 確保所有連線的玩家均載入相同的遊戲場景
        PhotonNetwork.AutomaticallySyncScene = true;
    }


    void Start()
    {

        state = SceneData.Get<PlayState>("state");


        roomUI = roomUIObj.GetComponent<RoomUI>();

        join = joinObj.GetComponent<Join>();

        join.setState(state);

        join.backButton.onClick.AddListener(this.JoinBack);

        join.roomButton.onClick.AddListener(this.JoinRoom);

        roomUI.backBtn.onClick.AddListener(this.RoomBack);

        roomUI.startBtn.onClick.AddListener(this.RoomStart);

        JoinUI();

        ///test
        JoinRoom();

    }

    void JoinBack()
    {

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        SceneManager.LoadScene(ScenceBuildIndex.SCENCE_START);
    }

    void JoinRoom()
    {
        connectRequest = true;
        Connect();

    }

    void RoomBack()
    {
        JoinUI();
        PhotonNetwork.LeaveRoom();

    }

    void RoomStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //Master Client 才載scene
            PhotonNetwork.LoadLevel(ScenceBuildIndex.SCENCE_2_PLAYER);
        }
    }



    private void OnDestroy()
    {

        join.backButton.onClick.RemoveListener(this.JoinBack);

        join.roomButton.onClick.RemoveListener(this.JoinRoom);

        roomUI.backBtn.onClick.RemoveListener(this.RoomBack);

        roomUI.startBtn.onClick.RemoveListener(this.RoomStart);
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
            OnConnectedToMaster();
        }
        else
        {
            // 未連線, 嚐試與 Photon Cloud 連線
            PhotonNetwork.GameVersion = GAME_VERSION;
            //https://doc.photonengine.com/en-us/pun/current/connection-and-authentication/regions#best_region_considerations
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "asia";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {

        //房主才顯示開始按鈕
        roomUI.startBtn.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN 呼叫 OnConnectedToMaster(), 已連上 Photon Cloud.");

        if (!connectRequest) return;

        // 已連線, 創立或加入一個遊戲室
        switch (state)
        {
            case PlayState.Create:
                // 設定遊戲玩家的名稱
                PhotonNetwork.NickName = "房長";
                
                    //join.nameInput.text;

                //創亂數room name

                roomName = "77777";
                
                //Random.Range(0, 100000).ToString().PadLeft(6, '0');


                Debug.Log("創立" + roomName + "遊戲室");
                PhotonNetwork.CreateRoom(roomName, new RoomOptions
                {
                    MaxPlayers = maxPlayersPerRoom
                }, TypedLobby.Default);



                break;

            case PlayState.Join:
                // 設定遊戲玩家的名稱
                PhotonNetwork.NickName = "加入的人";
                
                  //join.nameInput.text;

                roomName ="77777";
                
                // join.roomIdInput.text;

                Debug.Log("加入" + roomName + "遊戲室");

                PhotonNetwork.JoinRoom(roomName);
                break;


        }


        connectRequest = false;

    }
    public override void OnDisconnected(DisconnectCause cause)
    {

        connectRequest = false;
        Debug.LogWarningFormat("PUN 呼叫 OnDisconnected() {0}.", cause);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("PUN 呼叫 OnJoinRoomFailed(), 加入遊戲室失敗. " + message);

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN 呼叫 OnJoinedRoom(), 已成功進入遊戲室中.");
        roomUI.roomId.text = string.Format("RoomID : " + roomName);

        RoomUI();

        ReloadPlayerList();

        UpdatePlayerCounter();

        //房主才顯示開始按鈕
        roomUI.startBtn.gameObject.SetActive(PhotonNetwork.IsMasterClient);

    }



    private void ReloadPlayerList()
    {
        roomUI.ReloadData(PhotonNetwork.PlayerList);
    }

    private void UpdatePlayerCounter()
    {
        roomUI.PlayerCounter.text = string.Format("人數:{0}/{1}",
               PhotonNetwork.CurrentRoom.PlayerCount,
               PhotonNetwork.CurrentRoom.MaxPlayers);
    }


    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("{0} 進入遊戲室", other.NickName);
        UpdatePlayerCounter();
        ReloadPlayerList();

    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("{0} 離開遊戲室", other.NickName);
        UpdatePlayerCounter();
        ReloadPlayerList();
    }



}


