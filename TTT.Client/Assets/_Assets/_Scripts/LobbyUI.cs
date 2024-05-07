using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using TTT.Server.Network_Shared.Packets.Server_Client;
using TMPro;
using UnityEngine.SceneManagement;
using TTT.Server.Network_Shared.Packets.Client_Server;

public class LobbyUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI totalPlayerCount;
    [SerializeField] private Button findOpponentBtn;
    [SerializeField] private TextMeshProUGUI findingStatus;
    [SerializeField] private Button cancleFindOpponentBtn;

    [SerializeField] private RectTransform playerListContainer;
    [SerializeField] private PlayerListCard playerListCard;

    private void Start(){
        
        OnServerStatusRequestHandler.OnServerStatus += OnServerStatusRequestHandler_Refresh;
        RequestServerStatus();
    }
    private void OnDestroy(){
        OnServerStatusRequestHandler.OnServerStatus -= OnServerStatusRequestHandler_Refresh;
    }
    private void OnServerStatusRequestHandler_Refresh(NetOnServerStatus msg){
        Debug.Log("Server Request Recived");
        if(playerListContainer.childCount > 0){
            foreach(RectTransform childRect in playerListContainer){
                Destroy(childRect.gameObject);
            }
        }
        totalPlayerCount.SetText(string.Concat("Total Player : ",msg.topPlayer.Length));
        for (int i = 0; i < msg.topPlayer.Length; i++) {
            PlayerListCard card = Instantiate(playerListCard,playerListContainer);
            var player = msg.topPlayer[i];
            Debug.Log($"{player.userName } isOnline = {player.isOnline}");
            card.SetCardData(player);
        }
    }
    public void TryDisconnect(){
        NetworkClient.instance.Disconnect();
        StartCoroutine(LoadBackToLogin());
    }
    private IEnumerator LoadBackToLogin(){
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);
    }
    private void RequestServerStatus() {
        var msg = new NetServerStatusRequest();
        NetworkClient.instance.SendServer(msg);
    }

    public void FindOpponent(){
        findOpponentBtn.gameObject.SetActive(false);
        cancleFindOpponentBtn.gameObject.SetActive(true);
        findingStatus.SetText("Find Opponent");
        Debug.Log("Finding Opponent");
        // Sending Message to Server to Find Opponent.
        var msg = new NetFindOpponentRequest();
        // Send NetFindOpponent();
        NetworkClient.instance.SendServer(msg);
    }
    public void CanclefindOpponent(){
        Debug.Log("Cancled Finding Opponent");
        findOpponentBtn.gameObject.SetActive(true);
        cancleFindOpponentBtn.gameObject.SetActive(false);
        findingStatus.SetText(string.Empty);

        // Sending Message to Server to cancle Find Opponent.
        var msg = new NetCancleFindOpponentRequest();
        NetworkClient.instance.SendServer(msg);
    }
    // LogoutfindOpponent();
}