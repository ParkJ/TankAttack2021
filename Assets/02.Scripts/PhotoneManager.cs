using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class PhotoneManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "v1.0";
    private string userId = "rightway";

    public TMP_InputField userIdText;
    public TMP_InputField roomName;

    // Start is called before the first frame update
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        // 게임버젼 지정
        PhotonNetwork.GameVersion = gameVersion;
        // 유저명지정
        PhotonNetwork.NickName = userId;
        // 서버접속
        PhotonNetwork.ConnectUsingSettings();
    }

    void Start()
    {
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(0, 100):00}"); //"USER_Random.Range(0, 100):00"
        userIdText.text = userId;
        PhotonNetwork.NickName = userId;
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Server!!");
        // PhotonNetwork.JoinRandomRoom(); // 랜덤룸에 접속시도

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("joined lobby!!!");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"code = {returnCode}, msg = {message}");   

        //룸 옵션설정
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 30;

        //룸 생성
        PhotonNetwork.CreateRoom("My Room");

    }

    //룸생성완료 콜백함수
    public override void OnCreatedRoom()
    {
        Debug.Log("방생성 완료");
    }

    //룸에 입장했을 때 호출되는 콜백함수
    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장완료");
        Debug.Log(PhotonNetwork.CurrentRoom.Name);

        // PhotonNetwork.IsMessageQueueRunning = false;

        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("BattleField");
        }
        // //통신이 가능한 주인공 캐릭터
       
    }

    public void OnLoginClick()
    {
        if(string.IsNullOrEmpty(userIdText.text))
        {
            userId = $"USER_Random.Range(0, 100):00";
            userIdText.text = userId;
        }

        PlayerPrefs.SetString("USER_ID", userIdText.text);
        PhotonNetwork.NickName = userIdText.text;
        PhotonNetwork.JoinRandomRoom();
    }
    
}
