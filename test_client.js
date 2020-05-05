let io = require('socket.io-client');
let server = io.connect('http://localhost:8080');

server.on('connect', data => {
   console.log("connected");
});

server.on('initialize', data => {
    console.log("initialization required");
});