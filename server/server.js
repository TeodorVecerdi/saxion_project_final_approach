const io = require('socket.io');
let port = process.env.PORT || 8080;
const server = io.listen(port);

const Player = require('./models/Player');
const Room = require('./models/Room');

let players = {};
let rooms = {};

let socketIdToSocket = {};
let socketIdToPlayerId = {};
let playerIdToSocketId = {};

server.on("connection", socket => {
    console.info(`Client connected [id=${socket.id}]`);
    socketIdToSocket[socket.id] = socket;

    socket.emit("request_account");

    socket.on("request_account_success", data => {
        let accountData = JSON.parse(data);
        let player = new Player(accountData['username'], accountData['avatar'], accountData['guid'], socket.id, accountData["room"]);
        players[player.guid] = player;

        socketIdToPlayerId[socket.id] = player.guid;
        playerIdToSocketId[player.guid] = socket.id;
        
        console.info(`Got player data: ${data}`);
    });

    socket.on("request_rooms", data => {
        socket.emit("request_rooms_success", roomsToJSON());
    });

    socket.on("create_room", data => {
        let roomData = JSON.parse(data);
        let player = players[socketIdToPlayerId[socket.id]];
        let room = new Room(roomData['guid'], roomData['name'], roomData['desc'], roomData['type'], "", roomData['pub'], roomData['nsfw']);
        if (room.pub === false) {
            room.code = roomData['code'];
        }
        room.players.push(player);
        player.room = room.guid;
        rooms[room.guid] = room;
        socket.emit("create_room_success", roomData);
    });

    socket.on("join_room", data => {
        let roomData = JSON.parse(data);
        let room = rooms[roomData['guid']];
        let player = players[socketIdToPlayerId[socket.id]];

        if (room === undefined)
            socket.emit("join_room_failed", {'code': 0, 'message': 'Room does not exist.'});
        else if (room.pub === false && roomData['code'] !== room.code)
            socket.emit("join_room_failed", {'code': 1, 'message': 'Invalid room code.'});
        else {
            room.players.push(player.guid);
            player.room = room.guid;
            socket.emit("join_room_success", {'guid': room.guid});
            broadcast(socket, 'client_joined', player.toJSON(), room.guid);
        }
    });

    socket.on("send_message", data => {
        broadcast(socket, "new_message", {'message': data, 'from': players[socketIdToPlayerId[socket.id]].toJSON()})
    });

    socket.on("disconnect", data => {
        console.info(`Client gone [id=${socket.id}]`);
        let socketId = socket.id;
        let playerId = socketIdToPlayerId[socketId];
        broadcast(socket, 'client_disconnected', players[playerId].toJSON(), players[playerId].room);

        delete players[playerId];
        delete socketIdToPlayerId[socketId];
        delete socketIdToSocket[socketId];
        delete playerIdToSocketId[playerId];

        let roomsToDelete = [];
        for (let guid in rooms) {
            let room = rooms[guid];
            room.players = room.players.filter(player => player.guid !== playerId);
            if (!room.players || !room.players.length) {
                roomsToDelete.push(guid);
            }
        }
        for (let guid in roomsToDelete) {
            // noinspection JSUnfilteredForInLoop
            delete rooms[guid];
        }
    });
});

let broadcast = (socket, message, data, room = "") => {
    for (let guid in players) {
        if (players.hasOwnProperty(guid)) {
            let targetSocketId = playerIdToSocketId[guid];
            let targetSocket = socketIdToSocket[targetSocketId];
            if (targetSocket === undefined) continue;

            let player = players[guid];
            if ((room === "" || player.room === room) && (socket === undefined || targetSocketId !== socket.id)) {
                targetSocket.emit(message, data);
            }
        }
    }
};

let roomsToJSON = () => {
    let jsonRooms = {};
    for (let key in rooms) {
        jsonRooms[key] = rooms[key].toJSON();
    }
    return jsonRooms;
};

let playersToJSON = () => {
    let jsonPlayers = {};
    for (let key in players) {
        jsonPlayers[key] = players[key].toJSON();
    }
    return jsonPlayers;
};

/*setInterval(() => {
    broadcast(undefined, 'update', {});
}, 25);*/

console.log(`Server started on port ${port}`);