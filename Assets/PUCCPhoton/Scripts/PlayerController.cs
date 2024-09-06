using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PhotonView myPhotoView;
    void Start()
    {
        myPhotoView = GetComponent<PhotonView>();
        SendChatMessage("Mensagem teste");
    }

    [PunRPC]
    public void ChatMessage(string receivedText, PhotonMessageInfo info)
    {
        Debug.Log("Mensagem recebida: " + receivedText);
    }

    public void SendChatMessage(string textToSend)
    {
        myPhotoView.RPC("ChatMessage", RpcTarget.All, textToSend);
        Debug.Log("Mensagem recebida: " + textToSend);
    }
}
