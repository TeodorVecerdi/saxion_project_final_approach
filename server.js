const io = require("socket.io");
const server = io.listen(8000);

let sequenceNumberByClient = new Map();
let clientPositions = new Map();

// event fired every time a new client connects:
server.on("connection", (socket) => {
    console.info(`Client connected [id=${socket.id}]`);
    socket.emit('gimme-position');
    clientPositions.set(socket, {x: 0.0, y: 0.0});

    // initialize this client's sequence number
    sequenceNumberByClient.set(socket, 1);

    socket.on("hi", () => {
        console.log(`Client[id=${socket.id}] said hi! :)`)
        socket.emit("hi", "HELLO CLIENT!");
    })

    // when socket disconnects, remove it from the list:
    socket.on("disconnect", () => {
        sequenceNumberByClient.delete(socket);
        console.info(`Client gone [id=${socket.id}]`);
    });

    socket.on('update-position', (data) => {

        console.log("new position incoming");
        var obj = JSON.parse(data);
        clientPositions.set(socket, {x: obj.position.x, y: obj.position.y});
        console.log(clientPositions.get(socket));
        socket.broadcast.emit("new-position", { socket: socket, pos: clientPositions.get(socket)});

        // socket.broad
    });
    
});