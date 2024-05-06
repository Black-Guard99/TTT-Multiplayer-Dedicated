using TMPro;
using UnityEngine;
using TTT.Server.Network_Shared.Models;
public class TurnUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI turnInfoText;
    private void Start(){
        RefreshTurn();
    }
    public void RefreshTurn(){
        var myColor = GameManager.instance.myType == MarkType.X ? "blue":"red";
        var opponentColor = GameManager.instance.opponentType == MarkType.X ? "blue":"red";
        if(GameManager.instance.IsMyTurn){
            turnInfoText.SetText($"it's <color={myColor}> Your's </color> Turn");
        }else{
            turnInfoText.SetText($"it's <color={opponentColor}> Oppnent's </color> Turn");
        }
    }
}