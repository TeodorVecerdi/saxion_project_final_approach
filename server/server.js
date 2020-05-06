const io = require('socket.io');
let port = process.env.PORT || 8080;
const server = io.listen(port);

let players = {};
let onlinePlayers = {};

let socketIdToSocket = {};
let socketIdToPlayerId = {};
let playerIdToSocketId = {};

// TODO: HANDLE REQUESTS /////////////
// online_players_request ////////////
// TODO: IMPLEMENT ACCOUNT CREATION //

server.on("connection", socket => {
    console.info(`Client connected [id=${socket.id}]`);

    socket.on('data_request', data => {
        if (players.hasOwnProperty(data)) {
            onlinePlayers[data] = true;
            socketIdToPlayerId[socket.id] = data;
            socketIdToSocket[socket.id] = socket;
            playerIdToSocketId[data] = socket.id;

            socket.emit('data_request_success', players[data]);
            broadcast(socket, 'client_initialised', players[data]);
        } else {
            socket.emit('data_request_fail');
        }
    });
    socket.on('initialised', data => {
        let playerData = JSON.parse(data);

        onlinePlayers[playerData["id"]] = true;
        socketIdToPlayerId[socket.id] = playerData["id"];
        socketIdToSocket[socket.id] = socket;
        playerIdToSocketId[playerData["id"]] = socket.id;

        //TODO: Add more fields here --//
        players[playerData["id"]] = {
            id: playerData["id"],
            name: playerData["name"],
            position: playerData["position"]
        };
        //TODO: -----------------------//
        broadcast(socket, 'client_initialised', players[playerData["id"]]);
    });

    socket.on("disconnect", data => {
        console.info(`Client gone [id=${socket.id}]`);
        broadcast(socket, 'client_disconnected', {'playerId': socketIdToPlayerId[socket.id]});

        let socketId = socket.id;
        let playerId = socketIdToPlayerId[socketId];

        onlinePlayers[playerId] = false;
        delete socketIdToPlayerId[socketId];
        delete socketIdToSocket[socketId];
        delete playerIdToSocketId[playerId];
    });

    socket.on('updated', data => {
        let playerData = JSON.parse(data);
        let playerId = socketIdToPlayerId[socket.id];
        if (players[playerId] === undefined) return;

        //TODO: Add more fields here --//
        players[playerId]["position"] = playerData["position"];
        //TODO: -----------------------//

        broadcast(socket, 'client_updated', players[playerId]);
    });

    socket.on('online_players_request', data => {
        let playerId = socketIdToPlayerId[socket.id];
        console.info(`Player with GUID ${playerId} is requesting online players`);
        let onlinePlayersData = {};
        for(let key in onlinePlayers) {
            if(onlinePlayers.hasOwnProperty(key)) {
                if(onlinePlayers[key] === true && key !== playerId) {
                    onlinePlayersData[key] = players[key];
                }
            }
        }
        console.info(`Sending online players: ${JSON.stringify(onlinePlayersData)}`);
        socket.emit('online_players', onlinePlayersData);
    });

    socket.on('debug', data => {
        console.log(getDebugData());
        socket.emit('debug', getDebugData())
    });
});

let getDebugData = () => {
    return {
        'players': players,
        'onlinePlayers': onlinePlayers,
        's2p': socketIdToPlayerId
    };
};

let broadcast = (socket, message, data) => {
    for (let key in onlinePlayers) {
        if (onlinePlayers.hasOwnProperty(key)) {
            let targetSocketId = playerIdToSocketId[key];
            let targetSocket = socketIdToSocket[targetSocketId];
            if(targetSocket === undefined) continue;

            let value = onlinePlayers[key];
            if (value === true && (socket === undefined || targetSocketId !== socket.id)) {
                targetSocket.emit(message, data);
                // console.info(`broadcasted ${message} to [pID,sID] : [${key},${targetSocket.id}]`);
            }
        }
    }
};
setInterval(() => {
    broadcast(undefined, 'update', {});
}, 25);
console.log(`Server started on port ${port}`);