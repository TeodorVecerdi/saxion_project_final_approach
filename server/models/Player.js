module.exports = class Player {
    constructor(username, avatar, guid, socketId, consent, location = "", room = "") {
        this.username = username;
        this.avatar = avatar;
        this.guid = guid;
        this.socketId = socketId;
        this.consent = consent;
        this.location = location;
        this.room = room;
    }

    toJSON() {
        return {
            'username': this.username,
            'avatar': this.avatar,
            'guid': this.guid,
            'socketId': this.socketId,
            'consent': this.consent,
            'location': this.location,
            'room': this.room
        };
    }
};