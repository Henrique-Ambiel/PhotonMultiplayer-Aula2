using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variável
    public PhotonView myPhotoView;
    void Start()
    {
        //Pega o componente do player
        myPhotoView = GetComponent<PhotonView>();
    }

    //Envia a mensagem do player
    public void SendChatMessage(string textToSend, string userName)
    {
        myPhotoView.RPC("ReceiveChat", RpcTarget.All, textToSend, userName);
        Debug.Log("Mensagem recebida: " + textToSend);
    }

    //Recebe a mensagem do player
    [PunRPC]
    public void ReceiveChat(string receivedText, string userName, PhotonMessageInfo info)
    {
        Debug.Log("Mensagem recebida: " + receivedText);
        GameObject.Find("PUCCPhoton").GetComponent<PUCCPhoton>().UpdateMessage(receivedText, userName);
    }
}
