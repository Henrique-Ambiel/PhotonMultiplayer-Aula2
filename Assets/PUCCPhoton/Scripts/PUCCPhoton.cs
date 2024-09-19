using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;


public class PUCCPhoton : MonoBehaviourPunCallbacks
{
    public Text userList;
    public TMP_InputField input;
    public TextMeshProUGUI _messages;
    public TMP_InputField userNameInput;
    public GameObject myPlayer;
    void Start()
    {
        userList.text = "";
        //atualizar a versao do photon pra separar os
        //jogadores em servidores diferentes
        PhotonNetwork.GameVersion = Application.version;
        PhotonNetwork.NickName = "Ambielzin";
        Debug.Log("[PUCCPhoton] Conectando ao servidor com nickname: " + PhotonNetwork.NickName);

        //conecta no servidor do Photon usando
        //o App Id que está na pasta 
        //Assets/Photon/PhotonUnityNetwork/Resources
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("[PUCCPhoton] Conectou no servidor!");

        //agora que estamos conectados no servidor do Photon
        //precisamos entrar em Lobby para receber a lista de
        //salas ou criar um sala propria
        Debug.Log("[PUCCPhoton] Entrando ao lobby...");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("[PUCCPhoton] Entrou no lobby!");
        //agora que estamos em um lobby
        //podermos entrar numa sala ou criar uma se for necessario
        Debug.Log("[PUCCPhoton] Entrando ou Criando um sala...");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        PhotonNetwork.JoinOrCreateRoom("PUCCRoom", roomOptions, null);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        string log = "";
        foreach(RoomInfo room in roomList)
        {
            log += "ROOM: " + room.Name + "\n";
        }
        Debug.Log("[PUCCPhoton] RoomListUpdate: "+log);
    }

    public override void OnJoinedRoom()
    {
        userList.text += PhotonNetwork.NickName + "\n";
        //esse evento acontece quando EU entro na sala
        Debug.Log("[PUCCPhoton] Entrei na sala!");
        UpdateUserList();
        myPlayer = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        userList.text += newPlayer.NickName + "\n";
        //esse evento acontece quando outros players entram na sala.
        Debug.Log("[PUCCPhoton] Player: " + newPlayer.NickName + " entrou na sala...");
        UpdateUserList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //esse evento acontece quando outros players saem da na sala.
        Debug.Log("[PUCCPhoton] Player: " + otherPlayer.NickName + " saiu da sala!");
        userList.text = userList.text.Replace(otherPlayer.NickName + "\n", "");
        UpdateUserList();
    }

    //Atualiza a lista de players
    public void UpdateUserList()
    {
        userList.text = "";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            userList.text += player.NickName + "\n";
        }
    }

    //Chama o texto que o player irá enviar ao clicar no botão
    public void CallMessageRPC()
    {
        myPlayer.GetComponent<PlayerController>().SendChatMessage(input.text, PhotonNetwork.NickName);
        input.text = "";
    }

    //Exibe a mensagem do player na tela do jogo
    public void UpdateMessage(string message, string userName)
    {
        _messages.text += $"{userName}: {message}\n";
    }
}
