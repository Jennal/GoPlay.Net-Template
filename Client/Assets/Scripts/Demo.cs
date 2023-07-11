using System;
using Common.Protocols;
using GoPlay.Services;
using GoPlay.Services.Core.Protocols;
using GoPlay.Services.Core.Transport.NetCoreServer;
using GoPlay.Services.Extension.Frontend;
using Sirenix.OdinInspector;
using UnityEngine;
using GoPlay.Clients;

public class Demo : MonoBehaviour
{
    private Client<NcClient> _client = new Client<NcClient>();

    private void Awake() {
        _client.MainThreadActionRunner = UnityMainThreadActionRunner.Instance;
    }

    private void OnEnable()
    {
        _client.AddListener<PbString>(ProtocolConsts.Push_EchoPush, OnEchoPushAll);
    }

    private void OnDisable()
    {
        _client.RemoveListener<PbString>(ProtocolConsts.Push_EchoPush, OnEchoPushAll);
    }

    private void OnEchoPushAll(PbString obj)
    {
        Debug.Log("[OnEchoPushAll] " + obj.Value);
    }

    [Button(ButtonSizes.Large)]
    private async void Connect()
    {
        if (_client.IsConnected)
        {
            Debug.Log("Already Connected, Disconnecting...");
            await _client.DisconnectAsync();
            Debug.Log("Disconnected!");
        }
        
        Debug.Log("Start Connecting...");
        await _client.Connect("127.0.0.1", 1024, TimeSpan.FromSeconds(1));
        Debug.Log("Connected!");
    }

    [Button(ButtonSizes.Large)]
    private async void Login(string name)
    {
        if (!_client.IsConnected)
        {
            Debug.LogError("Not Connected!");
            return;
        }

        var (status, data) = await _client.Login_Login(new LoginRequest
        {
            Username = name
        });
        Debug.Log($"Login Request: {status.Code} => {data}");
    }
    
    [Button(ButtonSizes.Large)]
    private async void EchoRequest(string text)
    {
        if (!_client.IsConnected)
        {
            Debug.LogError("Not Connected!");
            return;
        }

        var (status, data) = await _client.Echo_Request(new PbString
        {
            Value = text
        });
        Debug.Log($"Echo Request: {status} => {data?.Value}");
    }
    
    [Button(ButtonSizes.Large)]
    private void EchoPushAll(string text)
    {
        if (!_client.IsConnected)
        {
            Debug.LogError("Not Connected!");
            return;
        }

        _client.Echo_PushAll(new PbString
        {
            Value = text
        });
    }
}
