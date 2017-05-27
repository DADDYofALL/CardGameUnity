let server = require('./src/server/server')
let packet = require('./packet')
let matchmaker = require('./matchmaker')

////////////////////////////////////////////////////////////////////////////////
// Remote Proxy (Server Side)
////////////////////////////////////////////////////////////////////////////////

class RemoteProxy extends server.RemoteProxy {

  onConnected() {
    console.log("RemoteProxy There is a connection from " + this.getPeerName())
  }

  onDisconnected() {
    console.log("RemoteProxy Disconnected from " + this.getPeerName())
    matchmaker.removeRemote(this);
  }

  findMatch(){
    matchmaker.waitForMatch(this)
  }

  play(card){
    matchmaker.play(this,card)
  }
}

module.exports = RemoteProxy