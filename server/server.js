const io = require('socket.io');
let port = process.env.PORT || 8080;
const server = io.listen(port);

const Player = require('./models/Player');
const Room = require('./models/Room');
const VotingSession = require('./models/VotingSession');
const MostLikelyToMinigame = require('./models/MostLikelyToMinigame');

let players = {};
let rooms = {};
let activeVotingSessions = {};
let activeMostLikelyToMinigames = {};

let socketIdToSocket = {};
let socketIdToPlayerId = {};
let playerIdToSocketId = {};

server.on("connection", socket => {
    console.info(`Client connected [id=${socket.id}]`);
    socketIdToSocket[socket.id] = socket;

    socket.emit("request_account");

    socket.on("request_account_success", data => {
        let accountData = JSON.parse(data);
        let player = new Player(accountData['username'], accountData['avatar'], accountData['guid'], socket.id, accountData['consent'], accountData["room"]);
        players[player.guid] = player;

        socketIdToPlayerId[socket.id] = player.guid;
        playerIdToSocketId[player.guid] = socket.id;
    });

    socket.on("set_location", data => {
        let playerId = socketIdToPlayerId[socket.id];
        players[playerId].location = data;
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
        room.players.push(player.guid);
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
            socket.emit("join_room_success", room.toJSON());
            broadcast(socket, 'client_joined', player.toJSON(), room.guid);
        }
    });

    socket.on('leave_room', data => {
        let playerId = socketIdToPlayerId[socket.id];
        let player = players[playerId];
        broadcast(socket, 'client_disconnected', player.toJSON(), player.room);
        player.room = "none";
        // clean-up
        rooms[players[playerId].room].players.splice(rooms[players[playerId].room].players.indexOf(playerId));
        if (!rooms[players[playerId].room].players || !rooms[players[playerId].room].players.length) {
            delete rooms[players[playerId].room];
        }
    });

    socket.on('request_players', data => {
        let requestedPlayers = {};
        let playerId = socketIdToPlayerId[socket.id];
        let room = rooms[players[playerId].room];
        for(let i = 0; i < room.players.length; i++) {
            let player = room.players[i];
            if(player !== playerId) {
                requestedPlayers[player] = players[player].toJSON();
            }
        }
        socket.emit('request_players_success', requestedPlayers);
    });

    socket.on('start_voting_session', data => {
       let voteData = JSON.parse(data);
       let playerId = socketIdToPlayerId[socket.id];
       let room = rooms[players[playerId].room];
       let votes = {};
       for(let player in room.players) {
           votes[player] = -1;
       }
       let votingSession = new VotingSession(voteData['guid'], voteData['creator'], voteData['reason'], votes);
       activeVotingSessions[votingSession.guid] = votingSession;
       broadcast(socket, 'started_voting_session', votingSession.toJSON(), players[playerId].room)
    });

    socket.on('voted', data => {
       let voteData = JSON.parse(data);
       let voteResponse = voteData['response'];
       let votingSessionId = voteData['guid'];
       let playerId = socketIdToPlayerId[socket.id];
       let votingSession = activeVotingSessions[votingSessionId];
        votingSession.votes[playerId] = voteResponse;
       let unvotedCount = 0;
       let yesCount = 0;
       let noCount = 0;
       for(let key in activeVotingSessions[votingSessionId].votes) {
           if(votingSession.votes[key] === -1) unvotedCount++;
           if(votingSession.votes[key] === 1) yesCount++;
           if(votingSession.votes[key] === 0) noCount++;
       }
       broadcast(socket, 'update_voting_session', {'votingSession': votingSession.toJSON(), 'unvotedCount': unvotedCount, 'yesCount': yesCount, 'noCount': noCount}, players[playerId].room);
       if(unvotedCount === 0) {
            broadcast(socket, 'finish_voting_session', {'votingSession': votingSession.toJSON(), 'yesCount': yesCount, 'noCount': noCount}, players[playerId].room);
            socket.emit('finish_voting_session', {'votingSession': votingSession.toJSON(), 'yesCount': yesCount, 'noCount': noCount});
            delete activeVotingSessions[votingSessionId];
       }
    });

    socket.on('start_minigame_1', data => {
        let minigameData = JSON.parse(data);
        let minigame = new MostLikelyToMinigame(minigameData['gameGuid'], minigameData['roomGuid'], minigameData['ownerGuid']);
        let playerId = socketIdToPlayerId[socket.id];
        activeMostLikelyToMinigames[minigame.gameGuid] = minigame;
        broadcast(socket, 'started_minigame_1', minigame.toJSON(), players[playerId].room);
    });
    socket.on('voted_minigame_1', data => {
        let playerId = socketIdToPlayerId[socket.id];
        broadcast(socket, 'voted_minigame_1', JSON.parse(data), players[playerId].room);
    });
    socket.on('request_minigame_1', data => {
        let minigameData = JSON.parse(data);
        let question = activeMostLikelyToMinigames[minigameData['gameGuid']].getQuestion();
        let playerId = socketIdToPlayerId[socket.id];
        socket.emit('request_minigame_1', {'question': question, 'gameGuid': minigameData['gameGuid']});
        broadcast(socket, 'request_minigame_1', {'question': question, 'gameGuid': minigameData['gameGuid']}, players[playerId].room);
    });
    socket.on('results_minigame_1', data => {
        let playerId = socketIdToPlayerId[socket.id];
        broadcast(socket, 'results_minigame_1', data, players[playerId].room);
    });
    socket.on('finish_minigame_1', data => {
        let minigameData = JSON.parse(data);
        let playerId = socketIdToPlayerId[socket.id];
        delete activeMostLikelyToMinigames[minigameData['gameGuid']];
        broadcast(socket, 'finished_minigame_1', minigameData, players[playerId].room);
    });

    socket.on("send_message", data => {
        broadcast(socket, "new_message", {'message': data, 'from': players[socketIdToPlayerId[socket.id]].toJSON()}, players[socketIdToPlayerId[socket.id]].room)
    });

    socket.on("disconnect", data => {
        console.info(`Client gone [id=${socket.id}]`);
        let socketId = socket.id;
        let playerId = socketIdToPlayerId[socketId];
        broadcast(socket, 'client_disconnected', players[playerId].toJSON(), players[playerId].room);

        // clean-up
        rooms[players[playerId].room].players.splice(rooms[players[playerId].room].players.indexOf(playerId),1);
        if (rooms[players[playerId].room].players.length == 0) {
            console.info(`Deleted room with guid ${players[playerId].room}`);
            delete rooms[players[playerId].room];
        }

        delete players[playerId];
        delete socketIdToPlayerId[socketId];
        delete socketIdToSocket[socketId];
        delete playerIdToSocketId[playerId];
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