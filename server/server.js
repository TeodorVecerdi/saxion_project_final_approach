const io = require('socket.io');
let port = process.env.PORT || 8080;
const server = io.listen(port);

let players = {};
let onlinePlayers = {};

let socketToPlayerId = {};
let socketIdToSocket = {};
let playerIdToSocket = {};

// TODO: HANDLE REQUESTS /////////////
// online_players_request ////////////
// TODO: IMPLEMENT ACCOUNT CREATION //

server.on("connection", (socket) => {
    console.info(`Client connected [id=${socket.id}]`);

    socket.on('data_request', data => {
        if (players.hasOwnProperty(data)) {
            onlinePlayers[data] = true;
            socketToPlayerId[socket] = data;
            socketIdToSocket[socket.id] = socket;
            playerIdToSocket[data] = socket;

            socket.emit('data_request_success', players[data]);
            broadcast(socket, 'client_initialised', players[data]);
        } else {
            socket.emit('data_request_fail');
        }
    });
    socket.on('initialised', data => {
        let playerData = JSON.parse(data);

        onlinePlayers[playerData["id"]] = true;
        socketToPlayerId[socket] = playerData["id"];
        socketIdToSocket[socket.id] = socket;
        playerIdToSocket[playerData["id"]] = socket;

        //TODO: Add more fields here --//
        players[playerData["id"]] = {
            id: playerData["id"],
            name: playerData["name"],
            position: playerData["position"]
        };
        //TODO: -----------------------//
        broadcast(socket, 'client_initialised', players[playerData["id"]]);
    });

    socket.on("disconnect", () => {
        onlinePlayers[socketToPlayerId[socket]] = false;
        delete socketToPlayerId[socket];
        delete socketIdToSocket[socket.id];
        delete playerIdToSocket[socketToPlayerId[socket]];

        broadcast(socket, 'client_disconnected', {'playerId': socketToPlayerId[socket]});
        console.info(`Client gone [id=${socket.id}]`);
    });

    socket.on('updated', (data) => {
        let playerData = JSON.parse(data);

        //TODO: Add more fields here --//
        players[socketToPlayerId[socket]]["position"] = playerData["position"];
        //TODO: -----------------------//

        broadcast(socket, 'client_updated', players[socketToPlayerId[socket]]);
    });

    socket.on('debug', (data) => {
        console.log(getDebugData());
        socket.emit('debug', getDebugData())
    });
});

let getDebugData = () => {
    return {
        'players': players,
        'onlinePlayers': onlinePlayers
    };
};

let broadcast = (socket, message, data) => {
    for (let key in onlinePlayers) {
        if (onlinePlayers.hasOwnProperty(key)) {
            let targetSocket = playerIdToSocket[key];
            let value = onlinePlayers[key];
            if (value === true && targetSocket.id !== socket.id) {
                targetSocket.emit(message, data);
                console.info(`broadcasted ${message} to [pID,sID] : [${key},${targetSocket.id}]`);
            }
        }
    }
};
/*setInterval(() => {
    console.log(getDebugData());
}, 5000);*/
console.log(`Server started on port ${port}`);