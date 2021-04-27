using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotoneManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "v1.0";
    private string UserId = "rightway";

    // Start is called before the first frame update
    void Awake()
    {
        // 게임버젼 지정
        PhotonNetwork.GameVersion = gameVersion;
        // 유저명지정
        PhotonNetwork.NickName = UserId;
        // 서버접속
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Server!!");
        PhotonNetwork.JoinRandomRoom(); // 랜덤룸에 접속시도
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"code = {returnCode}, msg = {message}");   
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

        //통신이 가능한 주인공 캐릭터
        PhotonNetwork.Instantiate("Tank", new Vector3(0, 5.0f, 0), Quaternion.identity, 0);
    }

    
}
