using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TTT.Server.Network_Shared.Models;
using TTT.Server.Network_Shared.Packets.Client_Server;
using TTT.Server.Network_Shared.Packets.Server_Client;
using UnityEngine;
public class GameUI : MonoBehaviour {
    [SerializeField] private TurnUI turnUI;
    [SerializeField] private EndUI endUi;
    [SerializeField] private RectTransform gameBoardRect;

    [SerializeField] private TextMeshProUGUI xUserName,oUserName;
    [SerializeField] private TextMeshProUGUI xUserScore,oUserScore;


    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameGridInteractionUI gameGridInteractionPrefab;

    private Dictionary<int,GameGridInteractionUI> gridInteractionCellDisctionary = new Dictionary<int, GameGridInteractionUI>();
    private int xScore,oScore;
    private void Start(){
        InitHeaders();
        OnMarkedCellHandler.onMarkedCell += UpdateBoard;
        OnNewRoundHandler.onNewround += BoardReset;
        OnSurrenderHandler.onSurrender += HandleSurrender;
        BoardReset();
    }

    private void HandleSurrender(NetOnSurrender surrender) {
        Debug.LogError(surrender.winner + " Won the Game with Surrender");
        StartCoroutine(ShowEndRoundRoutine(surrender.winner,false));
    }

    private void OnDestroy(){
        OnMarkedCellHandler.onMarkedCell -= UpdateBoard;
        OnNewRoundHandler.onNewround -= BoardReset;
        OnSurrenderHandler.onSurrender -= HandleSurrender;
    }
    private void UpdateBoard(NetOnMarkedCell msg){
        gridInteractionCellDisctionary[msg.index].UpdateUI(msg.actor);
        if(msg.markedOutCome != MarkedOutCome.None){

            var draw = msg.markedOutCome == MarkedOutCome.Draw;
            Debug.Log("Showing End Screen For Draw");
            // StopCoroutine(nameof(ShowEndRoundRoutine));
            StartCoroutine(ShowEndRoundRoutine(msg.actor,draw));
            return;
        }
        StopCoroutine(ShowTurn());
        StartCoroutine(ShowTurn());
    }
    private IEnumerator ShowEndRoundRoutine(string winnerId,bool isDraw){
        yield return new WaitForSeconds(.7f);
        var playeType = GameManager.instance.activeGame.GetPlayerType(winnerId);
        if(playeType == MarkType.X){
            xScore++;
            xUserScore.SetText(xScore.ToString());
        }else{
            oScore++;
            oUserScore.SetText(oScore.ToString());
        }
        endUi.ShowWindow(winnerId,isDraw);
    }
    private IEnumerator ShowTurn(){
        yield return new WaitForSeconds(.4f);
        turnUI.RefreshTurn();
    }
    private void InitHeaders(){
        var game = GameManager.instance.activeGame;
        xUserName.SetText("[x] " + game.xUser);
        oUserName.SetText("[o] " + game.oUser);
        xScore = GameManager.instance.activeGame.xUserScore;
        oScore = GameManager.instance.activeGame.oUserScore;
        
        xUserScore.SetText(xScore.ToString());
        oUserScore.SetText(oScore.ToString());
    }

    private void BoardReset(){
        gridInteractionCellDisctionary = new Dictionary<int, GameGridInteractionUI>();
        if(gameBoardRect.childCount > 0){
            foreach(RectTransform childRect in gameBoardRect){
                Destroy(childRect.gameObject);
            }
        }
        for (int i = 0; i < 9; i++) {
            GameGridInteractionUI gridInteractionItem = Instantiate(gameGridInteractionPrefab,gameBoardRect);
            gridInteractionCellDisctionary.Add(i,gridInteractionItem);
            gridInteractionItem.SetData((byte)i);
        }
    }
    public void SurrenderGame(){
        var msg = new NetSurrenderRequest();
        NetworkClient.instance.SendServer(msg);
    }
}