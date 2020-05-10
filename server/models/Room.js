module.exports = class Room {
    constructor(guid, name, desc, type, code = "", pub = true, nsfw = false) {
        this.guid = guid;
        this.name = name;
        this.desc = desc;
        this.type = type;
        this.code = code;
        this.pub = pub;
        this.nsfw = nsfw;
        this.players = [];
    }

    toJSON() {
        return {
            'guid': this.guid,
            'name': this.name,
            'desc': this.desc,
            'code': this.code,
            'pub': this.pub,
            'nsfw': this.nsfw,
            'players': this.players,
            'type': this.type
        }
    }
};