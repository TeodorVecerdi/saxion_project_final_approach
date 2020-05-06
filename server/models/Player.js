module.exports = class Player {
    constructor(username, avatar, guid, socketId, room = "") {
        this.username = username;
        this.avatar = avatar;
        this.guid = guid;
        this.socketId = socketId;
        this.room = room;
    }

    toJSON() {
        return {
            'username': this.username,
            'avatar': this.avatar,
            'guid': this.guid,
            'socketId': this.socketId,
            'room': this.room
        };
    }
};