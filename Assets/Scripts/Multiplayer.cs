using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class Multiplayer : MonoBehaviour
{
    static readonly List<Partner> _partners = new List<Partner>();
    static WebSocket _client;
    Transform _player;
    Transform _playerLeftArm;
    Transform _playerRightArm;
    float _connectTimer = 0;
    string _name = "?";
    string _scene = "?";

    void Start()
    {
        string[] env = Environment.GetEnvironmentVariable("Benzie").Split('|');
        _client = new WebSocket($"{env[1]}/benzie");
        _name = env[0];

        _player = GameObject.Find("Player").transform;
        _playerLeftArm = _player.Find("PlayerBody/PlayerLeftArm");
        _playerRightArm = _player.Find("PlayerBody/PlayerRightArm");

        _scene = SceneManager.GetActiveScene().name;
        _client.OnMessage += Client_OnMessage;
        _client.ConnectAsync();
    }

    void Update()
    {
        _connectTimer += Time.deltaTime;
        try
        {
            _client.Send($"{_name}|{_scene}|{(100 * _player.position.x):0}|{(100 * _player.position.y):0}|{(100 * _player.position.z):0}|{_player.localRotation.eulerAngles.y:0}|{_playerLeftArm.localRotation.eulerAngles.z:0}|{_playerRightArm.localRotation.eulerAngles.z:0}");
        }
        catch
        {
            if (_connectTimer >= 10)
            {
                _connectTimer = 0;
                _client.ConnectAsync();
            }
        }

        foreach (Partner partner in _partners)
        {
            partner.Update();
        }
    }

    void OnDestroy()
    {
        _client.CloseAsync();
    }

    void Client_OnMessage(object sender, MessageEventArgs e)
    {
        string[] data = e.Data.Split('|');
        if (data.Length == 8 && data[0] != _name && data[1] == _scene)
        {
            Partner partner = _partners.Find(x => x.Name == data[0]);
            if (partner == null)
            {
                partner = new Partner(data[0]);
                _partners.Add(partner);
            }

            partner.Data = data;
        }
    }

    private class Partner
    {
        Transform _partner;
        Transform _partnerLeftArm;
        Transform _partnerRightArm;

        internal Partner(string name)
        {
            Name = name;
        }

        internal string Name { get; }

        internal string[] Data { get; set; }

        internal void Update()
        {
            if (_partner == null)
            {
                GameObject template = Resources.Load("TPlayer") as GameObject;
                _partner = Instantiate(template).transform;
                _partnerLeftArm = _partner.Find("PlayerBody/PlayerLeftArm");
                _partnerRightArm = _partner.Find("PlayerBody/PlayerRightArm");
            }

            _partner.position = new Vector3(float.Parse(Data[2]) / 100, float.Parse(Data[3]) / 100, float.Parse(Data[4]) / 100);
            _partner.localRotation = Quaternion.Euler(0, float.Parse(Data[5]), 0);
            _partnerLeftArm.localRotation = Quaternion.Euler(0, 0, float.Parse(Data[6]));
            _partnerRightArm.localRotation = Quaternion.Euler(0, 0, float.Parse(Data[7]));
        }
    }
}
