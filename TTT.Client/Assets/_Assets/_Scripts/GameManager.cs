using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using JetBrains.Annotations;
using System;
using Baracuda.Monitoring;
using TTT.Server.Network_Shared.Models;
public class GameManager : MonoBehaviour {
    public static GameManager instance { get; private set; }

    [Monitor] public Game activeGame;
    [Monitor] public string myUserName{get;set;}
    [Monitor] public string opponentUserName{get;private set;}
    [Monitor] public MarkType myType{get;set;}
    [Monitor] public MarkType opponentType{get;set;}
    
    public bool IsMyTurn{
        get{
            if(activeGame == null){
                return false;
            }
            if(activeGame.currentUser != myUserName){
                return false;
            }
            return true;
        }
    }
    public bool inputsEnable{get;set;}
    private void Awake() {
        Monitor.StartMonitoring(this);
        if(instance != null && instance != this) {
            Destroy(instance);
        }else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }
    private void OnDestroy(){
        Monitor.StopMonitoring(this);
    }
    
    public void RegisterGame(Guid gameId , string xUser,string oUser){
        activeGame = new Game{
            id = gameId,
            xUser = xUser,
            oUser = oUser,
            startTime = DateTime.UtcNow,
            currentUser = xUser
        };

        if(myUserName == xUser){
            myType = MarkType.X;
            opponentUserName = oUser;
            opponentType = MarkType.O;
        }else{
            myType = MarkType.O;
            opponentUserName = xUser;
            opponentType = MarkType.X;
        }
        inputsEnable = true;
    }

}
public class Game{
    public Guid? id;
    public string xUser;
    public string oUser;
    public string currentUser;
    public DateTime startTime;
    public DateTime endTime;
    public void SwithCurrentPlayer() {
        currentUser = GetOpponent(currentUser);
    }

    public MarkType GetPlayerType(string actor) {
        if(actor == xUser){
            return MarkType.X;
        }else{
            return MarkType.O;
        }
    }

    private string GetOpponent(string currentUser) {
        if(currentUser == xUser){
            return oUser;
        }else{
            return xUser;
        }
    }

    public void ResetGame()
    {
        currentUser = xUser;
    }
}