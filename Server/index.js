let server = require('./src/server/server')
let packet = require('./packet')
let RemoteProxy = require('./remoteproxy')

let port = 3000
server.setRemoteProxyClass(RemoteProxy)
server.setPacketObject(packet)
server.listen(port)
console.log('Start listening on ' + port)