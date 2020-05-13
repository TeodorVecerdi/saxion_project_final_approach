module.exports = class Player {
    constructor(username, avatar, guid, socketId, consent, room = "", location = "") {
        this.username = username;
        this.avatar = avatar;
        this.guid = guid;
        this.socketId = socketId;
        this.consent = consent;
        this.room = room;
        this.location = location;
        this.completedInitialisation = false;
    }

    toJSON() {
        return {
            'username': this.username,
            'avatar': this.avatar,
            'guid': this.guid,
            'socketId': this.socketId,
            'consent': this.consent,
            'room': this.room,
            'location': this.location
        };
    }
};