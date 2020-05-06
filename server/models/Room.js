module.exports = class Room {
    constructor(guid, name, description, code = "", pub = true, nsfw = false) {
        this.guid = guid;
        this.name = name;
        this.description = description;
        this.code = code;
        this.pub = pub;
        this.nsfw = nsfw;
        this.players = [];
    }

    toJSON() {
        return {
            'guid': this.guid,
            'name': this.name,
            'description': this.description,
            'code': this.code,
            'pub': this.pub,
            'nsfw': this.nsfw,
            'players': this.players
        }
    }
};