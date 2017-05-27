using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface to DGT-net
class DGTPacket : PacketManager
{
    private NetworkController _controller;

    private enum PacketId
    {
        CS_FIND = 10001,
        CS_PLAY = 10002,

        SC_START = 20001,
        SC_PLAY = 20002,
        SC_END = 20003,
		SC_DRAW = 20004
    }

    public DGTPacket(NetworkController controller) : base()
    {
        _controller = controller;

        _Mapper[(int)PacketId.SC_START] = RecvStartMatch;
        _Mapper[(int)PacketId.SC_PLAY] = RecvOppPlay;
        _Mapper[(int)PacketId.SC_END] = RecvGameover;
		_Mapper[(int)PacketId.SC_DRAW] = RecvDrawCard;
    }

    protected override void OnConnected()
    {
        _controller.OnConnected();
    }

    protected override void OnDisconnected()
    {
        _controller.OnDisconnected();
    }

    protected override void OnFailed()
    {
        _controller.OnFailed();
    }

    //Send and receive

    public void SendFindMatch()
    {
        BeginSend((int)PacketId.CS_FIND);
        EndSend();
    }

	public void SendPlay(int Card)
    {
        PacketWriter pw = BeginSend((int)PacketId.CS_PLAY);
		pw.WriteInt8(Card);
        EndSend();
    }

    private void RecvStartMatch(int id, PacketReader pr)
    {
        int matchID = pr.ReadInt32 ();
        int PlayerNo = pr.ReadInt8 ();
		int Card1 = pr.ReadInt8 ();
		int Card2 = pr.ReadInt8 ();
		int Card3 = pr.ReadInt8 ();
		int Card4 = pr.ReadInt8 ();
		_controller.HandleStartMatch(matchID, PlayerNo, Card1, Card2, Card3, Card4);
    }

    private void RecvOppPlay(int id, PacketReader pr)
    {
		int Card = pr.ReadInt8();
		_controller.HandleOppPlay(Card);
    }

    private void RecvGameover(int id, PacketReader pr)
    {
        _controller.HandleGameOver(pr.ReadInt8() == 0);
    }

	private void RecvDrawCard(int id, PacketReader pr)
	{
		_controller.HandleDrawCard(pr.ReadInt8());
	}
}
