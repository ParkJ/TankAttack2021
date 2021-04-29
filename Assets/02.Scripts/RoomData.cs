using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomData : MonoBehaviour
{
    private RoomInfo _roomInfo;
    private TMP_Text roomInfoText;

    public RoomInfo RoomInfo //프로퍼티
    {
        get
        {
            return _roomInfo;
        }
        set
        {
            _roomInfo = value;
            // ex) room_03 (12/30)
            roomInfoText.text = $"{_roomInfo.Name} ({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})";

            //버튼의 클릭 이벤트에 함수 연결
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener( ()=>OnEnterRoom(_roomInfo.Name));
            // GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate() { OnEnterRoom(_roomInfo.Name);}); //델리게이트 이용하는 문법
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        roomInfoText = GetComponentInChildren<TMP_Text>();
    }

    void OnEnterRoom(string roomName)
    {
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 30;

        PhotonNetwork.JoinOrCreateRoom(roomName, ro , TypedLobby.Default);
    }

}
