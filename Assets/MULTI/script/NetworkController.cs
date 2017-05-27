using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController {

    public enum State
    {
        Disconnected, Disconnecting, Connected, Connecting
    }

    private static NetworkController _instance;
    public static NetworkController Instance
    {
        get{
            if (_instance == null)
            {
                _instance = new NetworkController();
            }
            return _instance;
        }
    }

    public GameManager GameManager;

    private DGTPacket _connection;
    private State _state;

    public NetworkController()
    {
        _connection = new DGTPacket(this);
	}
	
	public void ProcessEvents () {
        _connection.ProcessEvents();
	}

    public void Connect()
    {
        if (_state != State.Disconnected) return;
        _state = State.Connecting;
        _connection.Connect("127.0.0.1", 3000);
    }

    public void Disconnect()
    {
        if (_state != State.Connected) return;
        _state = State.Disconnecting;
        _connection.Disconnect();
    }

    public void OnConnected()
    {
        _state = State.Connected;
    }

    public void OnDisconnected()
    {
        _state = State.Disconnected;
    }

    public void OnFailed()
    {
        _state = State.Disconnected;
    }

    public bool Connected()
    {
        return _connection.Connected && _state == State.Connected;
    }

    public bool ConnectFailed()
    {
        return _connection.Failed;
    }

    //Routing methods, help with sending/receiving packet
    
    public void RequestFindMatch()
    {
        _connection.SendFindMatch();
    }

	public void RequestPlay(int Card)
    {
		_connection.SendPlay(Card);
    }

	public void HandleStartMatch(int matchID, int PlayerNo, int Card1, int Card2, int Card3, int Card4)
    {
		GameManager.OnStartMatch(matchID, PlayerNo, Card1, Card2, Card3, Card4);
    }

	public void HandleOppPlay(int Card)
    {
		GameManager.OnOppSelect(Card);
    }

    public void HandleGameOver(bool win)
    {
        GameManager.OnEndMatch(win);
    }

	public void HandleDrawCard(int Card)
	{
		GameManager.OnDrawCard(Card);
	}
}
