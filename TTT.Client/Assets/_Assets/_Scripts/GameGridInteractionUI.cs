using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using TTT.Server.Network_Shared.Packets.Client_Server;
using TTT.Server.Network_Shared.Models;

public class GameGridInteractionUI : MonoBehaviour, IPointerDownHandler {
    [SerializeField] private Image cellIcon;
    [SerializeField] private Sprite xSprite,oSprite;
    
    private byte index;
    private byte row;
    private byte col;
    private void Start(){
        Color imageColor = cellIcon.color;
        imageColor.a = 0;
        cellIcon.color = imageColor;
    }
    public void OnPointerDown(PointerEventData eventData) {
        if(!GameManager.instance.IsMyTurn || !GameManager.instance.inputsEnable){
            Debug.LogError("Not your Turn");
            return;
        }
        cellIcon.raycastTarget = false;
        GameManager.instance.inputsEnable = false;
        Debug.Log("Sending Marked Checll Request To Server");
        var msg = new NetMarkCellRequest{
            index = index,
        };
        NetworkClient.instance.SendServer(msg);
    }

    public void SetData(byte i) {
        index = i;
        row = (byte)(index / 3);
        col = (byte)(index % 3);
    }

    public void UpdateUI(string actor) {
        var actorType = GameManager.instance.activeGame.GetPlayerType(actor);
        if(actorType == MarkType.X){
            cellIcon.sprite = xSprite;
        }else{
            cellIcon.sprite = oSprite;
        }
        cellIcon.raycastTarget = false;
        Color imageColor = cellIcon.color;
        imageColor.a = 1;
        cellIcon.color = imageColor;
    }
}