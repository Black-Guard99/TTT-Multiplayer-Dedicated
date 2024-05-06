using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TTT.Server.Network_Shared.Models;
using TTT.Server.Network_Shared.Packets.Server_Client;
public class WinLineVisuel : MonoBehaviour {
    [SerializeField] private Image lineImage;
    private Dictionary<WinLineType,LineConfig> lineDataDictionary = new Dictionary<WinLineType, LineConfig>();
    private void Start(){
        lineImage.fillAmount = 0f;
        lineDataDictionary = LineDictionary();
        OnMarkedCellHandler.onMarkedCell += HandleOnMarkedCell;
        OnNewRoundHandler.onNewround += ResetLine;
        ResetLine();

    }
    private void OnDestroy(){
        OnMarkedCellHandler.onMarkedCell -= HandleOnMarkedCell;
        OnNewRoundHandler.onNewround -= ResetLine;
    }

    private void ResetLine() {
        lineImage.fillAmount = 0;
        lineImage.enabled = false;
        SetLineVisualData(lineDataDictionary[WinLineType.colMid]);
    }

    private void HandleOnMarkedCell(NetOnMarkedCell msg) {
        if(msg.markedOutCome == MarkedOutCome.Win){
            lineImage.enabled = true;
            var config = lineDataDictionary[msg.winLineType];
            SetLineVisualData(config,msg.actor);
        }
    }

    private Dictionary<WinLineType,LineConfig> LineDictionary(){
        return new Dictionary<WinLineType, LineConfig>{
            { WinLineType.None, new LineConfig() },
            { WinLineType.Diagnoal, new LineConfig() { height = 600f,  zrotaion = 45f, X = 0f, Y = 0f } },
            { WinLineType.AntiDiaognal, new LineConfig() { height = 600f,  zrotaion = -45f, X = 0f, Y = 0f } },
            { WinLineType.ColLeft, new LineConfig() { height = 600f,  zrotaion = 0f, X = -94f, Y = 0f } },
            { WinLineType.colMid, new LineConfig() {  height = 600f,  zrotaion = 0f, X = 0f, Y = 0f } },
            { WinLineType.ColRight, new LineConfig() {  height = 600f,  zrotaion = 0f, X = 94f, Y = 0f } },
            { WinLineType.RowTop, new LineConfig() {  height = 600f,  zrotaion = 90f, X = 0, Y = 200f } },
            { WinLineType.RowMid, new LineConfig() {  height = 600f,  zrotaion = 90f, X = 0, Y = 0f } },
            { WinLineType.RowBottom, new LineConfig() {  height = 200f,  zrotaion = 90f, X = 0, Y = -200f } },
        };
    }
    public void SetLineVisualData(LineConfig lineConfig,string actorId = default){
        Color lineColor = Color.yellow;
        if(!string.IsNullOrEmpty(actorId)){
            var type = GameManager.instance.activeGame.GetPlayerType(actorId);
            lineColor = type == MarkType.X ? Color.yellow : Color.cyan;
        }
        lineImage.color = lineColor;

        lineImage.rectTransform.eulerAngles = Vector3.forward * lineConfig.zrotaion;
        lineImage.rectTransform.sizeDelta = new Vector2(20,lineConfig.height);
        lineImage.rectTransform.anchoredPosition = new Vector2(lineConfig.X,lineConfig.Y);
        StopCoroutine(nameof(AnimateLine));
        StartCoroutine(nameof(AnimateLine));
    }
    private IEnumerator AnimateLine(){
        lineImage.fillAmount = 0f;
        yield return new WaitForSeconds(.5f);
        float fillAmount = 0f;
        while(fillAmount < 1){
            fillAmount += Time.deltaTime * 5f;
            lineImage.fillAmount = fillAmount;
            yield return null;
        }
    }
    public class LineConfig{
        public float X,Y;
        public float height;
        public float zrotaion;

    }
}