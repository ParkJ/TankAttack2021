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
    public TMP_InputField roomNameText;

    // 룸 목록 저장하기 위한 딕셔러니
    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();
    // 룸을 표시할 프리팹
    public GameObject roomPrefab;
    //Room 프리팹이 차일드화 시킬 부모 객체
    public Transform scrollContent;

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

        roomNameText.text = $"ROOM_{Random.Range(0, 100):000}";

        //룸 생성
        PhotonNetwork.CreateRoom(roomNameText.text, ro);

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


    #region UI_BUTTON_CALLBACK

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

    public void OnMakeRoomClick()
    {
         //룸 옵션설정
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 30;

        if(string.IsNullOrEmpty(roomNameText.text))
        {
            roomNameText.text = $"ROOM_{Random.Range(0, 100):000}";
        }
        //룸 생성
        PhotonNetwork.CreateRoom(roomNameText.text, ro);
    }

    // 룸 목록 수신 - 룸 목록이 변경(갱신)될 때마다 호출되는 콜백함수
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject tempRoom = null;

        foreach (var room in roomList)
        {
            Debug.Log($"room name = {room.Name}, ({room.PlayerCount}/{room.MaxPlayers})");
            
            //룸 삭제된 경우 => 딕셔너리에 삭제, RoomItem 프리팹 삭제
            if(room.RemovedFromList == true)
            {
                //딕셔너리에 삭제, roomItem프리팹 삭제
                roomDict.TryGetValue(room.Name, out tempRoom);
                //RooomItem 프리팹 삭제
                Destroy(tempRoom);
                //딕셔너리에서 데이터 삭제
                roomDict.Remove(room.Name);
            }
            else //룸정보가 갱신(변경)
            {
                //처음 생성된 경우 딕셔너리에 데이터 추가, RoomItem 생성
                if(roomDict.ContainsKey(room.Name) == false)
                {
                    GameObject _room = Instantiate(roomPrefab, scrollContent);
                    //룸정보표시
                    _room.GetComponent<RoomData>().RoomInfo = room;
                    //딕셔너리에 데이터 추가
                    roomDict.Add(room.Name, _room);
                }
                //처음이 아닐 경우 정보를 갱신
                else
                {
                    roomDict.TryGetValue(room.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().RoomInfo = room;

                }
            }

        }
    }
    

    #endregion
    
}
