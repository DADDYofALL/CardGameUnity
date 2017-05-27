'use strict'

//The server, this file is required to use dgt-net server

let network = require('./network.js');

class RemoteProxy {
  constructor(socket) {
    this.socket = socket
  }

  getPeerName() {
    return this.socket.remoteAddress + ":" + this.socket.remotePort
  }

  send(data) {
    this.socket.send(data)
  }
}

let server = {}

//Set proxy object. The object will be used by packet handler
//The method also requires onConnected, onDisconnected which should be put in RemoteProxy class above
server.setRemoteProxyClass = function (remoteProxyClass) {
  this.remoteProxyClass = remoteProxyClass
}

//Set packet handler. The handler is an object that map [packetID] => function(<RemoteProxy>, <packet_reader>)
server.setPacketObject = function (packetObject) {
  this.packetObject = packetObject
}

//Start server on specified port
server.listen = function (port) {
  if (!this.remoteProxyClass) {
    console.log('Remote Proxy Class is not set')
    return
  }
  if (!this.packetObject) {
    console.log('Packet Object is not set')
    return
  }
  network(port, this.remoteProxyClass, this.packetObject)
}


server.RemoteProxy = RemoteProxy
module.exports = server