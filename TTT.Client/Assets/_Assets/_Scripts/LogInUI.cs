using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Text;
using Network_Shared.Packets.ClientServer;
using TTT.Client.PacketHandlers;
using TTT.Server.Network_Shared.Packets.Server_Client;
using TTT.PacketHandlers;
public class LogInUI : MonoBehaviour {


    [SerializeField] private string username;
    [SerializeField] private string password;

    [SerializeField] private Button logInBtn;

    [SerializeField] private bool isConnected;
    private void Start() {
        NetworkClient.instance.OnPeerConnectedAction += NetworkClient_OnServerConnectd;
        OnAuthFaildHandler.OnAuthFail += OnErrorAuth;
        // NetworkClient.instance.Init();
    }
    private void OnErrorAuth(NetOnAuthFail netOnAuthFail) {
        Debug.Log("Authentication Failed Due to Password Wrong");
        logInBtn.interactable = false;
        password = string.Empty;
    }
    private void OnDestroy() {
        NetworkClient.instance.OnPeerConnectedAction -= NetworkClient_OnServerConnectd;
        OnAuthFaildHandler.OnAuthFail -= OnErrorAuth;
    }
    private void NetworkClient_OnServerConnectd() {
        isConnected = true;
    }
    public void ValidateUserName(string userName) {
        this.username = userName;
        ValidateBtn();
    }
    public void ValidatePassword(string password) {
        this.password = password;
        ValidateBtn();
    }

    private void ValidateBtn() {
        var usernameRegx = Regex.Match(username, "^[a-zA-Z0-9]+$");
        var passwordRgx = Regex.Match(password, "^[a-zA-Z0-9]+$");
        logInBtn.interactable = !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password) && usernameRegx.Success && passwordRgx.Success;

    }
    public void LogIn()
    {
        logInBtn.interactable = false;
        StopCoroutine(nameof(LoginRoutine));
        StartCoroutine(nameof(LoginRoutine));
    }

    private IEnumerator LoginRoutine() {

        NetworkClient.instance.Connect();

        while (!isConnected) {
            Debug.Log("Waiting to Connect");
            yield return null;
        }
        logInBtn.interactable = true;
        yield return new WaitForSeconds(1f);
        Debug.Log("Sending Auth Request");

        var authRequest = new NetAuthRequest { 
            Username = username,
            Password = password 
        };
        NetworkClient.instance.SendServer(authRequest);
        GameManager.instance.myUserName = username;
    }
}
