using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("Room Info")]
    public TMP_Text roomNameText;
    public TMP_Text connectInfoText;
    public TMP_Text messageText;
    
    [Header("Chattin UI")]
    public TMP_Text chatListText;
    public TMP_InputField msgIF; // message Info

    public Button exitBotton;

    private PhotonView pv;

    public static GameManager instatnce = null;


    void Awake()
    {
        instatnce = this;
        Vector3 pos = new Vector3(Random.Range(-150.0f, 150.0f), 5.0f, Random.Range(-150.0f, 150.0f));
        PhotonNetwork.Instantiate("Tank", new Vector3(0, 5.0f, 0), Quaternion.identity, 0);
    }

    void Start()
    {
        pv = GetComponent<PhotonView>();
        // pv = photonView; //이것도 가능.

        SetRoomInfo();
    }

    void SetRoomInfo()
    {
        Room currentRoom = PhotonNetwork.CurrentRoom;
        roomNameText.text = currentRoom.Name;
        connectInfoText.text = $"{currentRoom.PlayerCount}/{currentRoom.MaxPlayers})";
    }

    public void OnExitClick()
    {
        PhotonNetwork.LeaveRoom();
    }

    //CleanUp 끝난 후 호출되는 콜백
    public override void OnLeftRoom()
    {
        //Lobby 씨능로 되돌아 가기..
        SceneManager.LoadScene("Lobby");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#00ff00>{newPlayer.NickName}</color> is joined room";
        messageText.text += msg;

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#ff0000>{otherPlayer.NickName}</color> is left room";
        messageText.text += msg;
    }

    public void OnsendClick()
    {
        string _msg = $"<color=#00ff00>[{PhotonNetwork.NickName}]</color>{msgIF.text}";
        pv.RPC("SendChatMessage", RpcTarget.AllBufferedViaServer, _msg);
    }
    
    [PunRPC]
    void SendChatMessage(string msg)
    {
        chatListText.text += $"{msg}\n)";
    }

}
