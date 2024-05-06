using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using TMPro;
using TTT.Server.Network_Shared.Packets.Server_Client;

public class PlayerListCard : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI playerName,playerScore;
    [SerializeField] private GameObject onlineIcon,offlineIcon;
    public void SetCardData(PlayerNetDTO playerNet){
        playerName.SetText(playerNet.userName.ToString());
        playerScore.SetText(playerNet.score.ToString());
        onlineIcon.SetActive(playerNet.isOnline);
        offlineIcon.SetActive(!playerNet.isOnline);
    }
}