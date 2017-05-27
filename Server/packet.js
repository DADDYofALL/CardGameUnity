var packet_writer = require('./src/packet_writer')

var packet = {
  CS_FIND: 10001,
  CS_PLAY: 10002,

  SC_START: 20001,
  SC_PLAY: 20002,
  SC_END: 20003,
  SC_DRAW: 20004
};

packet[packet.CS_FIND] = function (remoteProxy, data) {
  if (!data.completed()) return true;
  remoteProxy.findMatch();
}

packet[packet.CS_PLAY] = function (remoteProxy, data) {
  let card = data.read_int8();
  if (!data.completed()) return true;
  remoteProxy.play(card);
}

packet.make_startMatch = function(id, playerId, card1, card2, card3, card4){
  var o = new packet_writer(packet.SC_START);
  o.append_int32(id);
  o.append_int8(playerId);
  o.append_int8(card1);
  o.append_int8(card2);
  o.append_int8(card3);
  o.append_int8(card4);
  o.finish();
  return o.buffer;
}

packet.make_Draw = function(card){
  var o = new packet_writer(packet.SC_DRAW);
  o.append_int8(card);
  o.finish();
  return o.buffer;
}

packet.make_oppPlay = function(card){
  var o = new packet_writer(packet.SC_PLAY);
  o.append_int8(card);
  o.finish();
  return o.buffer;
}

packet.make_gameover = function(win){
  var o = new packet_writer(packet.SC_END);
  o.append_int8(win ? 1 : 0);
  o.finish();
  return o.buffer;
}

module.exports = packet;
