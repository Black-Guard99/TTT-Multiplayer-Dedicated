using System.Collections;
using System.Collections.Generic;
using TMPro;
using TTT.Server.Network_Shared.Packets.Server_Client;
using UnityEngine;
public class GameUI : MonoBehaviour {
    [SerializeField] private TurnUI turnUI;
    [SerializeField] private RectTransform gameBoardRect;

    [SerializeField] private TextMeshProUGUI xUserName,oUserName;
    [SerializeField] private TextMeshProUGUI xUserScore,oUserScore;


    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameGridInteractionUI gameGridInteractionPrefab;

    private Dictionary<int,GameGridInteractionUI> gridInteractionCellDisctionary = new Dictionary<int, GameGridInteractionUI>();
    private void Start(){
        InitHeaders();
        OnMarkedCellHandler.onMarkedCell += UpdateBoard;
        BoardReset();
    }
    private void OnDestroy(){
        OnMarkedCellHandler.onMarkedCell -= UpdateBoard;
    }
    private void UpdateBoard(NetOnMarkedCell msg){
        gridInteractionCellDisctionary[msg.index].UpdateUI(msg.actor);
        if(msg.markedOutCome != TTT.Server.Network_Shared.Models.MarkedOutCome.None){

            var draw = msg.markedOutCome == TTT.Server.Network_Shared.Models.MarkedOutCome.Draw;
            Debug.Log("Showing End Screen For Draw");
            return;
        }
        StopCoroutine(ShowTurn());
        StartCoroutine(ShowTurn());
    }
    private IEnumerator ShowTurn(){
        yield return new WaitForSeconds(.4f);
        turnUI.RefreshTurn();
    }
    private void InitHeaders(){
        var game = GameManager.instance.activeGame;
        xUserName.SetText("[x] " + game.xUser);
        oUserName.SetText("[o] " + game.oUser);
    }

    private void BoardReset(){
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
}