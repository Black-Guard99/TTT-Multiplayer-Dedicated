using System;
using TMPro;
using TTT.Server.Network_Shared.Packets.Client_Server;
using TTT.Server.Network_Shared.Packets.Server_Client;
using UnityEngine;
using UnityEngine.UI;

public class EndUI : MonoBehaviour {
    [SerializeField] private Color drawColor;
    [SerializeField] private GameObject windowView;
    [SerializeField] private GameObject requestRematchBtn,acceptRematchBtn;
    [SerializeField] private Image headerBgImage;
    [SerializeField] private TextMeshProUGUI headerResultText,requestingInfoText;
    private void Start(){
        OnPlayAgainHandler.onPlayAgain += HandleOnPlayAgainRequest;
        OnNewRoundHandler.onNewround += RestUI;
    }

    private void RestUI() {
        headerBgImage.color = Color.white;
        windowView.SetActive(false);
    }

    private void OnDestroy(){
        OnPlayAgainHandler.onPlayAgain -= HandleOnPlayAgainRequest;
        OnNewRoundHandler.onNewround -= RestUI;
    }

    private void HandleOnPlayAgainRequest(OnNetPlayAgain again) {
        requestingInfoText.SetText("Opponent Wants To Play Again");
        acceptRematchBtn.SetActive(true);
        requestRematchBtn.SetActive(false);
    }

    public void ShowWindow(string winnerId,bool isDraw) {
        windowView.SetActive(true);
        requestRematchBtn.SetActive(true);
        acceptRematchBtn.SetActive(false);
        if(isDraw){
            headerBgImage.color = drawColor;
            headerResultText.SetText("Game Draw");
            return;
        }
        bool isMine = GameManager.instance.myUserName == winnerId;
        if(isMine){
            headerBgImage.color = Color.green;
            headerResultText.SetText("YOU WIN");
        }else{
            headerBgImage.color = Color.red;
            headerResultText.SetText("YOU LOOSE");
        }
    }
    public void RequestRematch(){
        requestRematchBtn.SetActive(false);
        acceptRematchBtn.SetActive(false);
        // Send Requst to server for Rematch.
        var msg = new NetPlayAgainRequest();
        NetworkClient.instance.SendServer(msg);

    }
    public void AcceptRematch(){
        Debug.Log("Accepted Rematch");
        requestRematchBtn.SetActive(false);
        acceptRematchBtn.SetActive(false);
        var msg = new NetAcceptPlayAgainRequest();
        NetworkClient.instance.SendServer(msg);
    }
}